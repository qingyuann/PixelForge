using PixelForge;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Render;

public class RenderPipeline
{
    static GL _gl;
    RenderFullscreen _renderScreen;
    readonly int _layerCount = GameSetting.MaxRenderLayer;
    List<RenderTexture> _layerRt = new List<RenderTexture>();
    RendererManager _rendererManager;
    
    
    RenderQuadInstances _quads;
    
    public RenderPipeline(IWindow window)
    {
        _gl = GL.GetApi(window);
        _rendererManager = new RendererManager(_gl);
        SetupLayerRt();
        

    }

    public void OnRender()
    {
        _gl.Viewport(0, 0, GameSetting.WindowWidth, GameSetting.WindowHeight);
        _gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
        _gl.DrawBuffer(GLEnum.ColorAttachment0);

        for (var index = 0; index < _layerRt.Count; index++)
        {
            var rt = _layerRt[index];
            rt.RenderToRt();
            _gl.Clear((uint)GLEnum.ColorBufferBit);
            _rendererManager.Render(index);
        }

        //渲染到屏幕
        _gl.BindFramebuffer(GLEnum.Framebuffer, 0);
        _gl.Clear((uint)GLEnum.ColorBufferBit);
        _renderScreen.Draw();
    }

    void SetupLayerRt()
    {
        for (int i = 0; i < _layerCount; i++)
        {
            _layerRt.Add(new RenderTexture(_gl, GameSetting.WindowWidth, GameSetting.WindowHeight));
        }

        _renderScreen = new RenderFullscreen(_gl);
        _renderScreen.SetTexture(0, "uTexture0", _layerRt[0].RT);
        _renderScreen.SetTexture(1, "uTexture1", _layerRt[1].RT);
    }

    /************************************改下面的***************************************/
    public void OnClose()
    {
        _renderScreen.Dispose();
        foreach (var rt in _layerRt)
        {
            rt.Dispose();
        }
    }
}