using Entitas;
using PixelForge.Tools;
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
		RandomTool.SetSeed( 123 );

		var camera = _contexts.game.CreateEntity();
		camera.AddComponentName( "mainCamera" );
		camera.AddComponentSize( 0.1f, 0.1f );
		camera.AddComponentRotation( 0 );
		camera.AddComponentPosition( 1.25f, 1.25f, 0 );
		camera.AddComponentCamera( 0, true, 2f );
		// camera.AddLightShadowLight( true, new[]{ 0 }, 0, 10f, 2f, 1f, new Vector3( 0.5f, 0.5f, 0f ), 0f, 0f );
		// camera.AddMatRenderSingle( true, 1, null );
		camera.AddMatPara( null, new System.Collections.Generic.Dictionary<string, object>(){
			{ "MainTex", "circle.png" }
		} );

		var li= _contexts.game.CreateEntity();
		li.AddComponentPosition( 2f, 2f, 0 );
		li.AddComponentSize( 0.1f, 0.1f );
		li.AddComponentRotation( 0 );
		li.AddLightShadowLight( true, new[]{ 0 }, 0, 10f, 1f, 0.8f, new Vector3( 0.5f, 0.5f, 0f ), 0f, 0f );
		li.AddMatRenderSingle( true, 1, null );
		li.AddMatPara( null, new System.Collections.Generic.Dictionary<string, object>(){
			{ "MainTex", "circle.png" }
		} );
		li.AddComponentBasicMove( true, 0.0003f );

		// #region Light
		// var lightSize =10;
		// GameEntity[,] lights = new GameEntity[lightSize, lightSize];
		// for( int i = 0; i < lightSize; i++ ) {
		// 	for( int j = 0; j < lightSize; j++ ) {
		// 		lights[i, j] = _contexts.game.CreateEntity();
		// 		lights[i, j].AddComponentPosition( 
		// 			RandomTool.Range( 0f,10f ), 
		// 			 RandomTool.Range( 0f, 10f) , 
		// 			0 );
		// 		lights[i, j].AddLightShadowLight( true, new[]{ 0 }, j + i * j+1, 2f, RandomTool.Float() * 1f, 0.4f, new Vector3( RandomTool.Float(), RandomTool.Float(), RandomTool.Float() ), 2f * RandomTool.Float() + 0.8f, 15f * RandomTool.Float() );
		// 	}
		// }
		// #endregion

		#region quad
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
				quads[i, j].AddComponentPosition( i * 0.25f*RandomTool.Range( 0, 5), RandomTool.Range( 0, 15)*j * 0.25f, 0 );
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