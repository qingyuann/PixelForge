using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render; 

public class RenderFullscreen:Renderer  {
	string _shaderVertPath;
	string _shaderFragPath;

	public RenderFullscreen( GL Gl ) : base( Gl ) {
		_shaderVertPath = AssetManager.GetAssetPath( "BG.vert" );
		_shaderFragPath = AssetManager.GetAssetPath( "BG.frag" );

		PatternMesh.CreateQuad( new Vector2( 0,0 ), 2, 2, out _vertices, out indices );
		Ebo = new BufferObject<uint>( _gl, indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( _gl, _vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( Gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		_baseShader = new Shader( _gl, _shaderVertPath, _shaderFragPath );
	}

	public RenderFullscreen( GL Gl, string vertPath, string fragPath ) : base( Gl ) {
		_shaderVertPath = AssetManager.GetAssetPath( vertPath );
		_shaderFragPath = AssetManager.GetAssetPath( fragPath );

		PatternMesh.CreateQuad( new Vector2( 0,0 ), 2, 2, out _vertices, out indices );
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
			Vao.Bind();
			_baseShader.Use();
			_gl.DrawElements( PrimitiveType.Triangles,
				(uint)indices.Length, DrawElementsType.UnsignedInt, null );
		}
	}
}