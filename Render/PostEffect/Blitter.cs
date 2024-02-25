using PixelForge;
using Render;
using Silk.NET.OpenGL;
using Texture = Render.Texture;

namespace pp;

public static class Blitter {
	static GL _gl;
	static int[] originalState = new int[5];
	static RenderFullscreen _renderFullscreen;
	static Blitter() {
		_gl = GlobalVariable.GL;
		_renderFullscreen = new RenderFullscreen( "Blit.vert", "Blit.frag" );
	}

	public static void Blit( Texture? rt1, RenderTexture rt2, Renderer renderer ) {
		rt2.RenderToRt();
		_gl.Clear( (uint)GLEnum.ColorBufferBit | (uint)GLEnum.DepthBufferBit );
		if( rt1 is not null ) {
			renderer.SetTexture( "_BlitTexture", rt1 );
		}
		renderer.Draw();
	}

	public static void Blit( Texture? rt1, RenderTexture rt2 ) {
		Blit( rt1, rt2, _renderFullscreen );
	}


	public static void Blit( RenderTexture rt1, RenderTexture rt2 ) {
		GlobalVariable.GL.BindFramebuffer( FramebufferTarget.ReadFramebuffer, rt1._fbo );
		GlobalVariable.GL.BindFramebuffer( FramebufferTarget.DrawFramebuffer, rt2._fbo );
		GlobalVariable.GL.BlitFramebuffer(
			0, 0, rt1.Width, rt1.Height, // 源区域左下角坐标和右上角坐标
			0, 0, rt2.Width, rt2.Height, // 目标区域左下角坐标和右上角坐标
			ClearBufferMask.ColorBufferBit, // 指定要复制的缓冲区
			BlitFramebufferFilter.Linear // 指定采样滤波方式
		);
		// 解绑帧缓冲区
		GlobalVariable.GL.BindFramebuffer( FramebufferTarget.ReadFramebuffer, 0 );
		GlobalVariable.GL.BindFramebuffer( FramebufferTarget.DrawFramebuffer, 0 );
	}

	static void RecordState() {
		_gl.GetInteger( GLEnum.ArrayBufferBinding, out originalState[0] );
		_gl.GetInteger( GLEnum.ElementArrayBufferBinding, out originalState[1] );
		_gl.GetInteger( GLEnum.TextureBinding2D, out originalState[2] );
		_gl.GetInteger( GLEnum.CurrentProgram, out originalState[3] );
		_gl.GetInteger( GLEnum.FramebufferBinding, out originalState[4] );
	}

	static void RecoverState() {
		_gl.BindBuffer( GLEnum.ArrayBuffer, (uint)originalState[0] );
		_gl.BindBuffer( GLEnum.ElementArrayBuffer, (uint)originalState[1] );
		_gl.BindTexture( GLEnum.Texture2D, (uint)originalState[2] );
		_gl.UseProgram( (uint)originalState[3] );
		_gl.BindFramebuffer( GLEnum.Framebuffer, (uint)originalState[4] );
	}
}