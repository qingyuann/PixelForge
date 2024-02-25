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
		var camera = _contexts.game.CreateEntity();
		camera.AddComponentName( "mainCamera" );
		camera.AddComponentSize( 0.1f, 0.1f );
		camera.AddComponentRotation( 0 );
		camera.AddComponentPosition( 0f, 0f, 1 );
		camera.AddComponentCamera( 0, true, 2f );
		camera.AddComponentBasicMove( true, 0.0003f );
		// camera.AddLightShadowLight( true, new[]{ 0 }, 0, 1f, 2f, 0.5f, new Vector3( 0.5f, 0.5f, 0f ), 1f, 15f );

		#region Light
		var lightSize =15;
		GameEntity[,] lights = new GameEntity[lightSize, lightSize];
		for( int i = 0; i < lightSize; i++ ) {
			for( int j = 0; j < lightSize; j++ ) {
				lights[i, j] = _contexts.game.CreateEntity();
				lights[i, j].AddComponentPosition( 
					RandomTool.Range( 0.1f,(float)lightSize*2 ), 
					0.25f + RandomTool.Range( 0, lightSize*2) * 0.5f, 
					0 );
				lights[i, j].AddLightShadowLight( true, new[]{ 0 }, j + i * j, 0.8f, 0.3f + RandomTool.Float() * 2f, 0.5f, new Vector3( RandomTool.Float(), RandomTool.Float(), RandomTool.Float() ), 2f * RandomTool.Float() + 0.8f, 5f * RandomTool.Float() );
			}
		}

		#endregion
		
		#region quad
		var listPic = new List<string>(){
			"red.png",
			"green.png",
			"blue.png",
			"yellow.png"
		};
		var edgesize = 20;
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