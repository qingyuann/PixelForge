using Entitas;
using Render;
using Render.PostEffect;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace PixelForge;

public sealed class PostProcessSystem : IInitializeSystem, IExecuteSystem {
	//set a singleton
	static PostProcessSystem _postProcessSystem;
	public static PostProcessSystem Instance {
		get {
			if( _postProcessSystem is null ) {
				_postProcessSystem = new PostProcessSystem();
			}
			return _postProcessSystem;
		}
	}



	Contexts _contexts;
	static IGroup<GameEntity> _ppEntityGroup;
	Dictionary<IComponent, PostProcessComputer> _computers = new Dictionary<IComponent, PostProcessComputer>();


	public void Initialize() {
		_contexts = Contexts.sharedInstance;
		_ppEntityGroup = _contexts.game.GetGroup( GameMatcher.GlobalPostProcessGroup );
		foreach( var e in _ppEntityGroup.GetEntities() ) {
			SetUpPostEffects( e );
		}
		_ppEntityGroup.OnEntityAdded += ( _, entity, _, _ ) => {
			SetUpPostEffects( entity );
		};
	}

	void SetUpPostEffects( GameEntity entity ) {
		var ppEntity = entity.globalPostProcessGroup;
		if( ppEntity.Enabled ) {
			foreach( var c in ppEntity.Computer ) {

			}
		}
	}

	public void Execute() {

	}

	public static void RenderPostProcess( int layer, RenderTexture rt ) {
		if( _ppEntityGroup == null ) {
			return;
		}
		foreach( var e in _ppEntityGroup ) {
			var pp = e.globalPostProcessGroup;
			if( pp.Enabled && pp.Layers.Contains( layer ) ) {
				foreach( var c in pp.Computer ) {
					c.Render( rt );
				}
			}
		}
	}
}