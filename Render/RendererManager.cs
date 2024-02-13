using System.Numerics;
using PixelForge.Tools;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Render;

public class RendererManager
{
    List<Renderer> _renderers = new List<Renderer>();
    GL _gl;

    public RendererManager(GL gl)
    {
        _gl = gl;
        
        var poses = new List<float>();
        for (int i = 0; i <8; i++)
        {
            poses.Add(RandomTool.Range(-1f, 1f));
        }
        _renderers.Add(new RenderQuadInstances(_gl, poses, 0.5f, 0.5f, 0));
        _renderers[0].SetUniform("uColor", new Vector3(0, 0.5f, 0.5f));
        
        _renderers.Add(new RenderQuad(_gl, new Vector3(0.5f, 0f, 0f), 0.2f, 0.2f, 0));
        _renderers[1].SetUniform("uColor", new Vector3(1, 0, 0));
        
        _renderers.Add(new RenderQuad(_gl, new Vector3(-0.51f, 0f,0f), 0.2f, 0.2f, 1));
        _renderers[2].SetUniform("uColor", new Vector3(0, 0, 1));
    }

    public void Render(int layer)
    {
        foreach (var renderer in _renderers)
        {
            if (renderer.Layer == layer)
            {
                renderer.Draw();
            }
        }
    }
}