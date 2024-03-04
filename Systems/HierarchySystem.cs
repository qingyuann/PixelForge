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
		camera.AddLightShadowLight( true, new[]{ 0 }, 0, 5f, 3f, 0.8f, new Vector3( 0.5f, 0.5f, 0f ), 0.7f, 0f );
		camera.AddMatRenderSingle( true, 1, null );
		camera.AddMatPara( null, new (){
			{ "MainTex", ("circle.png",default) }
		} );
		camera.AddComponentBasicMove( true, 0.0003f );

		
		var water= _contexts.game.CreateEntity();
		water.AddComponentRotation( 0 );
		water.AddComponentPosition( 0, 0, 0 );
		water.AddComponentSize( 1, 1);
		water.AddMatRenderSingle( true, 0, new RenderQuad( vertShaderName:"QuadBasic.vert", fragShaderName:"WaterElement.frag") );
		water.AddMatPara( null, new (){
			{ "MainTex", ("WaterPanel.png",default) },
			{"PerlinNoise", ("PerlinNoise.png",new TexParam(true,true))}
		} );
		
		
		
		// #region Light
		// var lightSize =15;
		// GameEntity[,] lights = new GameEntity[lightSize, lightSize];
		// for( int i = 0; i < lightSize; i++ ) {
		// 	for( int j = 0; j < lightSize; j++ ) {
		// 		lights[i, j] = _contexts.game.CreateEntity();
		// 		lights[i, j].AddComponentPosition( 
		// 			RandomTool.Range( 0f,10f ), 
		// 			 RandomTool.Range( 0f, 10f) , 
		// 			0 );
		// 		lights[i, j].AddLightShadowLight( true, new[]{ 0 }, j + i * j+1, 2f, RandomTool.Float() * 1f, 0.3f, new Vector3( RandomTool.Float(), RandomTool.Float(), RandomTool.Float() ), 2f * RandomTool.Float() + 0.8f, 15f * RandomTool.Float() );
		// 	}
		// }
		// #endregion
		//
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
				quads[i, j].AddMatPara( null, new (){
					{ "MainTex", (listPic[Tools.RandomTool.Range( 0, 4 )],default) }
				} );
			}
		}
		#endregion
	}
}