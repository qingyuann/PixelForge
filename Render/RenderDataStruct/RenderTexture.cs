using Silk.NET.OpenGL;

namespace Render;

public class RenderTexture:Texture {
	readonly uint _fbo;
	readonly int _colorAttachment;

	public RenderTexture( GL gl, uint width, uint height, int colorAttachment = 0 ) : base( gl, new byte[width * height * 4], width, height) {
		_colorAttachment = colorAttachment;
		_fbo=_gl.GenFramebuffer( );
		_gl.BindFramebuffer( GLEnum.Framebuffer, _fbo );
		_gl.FramebufferTexture2D( GLEnum.Framebuffer, GLEnum.ColorAttachment0 + _colorAttachment, GLEnum.Texture2D, _handle, 0 );

		// 检查帧缓冲完整性
		if( _gl.CheckFramebufferStatus( GLEnum.Framebuffer ) != GLEnum.FramebufferComplete ) {
			throw new Exception( "Framebuffer is not complete" );
		}
	}
	
	public void RenderToRt() {
		_gl.BindFramebuffer( GLEnum.Framebuffer, _fbo );
		_gl.DrawBuffer(GLEnum.ColorAttachment0+_colorAttachment);
	}

	public override void Dispose() {
		 base.Dispose();
		_gl.DeleteFramebuffer( _fbo );
	}

}