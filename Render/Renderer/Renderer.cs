using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

/// <summary>
/// 处理了shader的使用和销毁
/// </summary>
public class Renderer
{
    protected GL Gl;
    protected Shader BaseShader;

    public int Layer { get; private set; }

    protected float[] Vertices;
    protected uint[] Indices;
    protected BufferObject<float> Vbo;
    protected BufferObject<uint> Ebo;
    protected VertexArrayObject<float, uint> Vao;

    /// <summary>
    /// 使用默认着色器
    /// </summary>
    protected Renderer( int layer)
    {
        this.Gl = GlobalVariable.Gl;
        Layer = layer;
    }

    public virtual void Draw()
    {
    }

    public virtual void Dispose()
    {
        BaseShader.Dispose();
        Vbo.Dispose();
        Ebo.Dispose();
        Vao.Dispose();
    }

    /// <summary>
    /// 先use，再apply
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    void ApplyUniform(string name, object value)
    {
        if (value is Vector3)
        {
            BaseShader.SetUniform(name, (Vector3)value);
        }
        else if (value is float)
        {
            BaseShader.SetUniform(name, (float)value);
        }
        else if (value is Matrix4x4)
        {
            BaseShader.SetUniform(name, (Matrix4x4)value);
        }
        else if (value is float[])
        {
            BaseShader.SetUniform(name, (float[])value);
        }
    }

    public void SetUniform(string name, object value)
    {
        BaseShader.Use();
        ApplyUniform(name, value);
    }

    public void SetTexture(int textureNum, string texName, Texture texture)
    {
        BaseShader.Use();
        BaseShader.SetUniform(textureNum, texName, texture);
    }

    public void SetTexture(int textureNum, string texName, string texturePath)
    {
        var tex = new Texture(Gl, texturePath);
        BaseShader.Use();
        BaseShader.SetUniform(textureNum, texName, tex);
    }
}