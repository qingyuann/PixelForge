using PixelForge;
using Render.PostEffect;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Render;

public class RenderPipeline {
	readonly GL _gl;
	readonly RenderFullscreen _renderScreen;
	readonly int _layerCount = GameSetting.MaxRenderLayer;
	readonly List<RenderTexture> _layerRt = new List<RenderTexture>();


	public RenderPipeline( IWindow window ) {
		_gl = GL.GetApi( window );
		GlobalVariable.Gl = _gl;

		//创建渲染层
		for( int i = 0; i < _layerCount; i++ ) {
			_layerRt.Add( new RenderTexture( _gl, GameSetting.WindowWidth, GameSetting.WindowHeight ) );
		}
		_renderScreen = new RenderFullscreen();
	}

	public void OnRender() {
		_gl.Viewport( 0, 0, GameSetting.WindowWidth, GameSetting.WindowHeight );
		_gl.Clear( (uint)GLEnum.ColorBufferBit );
		_gl.ClearColor( 0.0f, 0.0f, 0.0f, 0.0f );

		for( var index = 0; index < _layerCount; index++ ) {
			_layerRt[index].RenderToRt();
			_gl.Clear( (uint)GLEnum.ColorBufferBit );
			_gl.ClearColor( 0.0f, 0.0f, 0.0f, 0.0f );
			RenderSystem.Render( index );
		}

		for( int i = 0; i < _layerCount; i++ ) {
			string texName = $"uTexture{i}";
			_renderScreen.SetTexture( i, texName, _layerRt[i].RT );
		}

		//渲染到屏幕
		_gl.BindFramebuffer( GLEnum.Framebuffer, 0 );
		_gl.Clear( (uint)GLEnum.ColorBufferBit );
		_renderScreen.Draw();
	}

	public void OnClose() {
		_renderScreen.Dispose();
		foreach( var rt in _layerRt ) {
			rt.Dispose();
		}
	}
}