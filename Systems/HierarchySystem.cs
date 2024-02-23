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
		camera.AddComponentCamera( 0, true, 2f );
		camera.AddComponentBasicMove( true, 0.0003f );
		camera.AddLightShadowLight( true, new[]{ 0 }, 0, 1f, 2f, 0.5f, new Vector3( 0.5f, 0.5f, 0f ), 2f, 15f );

		// var shadowLight = _contexts.game.CreateEntity();
		// shadowLight.AddComponentPosition( 0, 0, 1 );
		// shadowLight.AddLightShadowLight( true, new[]{ 0 }, 0, 0.5f, 2f, 0.5f, new Vector3( 0.5f, 0.5f, 0f ), 2f, 15f );


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
		//
		// var quad0 = _contexts.game.CreateEntity();
		// quad0.AddComponentName("quad0");
		// quad0.AddComponentPosition(0, 0, -0.1f);
		// quad0.AddComponentSize(0.5f, 0.5f);
		// quad0.AddComponentRotation(0);
		// quad0.AddMatRenderSingle(true, 0, null);
		// quad0.AddMatPara(null, new Dictionary<string, object>()
		// {
		//     { "MainTex", "2_4circle.png" }
		// });

		var listPic = new List<string>(){
			"red.png",
			"green.png",
			"blue.png",
			"yellow.png"
		};
		var edgesize = 10;
		GameEntity[,] quads = new GameEntity[edgesize, edgesize];
		for( int i = 0; i < edgesize; i++ ) {
			for( int j = 0; j < edgesize; j++ ) {
				quads[i, j] = _contexts.game.CreateEntity();
				quads[i, j].AddComponentPosition( i * 0.5f, j * 0.5f, 0 );
				quads[i, j].AddComponentSize( 0.1f, 0.1f );
				quads[i, j].AddComponentRotation( 0 );
				quads[i, j].AddMatRenderSingle( true, 0, null );
				quads[i, j].AddMatPara( null, new Dictionary<string, object>(){
					{ "MainTex", listPic[Tools.RandomTool.Range( 0, 4 )] }
				} );
			}
		}
		#endregion
	}
}