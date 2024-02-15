using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderQuadInstances : Renderer, IRenderInstanceObject {
	public float[] Vertices { get; set; }
	public uint[] Indices { get; set; }
	public BufferObject<float> Vbo { get; set; }
	public BufferObject<uint> Ebo { get; set; }
	public VertexArrayObject<float, uint> Vao { get; set; }


	string _shaderVertPath;
	string _shaderFragPath;

	Texture _texture;

	float[] _positions;
	float[] _sizes;
	float[] _rotations;

	//shader中的数组大小
	readonly int _maxInstance = GameSetting.MaxInstancePerDrawCall;
	int _instanceCount;

	public RenderQuadInstances( List<Vector3> pos, List<Vector2> scale, List<float> rotations, int layer,
		Anchor anchor = Anchor.Center ) : base( layer ) {
		_shaderVertPath = AssetManager.GetAssetPath( "QuadInstances.vert" );
		_shaderFragPath = AssetManager.GetAssetPath( "QuadInstances.frag" );
 		SetUp( pos, scale, rotations, anchor );
	}
	
	public RenderQuadInstances( List<Vector3> pos, List<Vector2> scale, List<float> rotations, int layer,string vertFileName, string fragFilePath, Anchor anchor = Anchor.Center ) : base( layer ) {
		_shaderVertPath = AssetManager.GetAssetPath( vertFileName );
		_shaderFragPath = AssetManager.GetAssetPath( fragFilePath );
		SetUp( pos, scale, rotations, anchor );

	}
	
	void SetUp(List<Vector3> pos, List<Vector2> scale, List<float> rotations, Anchor anchor = Anchor.Center) {
		//初始化数组，大小和shader中的一样
		_positions = new float[_maxInstance * 3];
		_sizes = new float[_maxInstance * 2];
		_rotations = new float[_maxInstance];

		//basic quad
		PatternMesh.CreateQuad( new Vector3( 0, 0, 0 ), new Vector2( 1, 1 ), 0f, out float[] vert, out uint[] indices, anchor );
		Vertices = vert;
		Indices = indices;
		Ebo = new BufferObject<uint>( base.Gl, Indices, BufferTargetARB.ElementArrayBuffer );
		Vbo = new BufferObject<float>( base.Gl, Vertices, BufferTargetARB.ArrayBuffer );
		Vao = new VertexArrayObject<float, uint>( GlobalVariable.Gl, Vbo, Ebo );

		//set pos
		Vao.VertexAttributePointer( 0, 3, VertexAttribPointerType.Float, 5, 0 );
		//set uv
		Vao.VertexAttributePointer( 1, 2, VertexAttribPointerType.Float, 5, 3 );
		BaseShader = new Shader( base.Gl, _shaderVertPath, _shaderFragPath );
		_texture = new Texture( GlobalVariable.Gl, AssetManager.GetAssetPath( "silk.png" ) );
		UpdateInstance( pos, scale, rotations );
	}
	
	public void UpdateInstance( List<Vector3> pos, List<Vector2> scale, List<float> rotations , Anchor anchor = Anchor.Center ) {
		if( pos.Count != scale.Count || pos.Count != rotations.Count ) {
			throw new System.Exception( "pos, scale, rotations count not match" );
		}
		_instanceCount = pos.Count;

		//transform 调整
		var posFloats = pos.SelectMany( v => new float[]{
			v.X,
			v.Y,
			v.Z
		} ).ToList();
		posFloats.CopyTo( 0, _positions, 0, Math.Min( _maxInstance * 3, pos.Count * 3 ) );
		var scaleFloats = scale.SelectMany( v => new float[]{
			v.X,
			v.Y
		} ).ToList();
		scaleFloats.CopyTo( 0, _sizes, 0, Math.Min( _maxInstance * 2, scale.Count * 2 ) );
		rotations.CopyTo( 0, _rotations, 0, Math.Min( _maxInstance, rotations.Count ) );

		SetUniform( "posOffset", _positions );
		SetUniform( "scaleOffset", _sizes );
		SetUniform( "rotationOffset", _rotations );
	}

	public void UpdateTransform( List<Vector3> pos ) {
		if( pos.Count != _instanceCount ) {
			throw new System.Exception( "pos, scale, rotations count not match" );
		}
		var posFloats = pos.SelectMany( v => new float[]{
			v.X,
			v.Y,
			v.Z
		} ).ToList();
		posFloats.CopyTo( 0, _positions, 0, Math.Min( _maxInstance * 3, pos.Count ) );
		SetUniform( "posOffset", _positions );
	}

	public void UpdateTransform( List<Vector2> scale ) {
		if( scale.Count != _instanceCount ) {
			throw new System.Exception( "pos, scale, rotations count not match" );
		}
		var scaleFloats = scale.SelectMany( v => new float[]{
			v.X,
			v.Y
		} ).ToList();
		scaleFloats.CopyTo( 0, _sizes, 0, Math.Min( _maxInstance * 2, scale.Count ) );
		SetUniform( "scaleOffset", _sizes );
	}

	public void UpdateTransform( List<float> rotations ) {
		if( rotations.Count != _instanceCount ) {
			throw new System.Exception( "pos, scale, rotations count not match" );
		}
		rotations.CopyTo( 0, _rotations, 0, Math.Min( _maxInstance, rotations.Count ) );
		SetUniform( "rotationOffset", _rotations );
	}

	public void Draw() {

		BaseShader.Use();

		SetTexture( 0, "_QuadInstanceMainTex", _texture );
		SetUniform( "viewMatrix", CameraSystem.MainCamViewMatrix );

		Vao.Bind();

		Gl.DrawArraysInstanced( PrimitiveType.Triangles, 0, 6, (uint)_instanceCount );
	}

	public override void Dispose() {
		base.Dispose();
		Vao.Dispose();
		Vbo.Dispose();
		Ebo.Dispose();
		_texture.Dispose();
	}
}