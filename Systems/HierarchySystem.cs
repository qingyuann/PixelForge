using Entitas;
using Render;
using Render.PostEffect;
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
		camera.AddComponentSize( 0.1f, 0.1f );
		camera.AddComponentRotation( 0 );
		camera.AddComponentPosition( 0, 0, 1 );
		camera.AddComponentCamera( 0, true, 1.000f );
		camera.AddComponentBasicMove( true, 0.0005f );
		camera.AddMatRenderSingle( true, 0, null );
		camera.AddMatPara( null, new Dictionary<string, object>(){
			{ "MainTex", "red.png" }
		} );
		camera.AddLightShadowLight( true, new[]{ 0 }, 0, new Vector2( 0f, 0f ), 0.5f, 1f, 1f, new Vector3( 1, 1, 1 ), 1f, 1f );


		
		// var postProcess = _contexts.game.CreateEntity();
		// postProcess.AddppLightSetting( true );
		// postProcess.AddppBloom( true, new[]{ 0 }, 2f, 5, 0.8f, 5f, new BloomComputer() );
		// postProcess.AddppGaussianBlur( true, new[]{ 1 }, 5f, 5, new GaussianBlurComputer() );


		#region Light
		// var shadowLight = _contexts.game.CreateEntity();
		// shadowLight.AddLightShadowLight( true, new[]{ 0 }, 0, new Vector2( 0f, 0f ), 0.5f, 1f, 1f, new Vector3( 1, 1, 1 ), 1f, 1f );
		//
		// // var globalLight = _contexts.game.CreateEntity();
		// globalLight.AddLightGlobalLight( true, new[]{ 0 }, 0, Vector3.One, 1f );
		#endregion


		#region quad
		var quad0 = _contexts.game.CreateEntity();
		quad0.AddComponentName( "quad0" );
		quad0.AddComponentPosition( 0, 0, -0.1f );
		quad0.AddComponentSize( 1f, 1f );
		quad0.AddComponentRotation( 0 );
		quad0.AddMatRenderSingle( true, 0, null );
		quad0.AddMatPara( null, new Dictionary<string, object>(){
			{ "MainTex", "blue.png" }
		} );

		var quad1 = _contexts.game.CreateEntity();
		quad1.AddComponentName( "quad1" );
		quad1.AddComponentPosition( 0, 0, 1 );
		quad1.AddComponentSize( 0.1f, 0.1f );
		quad1.AddComponentRotation( 0 );
		quad1.AddMatRenderSingle( true, 0, null );
		quad1.AddMatPara( null, new Dictionary<string, object>(){
			{ "MainTex", "yellow.png" }
		} );

		var quad2 = _contexts.game.CreateEntity();
		quad2.AddComponentName( "quad2" );
		quad2.AddComponentPosition( -1f, 1f, 0 );
		quad2.AddComponentSize( 0.1f, 0.1f );
		quad2.AddComponentRotation( 0 );
		quad2.AddMatRenderSingle( true, 0, null );
		quad2.AddMatPara( null, new Dictionary<string, object>(){
			{ "MainTex", "green.png" }
		} );

		var quad3 = _contexts.game.CreateEntity();
		quad3.AddComponentName( "quad3" );
		quad3.AddComponentPosition( 1, -1, 0 );
		quad3.AddComponentSize( 0.1f, 0.1f );
		quad3.AddComponentRotation( 0 );
		quad3.AddMatRenderSingle( true, 0, null );


		var quad4 = _contexts.game.CreateEntity();
		quad4.AddComponentName( "quad4" );
		quad4.AddComponentPosition( 1f, 1, -0f );
		quad4.AddComponentSize( 0.1f, 0.1f );
		quad4.AddComponentRotation( 0 );
		quad4.AddMatRenderSingle( true, 0, null );
		quad4.AddMatPara( null, new Dictionary<string, object>(){
			{ "MainTex", "red.png" }
		} );

		var quad5 = _contexts.game.CreateEntity();
		quad5.AddComponentName( "quad5" );
		quad5.AddComponentPosition( 2f, 0f, 0 );
		quad5.AddComponentSize( 0.1f, 0.1f );
		quad5.AddComponentRotation( 0 );
		quad5.AddMatRenderSingle( true, 0, null );
		quad5.AddMatPara( null, new Dictionary<string, object>(){
			{ "MainTex", "red.png" }
		} );

		var quad6 = _contexts.game.CreateEntity();
		quad6.AddComponentName( "quad6" );
		quad6.AddComponentPosition( -2f, 0f, -1 );
		quad6.AddComponentSize( 0.1f, 0.1f );
		quad6.AddComponentRotation( 0 );
		quad6.AddMatRenderSingle( true, 0, null );
		quad6.AddMatPara( null, new Dictionary<string, object>(){
			{ "MainTex", "blue.png" }
		} );
		#endregion

	}
}