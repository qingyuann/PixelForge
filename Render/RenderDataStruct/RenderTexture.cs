using Silk.NET.OpenGL;

namespace Render;

public class RenderTexture : Texture {
	public readonly uint _fbo;
	readonly int _colorAttachment;
	public readonly int Width;
	public readonly int Height;
	public RenderTexture( GL gl, uint width, uint height, int colorAttachment = 0 ) : base( gl, new byte[width * height * 4], width, height ) {
		Width = (int)width;
		Height = (int)height;
		_colorAttachment = colorAttachment;
		_fbo = _gl.GenFramebuffer();
		_gl.BindFramebuffer( GLEnum.Framebuffer, _fbo );
		
		_gl.FramebufferTexture2D( GLEnum.Framebuffer, GLEnum.ColorAttachment0 + _colorAttachment, GLEnum.Texture2D, _handle, 0 );
		
		//generate depth buffer
		var depth=_gl.GenRenderbuffers(1);
		_gl.BindRenderbuffer(GLEnum.Renderbuffer, depth);
		_gl.RenderbufferStorage(GLEnum.Renderbuffer, GLEnum.DepthComponent, width, height); 
		_gl.FramebufferRenderbuffer(GLEnum.Framebuffer, GLEnum.DepthAttachment, GLEnum.Renderbuffer, depth);
		
		// 检查帧缓冲完整性
		if( _gl.CheckFramebufferStatus( GLEnum.Framebuffer ) != GLEnum.FramebufferComplete ) {
			throw new Exception( "Framebuffer is not complete" );
		}
		
		_gl.BindFramebuffer(GLEnum.Framebuffer, 0);
		_gl.BindTexture(GLEnum.Texture2D, 0);
		_gl.BindRenderbuffer(GLEnum.Renderbuffer, 0);
	}

	public void RenderToRt() {
		_gl.BindFramebuffer( GLEnum.Framebuffer, _fbo );
		_gl.DrawBuffer( GLEnum.ColorAttachment0 + _colorAttachment );
	}

	public override void Dispose() {
		base.Dispose();
		_gl.DeleteFramebuffer( _fbo );
	}

}