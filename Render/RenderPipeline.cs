using PixelForge;
using PixelForge.Light;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderPipeline {
	readonly GL _gl;
	readonly RenderFullscreen _renderScreen;
	readonly RenderFullscreen _mergeLight;
	readonly int _layerCount = GameSetting.MaxRenderLayer;
	readonly List<RenderTexture> _layerRt = new List<RenderTexture>();

	RenderQuad _quad;

	public RenderPipeline( GL gl ) {
		_mergeLight = new RenderFullscreen( "Blit.vert", "QuadBasic.frag" );

		_gl = GlobalVariable.GL;
		_gl.Enable( GLEnum.DepthTest );
		// gl.Enable(EnableCap.Blend);
		_gl.DepthFunc( DepthFunction.Less );

		//create render layer
		for( int i = 0; i < _layerCount; i++ ) {
			_layerRt.Add( TexturePool.GetRT( GameSetting.WindowWidth, GameSetting.WindowHeight ) );
		}
		// _renderScreen = new RenderFullscreen();
		_gl.Viewport( 0, 0, GameSetting.WindowWidth, GameSetting.WindowHeight );

		//set renderScreen shader
		// for( int i = 0; i < _layerCount; i++ ) {
		// 	string texName = $"uTexture{i}";
		// 	_renderScreen.SetTexture( texName, _layerRt[i] );
		// }

		_gl.BindFramebuffer( GLEnum.Framebuffer, 0 );

		_quad = new RenderQuad( Vector3.Zero + new Vector3( 0f, 0f, 0.1f ), Vector2.One, 0, 0 );
	}

	public void OnRender() {
		_gl.Enable( EnableCap.DepthTest );
		_gl.ClearColor( 0.0f, 0.0f, 0.0f, 0.0f );

		//render to layer
		for( var index = 0; index < _layerCount; index++ ) {
			_layerRt[index].RenderToRt();
			_gl.Clear( (uint)GLEnum.ColorBufferBit | (uint)ClearBufferMask.DepthBufferBit );
			RenderSystem.Render( index );
		}

		//2d light
		for( int i = 0; i < _layerCount; i++ ) {
			LightSystem.RenderLights( i, _layerRt[i] );
		}

		//post process
		for( var index = 0; index < _layerCount; index++ ) {
			PostProcessSystem.RenderPostProcess( index, _layerRt[index] );
		}

		//blit to screen
		_gl.BindFramebuffer( GLEnum.Framebuffer, 0 );
		_gl.Clear( (uint)GLEnum.ColorBufferBit | (uint)ClearBufferMask.DepthBufferBit );
		
		GlobalVariable.GL.DepthMask(false); 
		GlobalVariable.GL.Enable( EnableCap.Blend );
		GlobalVariable.GL.BlendFunc( BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha );
		foreach( RenderTexture rt in _layerRt ) {
			_mergeLight.SetTexture( "MainTex", rt );
			_mergeLight.Draw();
		}
		GlobalVariable.GL.Disable( EnableCap.Blend );
		GlobalVariable.GL.DepthMask( true );
	}

	public void OnClose() {
		_renderScreen.Dispose();
		foreach( var rt in _layerRt ) {
			TexturePool.ReturnRT( rt );
		}
	}
}