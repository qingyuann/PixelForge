using PixelForge;
using Silk.NET.OpenGL;

namespace Render;

public static class TexturePool
{
    static Dictionary<(uint, uint), Stack<RenderTexture>> _rt_pool = new Dictionary<(uint, uint), Stack<RenderTexture>>();
    static Dictionary<(uint, uint), Stack<Texture>> _tex_pool = new Dictionary<(uint, uint), Stack<Texture>>();

    public static RenderTexture GetRT(uint width, uint height,bool addDepth = true)
    {
        if (_rt_pool.TryGetValue((width, height), out var stack))
        {
            if (stack.Count > 0)
            {
                return stack.Pop();
            }
            else
            {
                var rt = new RenderTexture(GlobalVariable.GL, width, height, addDepth: addDepth);
                return rt;
            }
        }
        else
        {
            var rt = new RenderTexture(GlobalVariable.GL, width, height, addDepth: addDepth);
            _rt_pool.Add((width, height), new Stack<RenderTexture>());
            return rt;
        }
    }
    
    public static Texture GetTex(uint width, uint height)
    {
        if (_tex_pool.TryGetValue((width, height), out var stack))
        {
            if (stack.Count > 0)
            {
                return stack.Pop();
            }
            else
            {
                var tex = new Texture(GlobalVariable.GL, new byte[width * height * 4], width, height);
                return tex;
            }
        }
        else
        {
            var tex = new Texture(GlobalVariable.GL, new byte[width * height * 4], width, height);
            _tex_pool.Add((width, height), new Stack<Texture>());
            return tex;
        }
    }
    
    public static void ReturnRT(RenderTexture rt)
    {
        if (_rt_pool.TryGetValue(((uint)rt.Width, (uint)rt.Height), out var stack))
        {
            stack.Push(rt);
        }
        else
        {
            _rt_pool.Add(((uint)rt.Width, (uint)rt.Height), new Stack<RenderTexture>());
            _rt_pool[((uint)rt.Width, (uint)rt.Height)].Push(rt);
        }
    }
    
    public static void ReturnTex(Texture tex)
    {
        if (_tex_pool.TryGetValue(((uint)tex.Width, (uint)tex.Height), out var stack))
        {
            stack.Push(tex);
        }
        else
        {
            _tex_pool.Add(((uint)tex.Width, (uint)tex.Height), new Stack<Texture>());
            _tex_pool[((uint)tex.Width, (uint)tex.Height)].Push(tex);
        }
    }
    
    
    
}