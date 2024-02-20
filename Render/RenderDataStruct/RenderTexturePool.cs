using PixelForge;
using Silk.NET.OpenGL;

namespace Render;

public static class RenderTexturePool
{
    static Dictionary<(uint, uint), Stack<RenderTexture>> _pool = new Dictionary<(uint, uint), Stack<RenderTexture>>();

    public static RenderTexture Get(uint width, uint height)
    {
        if (_pool.TryGetValue((width, height), out var stack))
        {
            if (stack.Count > 0)
            {
                return stack.Pop();
            }
            else
            {
                var rt = new RenderTexture(GlobalVariable.GL, width, height);
                return rt;
            }
        }
        else
        {
            var rt = new RenderTexture(GlobalVariable.GL, width, height);
            _pool.Add((width, height), new Stack<RenderTexture>());
            return rt;
        }
    }

    public static void Return(RenderTexture rt)
    {
        if (_pool.TryGetValue(((uint)rt.Width, (uint)rt.Height), out var stack))
        {
            stack.Push(rt);
        }
        else
        {
            _pool.Add(((uint)rt.Width, (uint)rt.Height), new Stack<RenderTexture>());
            _pool[((uint)rt.Width, (uint)rt.Height)].Push(rt);
        }
    }
}