using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderQuad : Renderer {
	string _shaderVertPath;
	string _shaderFragPath;

	public RenderQuad( GL Gl, Vector3 pos, float width, float height, int layer,Anchor anchor = Anchor.Center ) : base( Gl,layer ) {
		_shaderVertPath = AssetManager.GetAssetPath( "QuadBasic.vert" );
		_shaderFragPath = AssetManager.GetAssetPath( "QuadBasic.frag" );

		PatternMesh.CreateQuad( pos, width, height, out Vertices, out Indices, anchor );
		Ebo = new BufferObject<uint>( base.Gl, Indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( base.Gl, Vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( Gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		BaseShader = new Shader( base.Gl, _shaderVertPath, _shaderFragPath );
	}
	
	public RenderQuad( GL Gl,  Vector3 pos, float width, float height,int layer, string vertPath, string fragPath, Anchor anchor = Anchor.Center  ) : base( Gl,layer ) {
		_shaderVertPath = vertPath;
		_shaderFragPath = fragPath;

		PatternMesh.CreateQuad( pos, width, height, out Vertices, out Indices, anchor );
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
			SetUniform( "viewMatrix", VirtualCamera.GetViewMatrix() );
			Vao.Bind();
			BaseShader.Use();
			Gl.DrawElements( PrimitiveType.Triangles,
				(uint)Indices.Length, DrawElementsType.UnsignedInt, null );
		}
	}
}