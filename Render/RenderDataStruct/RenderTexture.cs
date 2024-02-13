using Silk.NET.OpenGL;

namespace Render;

public class RenderTexture {
	uint _fbo;

	public Texture RT;
	GL _gl;

	public RenderTexture( GL gl, uint width, uint height ,int colorAttachment = 0) {
		unsafe {
			_gl = gl;
			_gl.GenFramebuffers( 1, out _fbo );
			_gl.BindFramebuffer( GLEnum.Framebuffer, _fbo );
			
			// 创建纹理对象:四个通道，所以四倍
			Span<byte> data = new Span<byte>(new byte[width * height * 4]);
			RT = new Texture( gl, data, width, height  );
			RT.Bind();
			
			_gl.FramebufferTexture2D( GLEnum.Framebuffer, GLEnum.ColorAttachment0 + colorAttachment, GLEnum.Texture2D, RT.Handle, 0 );

			// 检查帧缓冲完整性
			if(_gl.CheckFramebufferStatus( GLEnum.Framebuffer ) != GLEnum.FramebufferComplete ) {
				throw new Exception( "Framebuffer is not complete" );
			} 
		}
	}

	public void RenderToRt() {
		_gl.BindFramebuffer( GLEnum.Framebuffer, _fbo );
	}

	public void SetShaderUniform( Shader shader, string uniformName ) {
		_gl.BindTexture( GLEnum.Texture2D, RT.Handle );
		shader.SetUniform( "uniformName", 0 );
	}


	public void Dispose() {
		_gl.DeleteFramebuffer( _fbo );
		_gl.DeleteTexture( RT.Handle );
	}

}