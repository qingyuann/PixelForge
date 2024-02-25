using Entitas;
using Light;
using pp;
using Render;
using Render.PostEffect;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Component;

namespace PixelForge.Light;

public class LightSystem : IExecuteSystem, IInitializeSystem {
	public static LightSystem Instance { get; private set; }

	static Dictionary<Type, (LightEffectComputer, List<(ILightComponent, PositionComponent)>)> _lightComputers = new Dictionary<Type, (LightEffectComputer, List<(ILightComponent, PositionComponent)>)>(){
		{ typeof( GlobalLightComponent ), ( new ShadowLightComputer(), new List<(ILightComponent, PositionComponent)>() ) },
		{ typeof( CircleLightComponent ), ( new CircleLightComputer(), new List<(ILightComponent, PositionComponent)>() ) },
		{ typeof( ShadowLightComponent ), ( new ShadowLightComputer(), new List<(ILightComponent, PositionComponent)>() ) },
	};

	List<IMatcher<GameEntity>> _matchers = new List<IMatcher<GameEntity>>(){
		GameMatcher.LightGlobalLight,
		GameMatcher.LightCircleLight,
		GameMatcher.LightShadowLight,
	};

	/************************************************************************/
	IGroup<GameEntity> _lightsGroup;

	Contexts _contexts;


	public LightSystem( Contexts contexts ) {
		_contexts = contexts;
		if( Instance is null ) {
			Instance = this;
		} else {
			throw new Exception( "LightSystem is a singleton" );
		}
	}

	public void Initialize() {
		_lightsGroup = _contexts.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentPosition ).AnyOf( _matchers.ToArray() ) );
		foreach( var e in _lightsGroup ) {
			AddNewComputer( e );
		}

		_lightsGroup.OnEntityAdded += ( _, entity, _, _ ) => {
			AddNewComputer( entity );
		};
	}

	void AddNewComputer( GameEntity e ) {
		foreach( var c in e.GetComponents() ) {
			if( c is ILightComponent lightComponent ) {
				if( _lightComputers.TryGetValue( c.GetType(), out (LightEffectComputer, List<(ILightComponent, PositionComponent)>) com ) ) {
					com.Item2.Add( ( lightComponent, e.componentPosition ) );
				}
			}
		}
	}

	public void Execute() {
	}


	public static void RenderLights( int layer, RenderTexture rt ) {

		// //Render Global Lights
		// RenderLight( rt, layer, typeof( GlobalLightComponent ) );
		//
		// //Render Other Lights
		// var otherLights = _computers.Where( c => c.Key != typeof( GlobalLightComponent ) && c.Key != typeof( ShadowLightComponent ) );
		// foreach( var light in otherLights ) {
		// 	RenderLight( rt, layer, light.Key );
		// }

		//Render Shadow Lights
		RenderLight( rt, layer, typeof( ShadowLightComponent ) );
	}

	static void RenderLight( RenderTexture rt, int layer, Type type ) {
		if( _lightComputers.TryGetValue( type, out (LightEffectComputer, List<(ILightComponent, PositionComponent)>) com ) ) {
			var computer = com.Item1;
			var param = com.Item2;
			param = param.Where( x => x.Item1.Layers.Contains( layer ) ).OrderBy( p => p.Item1.LightOrder ).ToList();
			if( param.Count == 0 ) {
				return;
			}
			computer.SetParams( param );
			computer.Render( rt );
		}
	}
}