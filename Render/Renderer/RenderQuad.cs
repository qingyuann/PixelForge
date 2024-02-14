using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderQuad : Renderer {
	string _shaderVertPath;
	string _shaderFragPath;
	GL _gl;

	public RenderQuad(Vector3 pos, Vector2 scale,float rotation, int layer,Anchor anchor = Anchor.Center ) : base( layer ) {
		_gl = GlobalVariable.Gl;
		_shaderVertPath = AssetManager.GetAssetPath( "QuadBasic.vert" );
		_shaderFragPath = AssetManager.GetAssetPath( "QuadBasic.frag" );

		PatternMesh.CreateQuad( pos, scale, rotation, out Vertices, out Indices, anchor );
		
		Ebo = new BufferObject<uint>( base.Gl, Indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( base.Gl, Vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( _gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		BaseShader = new Shader( base.Gl, _shaderVertPath, _shaderFragPath );
	}
	
	public void UpdateQuad(Vector3 pos, Vector2 scale, float rotation, Anchor anchor = Anchor.Center) {
		PatternMesh.CreateQuad( pos, scale, rotation, out Vertices, out Indices, anchor );
		Vbo.UpdateBuffer( Vertices );
	}

	override public void Draw() {
		unsafe {
			SetUniform( "viewMatrix", CameraSystem.MainCamViewMatrix );
			Vao.Bind();
			BaseShader.Use();
			Gl.DrawElements( PrimitiveType.Triangles,
				(uint)Indices.Length, DrawElementsType.UnsignedInt, null );
		}
	}
}