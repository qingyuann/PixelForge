using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderFullscreen : Renderer, IRenderSingleObject {
	public float[] Vertices { get; set;}
	public uint[] Indices { get; set; }
	public BufferObject<float> Vbo { get; set; }
	public BufferObject<uint> Ebo { get; set; }
	public VertexArrayObject<float, uint> Vao { get; set; }

	public RenderFullscreen( int layer = -1 ) : base( layer ) {
		GenerateBgShader( GameSetting.MaxRenderLayer, out string shaderVert, out string shaderFrag );

		PatternMesh.CreateQuad( new Vector3( 0, 0, 0 ), new Vector2( 1, 1 ), 0, out float[] vert, out uint[] indices);
		Vertices = vert;
		Indices = indices;
		Ebo = new BufferObject<uint>( base.Gl, Indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( base.Gl, Vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( Gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		BaseShader = new Shader( base.Gl, shaderVert, shaderFrag, true );

		// string shaderVertTest = AssetManager.GetAssetPath( "BG.vert" );
		// string shaderFragTest = AssetManager.GetAssetPath( "BG.frag" );
		// BaseShader = new Shader( base.Gl, shaderVertTest, shaderFragTest);
	}

	public void Update( Vector3 pos, Vector2 scale, float rotation, Anchor anchor = Anchor.Center ) { }
	
	public void Draw() {
		unsafe {
			Vao.Bind();
			BaseShader.Use();
			Gl.DrawElements( PrimitiveType.Triangles,
				(uint)Indices.Length, DrawElementsType.UnsignedInt, null );
		}
	}

	public override void Dispose() {
		base.Dispose();
		Vbo.Dispose();
		Ebo.Dispose();
		Vao.Dispose();
	}

	//generate bg shader
	void GenerateBgShader( int layerNumber, out string bgVertShader, out string bgFragShader ) {
		bgFragShader = @"
#version 330 core
in vec2 fUv;
out vec4 FragColor;
";

		if( layerNumber > 0 ) {
			bgFragShader += "\n";
			for( int i = 0; i < layerNumber; i++ ) {
				bgFragShader += $@"uniform sampler2D uTexture{i};";
				bgFragShader += "\n";
			}

			bgFragShader += @"		 
void main()
{";

			for( int i = 0; i < layerNumber; i++ ) {
				bgFragShader += $@"
vec4 color{i} = texture(uTexture{i}, fUv);";
			}

			bgFragShader += "\nFragColor = color0;\n";

			for( int i = 1; i < layerNumber; i++ ) {
				bgFragShader += $"FragColor = mix(FragColor,color{i},color{i}.w);";
			}
			bgFragShader += "}";
		} else {
			bgFragShader += @"
void main()
{
FragColor = vec4(0,0,0,0);
}";
		}

		bgVertShader = @"
#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec2 vUv;

out vec2 fUv;

void main()
{		   
    gl_Position = vec4(vPos.x, vPos.y, vPos.z, 1.0);
    fUv = vUv;
}
";
	}
}