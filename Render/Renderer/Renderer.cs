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
    public Dictionary<string, Texture> Textures=new Dictionary<string, Texture>();

    public int Layer { get; private set; }

    /// <summary>
    /// 使用默认着色器
    /// </summary>
    protected Renderer( int layer, string shaderVertName, string shaderFragName, bool isShaderContent = false)
    {
        Gl = GlobalVariable.GL;
        Layer = layer;

        if( isShaderContent==false ) {
            shaderVertName = AssetManager.GetAssetPath( shaderVertName );
            shaderFragName = AssetManager.GetAssetPath( shaderFragName);
        }
        BaseShader = new Shader( Gl, shaderVertName, shaderFragName ,isShaderContent);
    }

    public virtual void Dispose()
    {
        BaseShader.Dispose();
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
        } else {
            Debug.Log("Renderer.ApplyUniform: 未知的类型");
        }
    }

    public void SetUniform(string name, object value)
    {
        BaseShader.Use();
        ApplyUniform(name, value);
    }

    public void SetTexture( string texName, Texture texture)
    {
        if( Textures.ContainsKey(texName) )
        {
            Textures[texName].Dispose();
            Textures.Remove(texName);
        }
        Textures.Add(texName, texture);
    }
    
    public void SetTexture(string texName, string texturePath)
    {
        if (Textures.ContainsKey(texName))
        {
            Textures[texName].Dispose();
            Textures.Remove(texName);
        }
        var path = AssetManager.GetAssetPath(texturePath);
        var tex = new Texture(Gl, path);
        Textures.Add(texName, tex);
    }

}