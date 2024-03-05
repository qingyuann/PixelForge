using Entitas;
using PixelForge.Tools;
using Render;
using Render.PostEffect;
using System.Numerics;
using PixelForge.Physics;

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
		camera.AddComponentCamera( 0, true, 1f );
		camera.AddComponentPosition( 0f, 0f, 0 );

		var bg = _contexts.game.CreateEntity();
		bg.AddComponentName( "bg" );
		bg.AddComponentPosition( 0, 0, 0 );
		bg.AddComponentSize( 1f, 1f );
		bg.AddComponentRotation( 0 );
		bg.AddMatRenderSingle( true, 0, null );
		bg.AddMatPara( null, new(){
			{ "MainTex", ( "bg.jpg", default ) }
		} );


		var light1 = _contexts.game.CreateEntity();
		light1.AddComponentPosition( 0f, 0f, 0 );
		light1.AddLightGlobalLight( true, new[]{ 1 }, 0, new Vector3( 0.2f, 0.2f, 0.4f ), 0.1f, 1f );
		
		var light2 = _contexts.game.CreateEntity();
		light2.AddComponentPosition( 0f, 0f, 0 );
		light2.AddLightGlobalLight( true, new[]{ 1 }, 0, new Vector3( 0.8f, 0.7f, 0.5f ) * 1.5f, 0.4f, 1f );

		/*
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

		var quad2 = _contexts.game.CreateEntity();
		quad2.AddComponentName( "quad2" );
		quad2.AddComponentPosition( -1.5f, -2f, 0 );
		quad2.AddComponentSize( 0.2f, 0.2f );
		quad2.AddComponentRotation( 0 );
		quad2.AddMatRenderSingle( true, 2, null );
		quad2.AddMatPara( null, new Dictionary<string, object>(){
		    { "MainTex", "silk3.png" }
		} );
		*/


		var quad3 = _contexts.game.CreateEntity();
		quad3.AddComponentName( "quad3" );
		quad3.AddComponentPosition( 0, 0, 0 );
		quad3.AddComponentSize( 1f, 1f );
		quad3.AddComponentRotation( 0 );
		quad3.AddMatRenderSingle( true, 1, new RenderQuad( fragShaderName:"CellularElement.frag" ) );
		quad3.isComponentCellAutoTexture = true;

		var quad4 = _contexts.game.CreateEntity();
		quad4.AddComponentName( "quad4" );
		quad4.AddComponentPosition( 0, 0, 0 );
		quad4.AddComponentSize( 0.04f, 0.08f );
		quad4.AddComponentRotation( 0 );
		quad4.AddMatRenderSingle( true, 2, null );
		quad4.isComponentCellAutoTexture = true;
		quad4.AddLightShadowLight( true, new[]{ 1 }, 0, 10f, 1f, 0.8f, new Vector3( 246/255f, 149/255f, 96/255f ), 1f, 0f );


		/*
		var quad4 = _contexts.game.CreateEntity();
		quad4.AddComponentName( "quad4" );
		quad4.AddComponentPosition( 3.5f, -2.8f, -0.5f );
		quad4.AddComponentSize( 0.2f, 0.2f );
		quad4.AddComponentRotation( 0 );
		quad4.AddMatRenderSingle( true, 2, null );
		quad4.AddMatPara( null, new Dictionary<string, object>(){
		    { "MainTex", "silk3.png" }
		} );

		var quad5 = _contexts.game.CreateEntity();
		quad5.AddComponentName( "quad5" );
		quad5.AddComponentPosition( 1.8f, -5f, 0 );
		quad5.AddComponentSize( 0.2f, 0.2f );
		quad5.AddComponentRotation( 0 );
		quad5.AddMatRenderSingle( true, 2, null );
		quad5.AddMatPara( null, new Dictionary<string, object>(){
		    { "MainTex", "silk2.png" }
		} );

		var quad6 = _contexts.game.CreateEntity();
		quad6.AddComponentName( "quad6" );
		quad6.AddComponentPosition( 2.38f, -1.85f, -1 );
		quad6.AddComponentSize( 0.2f, 0.2f );
		quad6.AddComponentRotation( 20);
		quad6.AddMatRenderSingle( true, 2, null );
		quad6.AddMatPara( null, new Dictionary<string, object>(){
		    { "MainTex", "silk.png" }
		} );
		*/
	}
}