using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderQuad : Renderer, IRenderSingleObject {

	public float[] Vertices { get; set; }
	public uint[] Indices { get; set; }
	public BufferObject<float> Vbo { get; set; }
	public BufferObject<uint> Ebo { get; set; }
	public VertexArrayObject<float, uint> Vao { get; set; }

	public RenderQuad( Vector3 pos, Vector2 scale, float rotation, int layer, Anchor anchor = Anchor.Center, string vertShaderName = "QuadBasic.vert", string fragShaderName = "QuadBasic.frag", bool relativeLength = false, bool invertV = false ) : base( layer, vertShaderName, fragShaderName ) {
		GL gl = GlobalVariable.GL;
		PatternMesh.CreateQuad( pos, scale, rotation, out float[] vert, out uint[] indices, anchor, relativeLength, invertV );
		Vertices = vert;
		Indices = indices;
		Ebo = new BufferObject<uint>( Gl, Indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( Gl, Vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( gl, Vbo, Ebo );
		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
	}

	public void UpdateTransform( Vector3 pos, Vector2 scale, float rotation, Anchor anchor = Anchor.Center ) {
		PatternMesh.CreateQuad( pos, scale, rotation, out float[] vert, out _, anchor );
		Vertices = vert;
		Vbo.UpdateBuffer( Vertices );
	}

	public override void Draw() {
		unsafe {
			BaseShader.Use();
			Vao.Bind();

			int textureNum = 0;
			foreach( var tex in Textures ) {
				BaseShader.SetUniform( textureNum, tex.Key, tex.Value );
				textureNum++;
			}
			SetUniform( "viewMatrix", CameraSystem.MainCamViewMatrix );

			Gl.DrawElements( PrimitiveType.Triangles,
				(uint)Indices.Length, DrawElementsType.UnsignedInt, null );

			//unbind
			for( int i = 0; i < textureNum; i++ ) {
				Gl.ActiveTexture( TextureUnit.Texture0 + i );
				Gl.BindTexture( TextureTarget.Texture2D, 0 );
			}
		}
	}

	public override void Dispose() {
		base.Dispose();
		Vbo.Dispose();
		Ebo.Dispose();
		Vao.Dispose();
	}
}