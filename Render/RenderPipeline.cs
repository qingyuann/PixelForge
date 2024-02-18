using PixelForge;
using Render.PostEffect;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

namespace Render;

public class RenderPipeline {
	readonly GL _gl;
	readonly RenderFullscreen _renderScreen;
	readonly int _layerCount = GameSetting.MaxRenderLayer;
	readonly List<RenderTexture> _layerRt = new List<RenderTexture>();
	public readonly IWindow _window;


	RenderQuad testQuad;

	Texture tex;
	public RenderPipeline( IWindow window ) {
		_window = window;
		_gl = GL.GetApi( window );
		GlobalVariable.GL = _gl;

		//create render layer
		for( int i = 0; i < _layerCount; i++ ) {
			_layerRt.Add( new RenderTexture( _gl, GameSetting.WindowWidth, GameSetting.WindowHeight ) );
		}
		_renderScreen = new RenderFullscreen();
		_gl.Viewport( 0, 0, GameSetting.WindowWidth, GameSetting.WindowHeight );
		
		
		//set renderScreen shader
		for( int i = 0; i < _layerCount; i++ ) {
			string texName = $"uTexture{i}";
			_renderScreen.SetTexture( texName, _layerRt[i] );
		}
		
		//test
		// testQuad = new RenderQuad( Vector3.Zero, Vector2.One * 0.2f, 0, 0 );
		// testQuad.SetTexture( "MainTex","silk2.png" );
		// var path = AssetManager.GetAssetPath( "silk2.png" );
		// tex = new Texture( _gl, path );
		// testQuad.SetTexture( "MainTex", tex );
		
		
		_gl.BindFramebuffer( GLEnum.Framebuffer, 0 );
	}

	public void OnRender() {
		_gl.ClearColor( 0.0f, 0.0f, 0.0f, 0.0f );
		//render to layer
		for( var index = 0; index < _layerCount; index++ ) {
			_layerRt[index].RenderToRt();
			_gl.Clear( (uint)GLEnum.ColorBufferBit | (uint)GLEnum.DepthBufferBit );
			RenderSystem.Render( index );
		}
		
		//post process
		for( var index = 0; index < _layerCount; index++ ) {
			_layerRt[index].RenderToRt();
			PostProcessSystem.RenderPostProcess( index, _layerRt[index] );
		}


		
		//blit to screen
		_gl.BindFramebuffer( GLEnum.Framebuffer, 0 );
		_gl.Clear( (uint)GLEnum.ColorBufferBit | (uint)GLEnum.DepthBufferBit );
		_renderScreen.Draw();
	}

	public void OnClose() {
		_renderScreen.Dispose();
		foreach( var rt in _layerRt ) {
			rt.Dispose();
		}
	}
}