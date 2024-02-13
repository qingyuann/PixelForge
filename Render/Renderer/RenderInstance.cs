using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

public class RenderQuadInstances : Renderer
{
    string _shaderVertPath;
    string _shaderFragPath;

    float[] _positions;

    //shader中的数组大小
    readonly int _arraySize = GameSetting.MaxInstancePerDrawCall * 2;
    int _instanceCount;

    public RenderQuadInstances(GL Gl, List<float> pos, float width, float height, int layer,
        Anchor anchor = Anchor.Center) : base(Gl, layer)
    {
        _shaderVertPath = AssetManager.GetAssetPath("QuadInstances.vert");
        _shaderFragPath = AssetManager.GetAssetPath("QuadInstances.frag");

        //初始化数组，大小和shader中的一样
        _positions = new float[_arraySize];
        //实例的数量
        _instanceCount = pos.Count / 2;
        pos.CopyTo(0, _positions, 0, Math.Min(_arraySize, pos.Count));

        //basic quad
        PatternMesh.CreateQuad(new Vector3(0, 0, 0), width, height, out Vertices, out Indices, anchor);
        Ebo = new BufferObject<uint>(base.Gl, Indices, BufferTargetARB.ElementArrayBuffer);
        Console.WriteLine(Indices.Length);
        Vbo = new BufferObject<float>(base.Gl, Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<float, uint>(Gl, Vbo, Ebo);

        //set pos
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
        //set uv
        Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);
        BaseShader = new Shader(base.Gl, _shaderVertPath, _shaderFragPath);
    }

    public RenderQuadInstances(GL Gl, List<float> pos, float width, float height, int layer, string vertPath,
        string fragPath, Anchor anchor = Anchor.Center) : base(Gl, layer)
    {
        _shaderVertPath = AssetManager.GetAssetPath(vertPath);
        _shaderFragPath = AssetManager.GetAssetPath(fragPath);
        _positions = new float[_arraySize];

        //初始化数组，大小和shader中的一样
        _positions = new float[_arraySize];
        //实例的数量
        _instanceCount = pos.Count / 2;
        pos.CopyTo(0, _positions, 0, Math.Min(_arraySize, pos.Count));

        //basic quad
        PatternMesh.CreateQuad(new Vector3(0, 0, 0), width, height, out Vertices, out Indices, anchor);

        Ebo = new BufferObject<uint>(base.Gl, Indices, BufferTargetARB.ElementArrayBuffer);
        Vbo = new BufferObject<float>(base.Gl, Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<float, uint>(Gl, Vbo, Ebo);

        //set pos
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
        //set uv
        Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);
        BaseShader = new Shader(base.Gl, _shaderVertPath, _shaderFragPath);
    }

    public override void Draw()
    {
        BaseShader.Use();

        SetUniform("viewMatrix", VirtualCamera.GetViewMatrix());
        SetUniform("posOffset", _positions);

        Vao.Bind();

        Gl.DrawArraysInstanced(PrimitiveType.Triangles, 0, 6, (uint)_instanceCount);
    }
}