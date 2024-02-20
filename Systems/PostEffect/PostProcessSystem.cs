using Entitas;
using pp;
using Render;
using Render.PostEffect;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace PixelForge;

public sealed class PostProcessSystem : IInitializeSystem {
	//set a singleton
	public static PostProcessSystem Instance;

	// pair of post process component and its computer
	static Dictionary<Type, Type> _ppTypes = new Dictionary<Type, Type>(){
		{ typeof( BloomComponent ), typeof( BloomComputer ) },
		{ typeof( GaussianBlurComponent ), typeof( GaussianBlurComputer ) },
	};

	Contexts _contexts;
	GameEntity _ppEntity;
	//store all post process components
	Dictionary<IComponent, PostProcessComputer> _computers;

	public PostProcessSystem( Contexts contexts ) {
		_contexts = contexts;
		_computers = new Dictionary<IComponent, PostProcessComputer>();
		if( Instance is null ) {
			Instance = this;
		}else {
			throw new Exception( "PostProcessSystem is a singleton" );
		}
	}

	public void Initialize() {
		_ppEntity = _contexts.game.ppLightSettingEntity;
		if( _ppEntity is null ) {
			return;
		}
		foreach( var c in _ppEntity.GetComponents() ) {
			foreach( var pp in _ppTypes ) {
				if( pp.Key == c.GetType() ) {
					var computer = (PostProcessComputer)Activator.CreateInstance( pp.Value )!;
					computer!.SetParams( c );
					_computers.Add( c, computer );
					break;
				}
			}
		}
	}

	public static void RenderPostProcess( int layer, RenderTexture rt ) {
		if( Instance._ppEntity is null ) {
			return;
		}
		//render all post process components
		foreach( var c in Instance._computers ) {
			if( c.Key is IPostProcessingComponent pp ) {
				if( pp.Enabled && pp.Layers.Contains( layer ) ) {
					c.Value.Render( rt );
				}
			}
		}
	}
}