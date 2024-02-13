using System.Numerics;
using PixelForge.Tools;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Render;

public class RendererManager
{
    List<Renderer> _renderers = new List<Renderer>();
    GL _gl;
    private Contexts _contexts;
    

    public RendererManager(GL gl, Contexts contexts)
    {
        _gl = gl;
        _contexts = contexts;
        
        
        
        //_renderers.Add(new RenderQuad(_gl, new Vector3(0.5f, 0f, 0f), 0.2f, 0.2f, 0));
        //_renderers[1].SetUniform("uColor", new Vector3(1, 0, 0));
        
        //_renderers.Add(new RenderQuad(_gl, new Vector3(-0.51f, 0f,0f), 0.2f, 0.2f, 1));
        //_renderers[2].SetUniform("uColor", new Vector3(0, 0, 1));
    }

    public void Render(int layer)
    {
        var entities = _contexts.game.GetEntities();
        //Console.WriteLine(entities.Length);
        var poses = new List<float>();
        foreach (var entity in entities)
        {
            if (entity.hasPixelForgeBasicComponentscsPosition)
            {
                Console.WriteLine(entity.pixelForgeBasicComponentscsPosition.X);
                poses.Add(entity.pixelForgeBasicComponentscsPosition.X);
                poses.Add(entity.pixelForgeBasicComponentscsPosition.Y);
            }
        }
        _renderers.Add(new RenderQuadInstances(_gl, poses, 0.5f, 0.5f, 0));
        _renderers[0].SetUniform("uColor", new Vector3(0, 0.5f, 0.5f));
        
        foreach (var renderer in _renderers)
        {
            if (renderer.Layer == layer)
            {
                renderer.Draw();
            }
        }
    }
}