using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderQuad : Renderer {
	string _shaderVertPath;
	string _shaderFragPath;

	public RenderQuad( GL Gl, Vector2 pos, float width, float height, Anchor anchor = Anchor.Center ) : base( Gl ) {
		_shaderVertPath = AssetManager.GetAssetPath( "QuadBasic.vert" );
		_shaderFragPath = AssetManager.GetAssetPath( "QuadBasic.frag" );

		PatternMesh.CreateQuad( pos, width, height, out _vertices, out indices, anchor );
		Ebo = new BufferObject<uint>( _gl, indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( _gl, _vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( Gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		_baseShader = new Shader( _gl, _shaderVertPath, _shaderFragPath );
	}
	
	public RenderQuad( GL Gl,  Vector2 pos, float width, float height, string vertPath, string fragPath, Anchor anchor = Anchor.Center  ) : base( Gl ) {
		_shaderVertPath = vertPath;
		_shaderFragPath = fragPath;

		PatternMesh.CreateQuad( pos, width, height, out _vertices, out indices, anchor );
		Ebo = new BufferObject<uint>( _gl, indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( _gl, _vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( Gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		_baseShader = new Shader( _gl, _shaderVertPath, _shaderFragPath );
	}

	override public void Draw() {
		unsafe {
			SetUniform( "viewMatrix", VirtualCamera.GetViewMatrix() );
			Vao.Bind();
			_baseShader.Use();
			_gl.DrawElements( PrimitiveType.Triangles,
				(uint)indices.Length, DrawElementsType.UnsignedInt, null );
		}
	}
}