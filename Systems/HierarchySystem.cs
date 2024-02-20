using Entitas;
using Render;
using Render.PostEffect;
using System.Numerics;

namespace PixelForge;

public class HierarchySystem : IInitializeSystem {
	readonly Contexts _contexts;

	public HierarchySystem( Contexts contexts ) {
		_contexts = contexts;
		var a = contexts.allContexts;
		foreach( IContext c in a ) {
			Debug.Log( c.contextInfo.name );
		}
	}

	public void Initialize() {
		InitEntity();
	}

	void InitEntity() {
		var camera = _contexts.game.CreateEntity();
		camera.AddComponentName( "mainCamera" );
		camera.AddComponentSize( 0.1f, 0.1f );
		camera.AddComponentRotation( 0 );
		camera.AddComponentPosition( 0, 0, 0 );
		camera.AddComponentCamera( 0, true, 0.5f );
		camera.AddComponentBasicMove( true, 0.0005f );
		//camera.AddMatRenderSingle( true, 0, null );

		var globalLight = _contexts.game.CreateEntity();
		globalLight.AddGlobalPostProcessGroup( true, new int[]{ 0, 1 }, 0.5f,
			new PostProcessComputer[]{ new BloomComputer() } );


		// var quad1 = _contexts.game.CreateEntity();
		// quad1.AddComponentName( "quad1" );
		// quad1.AddComponentPosition( 0, 0, 0 );
		// quad1.AddComponentSize( 0.2f, 0.2f );
		// quad1.AddComponentRotation( 0 );
		// quad1.AddMatRenderSingleTrigger( true, 0 );
		//
		// //
		// var quad2 = _contexts.game.CreateEntity();
		// quad2.AddComponentName( "quad2" );
		// quad2.AddComponentPosition( 1, 0, 0 );
		// quad2.AddComponentSize( 0.2f, 0.2f );
		// quad2.AddComponentRotation( 0 );
		// quad2.AddMatRenderSingleTrigger( true, 1 );
		//

		var quad3 = _contexts.game.CreateEntity();
		quad3.AddComponentName( "quad3" );
		quad3.AddComponentPosition( 0, 0, 0 );
		quad3.AddComponentSize( 2f, 2f );
		quad3.AddComponentRotation( 0 );
		quad3.AddMatRenderSingle( true, 0, null );
		quad3.isComponentCellAutoTexture = true;
	}

}