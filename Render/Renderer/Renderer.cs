using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

/// <summary>
/// 处理了shader的使用和销毁
/// </summary>
public class Renderer {
	protected GL _gl;
	protected Shader _baseShader;
	
	protected float[] _vertices;
	protected uint[] indices;
	protected BufferObject<float> Vbo;
	protected BufferObject<uint> Ebo;
	protected VertexArrayObject<float, uint> Vao;
	
	/// <summary>
	/// 使用默认着色器
	/// </summary>
	protected Renderer( GL Gl) {
		_gl = Gl;
	}

	public virtual void Draw() {
	}

	public virtual void Dispose() {
		_baseShader.Dispose();
		Vbo.Dispose();
		Ebo.Dispose();
		Vao.Dispose();
	}
	
	
	/// <summary>
	/// 先use，再apply
	/// </summary>
	/// <param name="name"></param>
	/// <param name="value"></param>
	void ApplyUniform( string name, object value ) {
		if( value is Vector3 ) {
			_baseShader.SetUniform( name, (Vector3)value );
		} else if( value is float ) {
			_baseShader.SetUniform( name, (float)value );
		} else if( value is Matrix4x4 ) {
			_baseShader.SetUniform( name, (Matrix4x4)value );
		}
	}

	public void SetUniform( string name, object value ) {
		_baseShader.Use();
		ApplyUniform( name, value );
	}

	public void SetTexture( int textureNum, string texName, Texture texture ) {
		_baseShader.Use();
		_baseShader.SetUniform( textureNum, texName, texture );
	}

	public void SetTexture( int textureNum, string texName, string texturePath ) {
		var tex = new Texture( _gl, texturePath );
		_baseShader.Use();
		_baseShader.SetUniform( textureNum, texName, tex );
	}
}