using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderFullscreen : Renderer {

	public RenderFullscreen( GL Gl, int layer = -1 ) : base( Gl, layer ) {
		GenerateBgShader( GameSetting.MaxRenderLayer, out string shaderVert, out string shaderFrag );

		PatternMesh.CreateQuad( new Vector3( 0, 0, 0 ), 2, 2, out Vertices, out Indices );
		Ebo = new BufferObject<uint>( base.Gl, Indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( base.Gl, Vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( Gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		BaseShader = new Shader( base.Gl, shaderVert, shaderFrag,true );
		
		// string shaderVertTest = AssetManager.GetAssetPath( "BG.vert" );
		// string shaderFragTest = AssetManager.GetAssetPath( "BG.frag" );
		// BaseShader = new Shader( base.Gl, shaderVertTest, shaderFragTest);
	}

	override public void Draw() {
		unsafe {
			Vao.Bind();
			BaseShader.Use();
			Gl.DrawElements( PrimitiveType.Triangles,
				(uint)Indices.Length, DrawElementsType.UnsignedInt, null );
		}
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