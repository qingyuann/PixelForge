using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render; 

public class RenderFullscreen:Renderer  {
	string _shaderVertPath;
	string _shaderFragPath;

	public RenderFullscreen( GL Gl, int layer=-1 ) : base( Gl,layer ) {
		_shaderVertPath = AssetManager.GetAssetPath( "BG.vert" );
		_shaderFragPath = AssetManager.GetAssetPath( "BG.frag" );

		PatternMesh.CreateQuad( new Vector3(0, 0,0 ), 2, 2, out Vertices, out Indices );
		Ebo = new BufferObject<uint>( base.Gl, Indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( base.Gl, Vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( Gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		BaseShader = new Shader( base.Gl, _shaderVertPath, _shaderFragPath );
	}

	public RenderFullscreen( GL Gl, string vertPath, string fragPath ,int layer=-1) : base( Gl,layer ) {
		_shaderVertPath = AssetManager.GetAssetPath( vertPath );
		_shaderFragPath = AssetManager.GetAssetPath( fragPath );

		PatternMesh.CreateQuad( new Vector3( 0,0,0 ), 2, 2, out Vertices, out Indices );
		Ebo = new BufferObject<uint>( base.Gl, Indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( base.Gl, Vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( Gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		BaseShader = new Shader( base.Gl, _shaderVertPath, _shaderFragPath );
	}
	
	override public void Draw() {
		unsafe {
			Vao.Bind();
			BaseShader.Use();
			Gl.DrawElements( PrimitiveType.Triangles,
				(uint)Indices.Length, DrawElementsType.UnsignedInt, null );
		}
	}
}