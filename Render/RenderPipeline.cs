using PixelForge;
using System.Numerics;

namespace Render;

using Silk.NET.OpenGL;
using Silk.NET.Windowing;

public class RenderPipeline {
	static GL _gl;
	RenderFullscreen _renderScreen;
	readonly int _layerCount = 2;
	RenderQuad _quad1;
	RenderQuad _quad2;

	List<RenderTexture> _layerRt = new List<RenderTexture>();

	public RenderPipeline( IWindow window ) {
		_gl = GL.GetApi( window );
		SetupLayerRt();
		SetupObjects();
	}

	public void OnRender() {
		_gl.Viewport( 0, 0, GameSetting.WindowWidth, GameSetting.WindowHeight );
		_gl.ClearColor( 0.0f, 0.0f, 0.0f, 0.0f );
		_gl.DrawBuffer( GLEnum.ColorAttachment0 );
		//渲染到rt1
		_layerRt[0].RenderToRt();
		_gl.Clear( (uint)GLEnum.ColorBufferBit );
		RenderLayer1();

		//渲染到rt2
		_layerRt[1].RenderToRt();
		_gl.Clear( (uint)GLEnum.ColorBufferBit );
		RenderLayer2();

		//渲染到屏幕
		_gl.BindFramebuffer( GLEnum.Framebuffer, 0 );
		_gl.Clear( (uint)GLEnum.ColorBufferBit );
		_renderScreen.Draw();
	}

	void SetupLayerRt() {
		for( int i = 0; i < _layerCount; i++ ) {
			_layerRt.Add( new RenderTexture( _gl, GameSetting.WindowWidth, GameSetting.WindowHeight ) );
		}
		//长宽都是2，沾满屏幕
		_renderScreen = new RenderFullscreen( _gl );

		_renderScreen.SetTexture( 0, "uTexture0", _layerRt[0].RT );
		_renderScreen.SetTexture( 1, "uTexture1", _layerRt[1].RT );
	}

	/************************************改下面的***************************************/
	void SetupObjects() {
		var vert = AssetManager.GetAssetPath( "QuadBasic.vert" );
		var frag = AssetManager.GetAssetPath( "QuadBasic.frag" );

		_quad1 = new RenderQuad( _gl, new Vector2( 0f, 0f ), 1f, 1f );
		_quad1.SetUniform( "uColor", new Vector3( 1, 0, 0 ) );
		
		_quad2 = new RenderQuad( _gl, new Vector2( -0.01f, 0f ), 1f, 1f);
		_quad2.SetUniform( "uColor", new Vector3( 0, 0, 1 ) );
	}

	void RenderLayer1() {
		_quad1.Draw();
	}

	void RenderLayer2() {
		_quad2.Draw();
	}

	public void OnClose() {

		_layerRt[0].RT.Bind();

		_quad1.Dispose();
		_quad2.Dispose();
		_renderScreen.Dispose();
		foreach( var rt in _layerRt ) {
			rt.Dispose();
		}
	}
}