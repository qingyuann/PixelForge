using Entitas;
using Render;
using System.Numerics;

namespace PixelForge;

public class HierarchySystem : IInitializeSystem {
	readonly Contexts _contexts;

	public HierarchySystem( Contexts contexts ) {
		_contexts = contexts;
	}

	public void Initialize() {
		InitEntity();
	}

	void InitEntity() {
		var camera = _contexts.game.CreateEntity();
		camera.AddComponentName( "mainCamera" );
		camera.AddComponentPosition( 0, 0 ,0);
		camera.AddComponentCamera( 0, true, 0.5f );
		camera.AddComponentBasicMove( true,0.001f ); 
		
		
		
		var quad1 = _contexts.game.CreateEntity();
		quad1.AddComponentName( "quad1" );
		quad1.AddComponentPosition( 0, 0 ,0);
		quad1.AddComponentSize( 0.2f ,0.2f);
		quad1.AddComponentRotation( 0 );
		quad1.AddComponentRenderSinglePara( true,new Vector3( 1f,1f,1f ),"silk.png",0 );
		// quad1.AddComponentBasicMove( true,0.001f );
	}
}