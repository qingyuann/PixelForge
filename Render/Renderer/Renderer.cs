using PixelForge;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Render;

/// <summary>
/// 处理了shader的使用和销毁
/// </summary>
public class Renderer
{
    protected readonly GL Gl;
    protected Shader BaseShader;
    public Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

    public int Layer { get; private set; }

    /// <summary>
    /// 使用默认着色器
    /// </summary>
    protected Renderer(int layer, string shaderVertName, string shaderFragName, bool isShaderContent = false)
    {
        Gl = GlobalVariable.GL;
        Layer = layer;

        if (isShaderContent == false)
        {
            shaderVertName = AssetManager.GetAssetPath(shaderVertName);
            shaderFragName = AssetManager.GetAssetPath(shaderFragName);
        }

        BaseShader = new Shader(Gl, shaderVertName, shaderFragName, isShaderContent);
    }

    public virtual void Dispose()
    {
        BaseShader.Dispose();
    }

    public virtual void Draw()
    {
    }

    /// <summary>
    /// 先use，再apply
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    void ApplyUniform(string name, object value)
    {
        if (value is Vector3 v3)
        {
            BaseShader.SetUniform(name, v3);
        }
        else if (value is Matrix4x4 x4)
        {
            BaseShader.SetUniform(name, x4);
        }
        else if (value is float[] floats)
        {
            BaseShader.SetUniform(name, floats);
        }
        else if (value is int or uint or float or double)
        {
            float v = Convert.ToSingle(value);
            BaseShader.SetUniform(name, v);
        }
        else if (value is Vector2 v2)
        {
            BaseShader.SetUniform(name, v2);
        }
        else if (value is Vector4 v4)
        {
            BaseShader.SetUniform(name, v4);
        }
        else
        {
            Debug.LogError("Renderer.ApplyUniform: 未知的类型");
        }
    }

    public void SetUniform(string name, object value)
    {
        BaseShader.Use();
        ApplyUniform(name, value);
    }

    public void SetTexture(string texName, Texture texture)
    {
        if (Textures.ContainsKey(texName))
        {
            TexturePool.ReturnTex(Textures[texName]);
            Textures[texName] = texture;
        }
        else
        {
            Textures.Add(texName, texture);
        }
    }

    public void SetTexture(string texName, string textureName)
    {
        var path = AssetManager.GetAssetPath(textureName);
        var tex = new Texture(Gl, path);
        if (Textures.TryAdd(texName, tex))
            return;
        TexturePool.ReturnTex(Textures[texName]);
        Textures[texName] = tex;
    }

}