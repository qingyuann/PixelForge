using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderQuad : Renderer, IRenderSingleObject {
	string _shaderVertPath;
	string _shaderFragPath;
	GL _gl;
	
	public float[] Vertices { get; set;}
	public uint[] Indices { get; set; }
	public BufferObject<float> Vbo { get; set; }
	public BufferObject<uint> Ebo { get; set; }
	public VertexArrayObject<float, uint> Vao { get; set; }

	public RenderQuad(Vector3 pos, Vector2 scale,float rotation, int layer,Anchor anchor = Anchor.Center ) : base( layer ) {
		_gl = GlobalVariable.Gl;
		_shaderVertPath = AssetManager.GetAssetPath( "QuadBasic.vert" );
		_shaderFragPath = AssetManager.GetAssetPath( "QuadBasic.frag" );
		SetUp( pos, scale, rotation, anchor );
	}
	
	public RenderQuad(Vector3 pos, Vector2 scale,float rotation, int layer,string vertFileName, string fragFileName, Anchor anchor = Anchor.Center ) : base( layer ) {
		_gl = GlobalVariable.Gl;
		_shaderVertPath = AssetManager.GetAssetPath( vertFileName);
		_shaderFragPath = AssetManager.GetAssetPath( fragFileName);
		SetUp( pos, scale, rotation, anchor );
	}
	
	void SetUp( Vector3 pos, Vector2 scale, float rotation, Anchor anchor ) {

		PatternMesh.CreateQuad( pos, scale, rotation, out float[] vert, out uint[] indices, anchor );
		Vertices = vert;
		Indices = indices;

		Ebo = new BufferObject<uint>( base.Gl, Indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( base.Gl, Vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( _gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		BaseShader = new Shader( base.Gl, _shaderVertPath, _shaderFragPath );
	}

	public void Update(Vector3 pos, Vector2 scale, float rotation, Anchor anchor = Anchor.Center) {
		PatternMesh.CreateQuad( pos, scale, rotation, out float[] vert, out uint[] indices,anchor);
		Vertices = vert;
		Indices = indices;
		Vbo.UpdateBuffer( Vertices );
	}

	public void Draw() {
		unsafe {
			SetUniform( "viewMatrix", CameraSystem.MainCamViewMatrix );
			Vao.Bind();
			BaseShader.Use();
			Gl.DrawElements( PrimitiveType.Triangles,
				(uint)Indices.Length, DrawElementsType.UnsignedInt, null );
		}
	}
}