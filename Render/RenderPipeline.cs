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
    
    private readonly Contexts _contexts;
    RenderQuadInstances _quads;
    
    public RenderPipeline(IWindow window, Contexts contexts)
    {
        _gl = GL.GetApi(window);
        _gl.Viewport( 0, 0, GameSetting.WindowWidth, GameSetting.WindowHeight );
        _contexts = contexts;
        _rendererManager = new RendererManager(_gl, _contexts);
        SetupLayerRt(); }

	void SetupLayerRt() {
		for( int i = 0; i < _layerCount; i++ ) {
			_layerRt.Add( new RenderTexture( _gl, GameSetting.WindowWidth, GameSetting.WindowHeight, i ) );
		}

		_renderScreen = new RenderFullscreen( _gl );
	}

	public void OnRender() {
		_gl.Clear( (uint)GLEnum.ColorBufferBit );
		_gl.ClearColor( 0.0f, 0.0f, 0.0f, 0.0f );


		for( var index = 0; index < _layerCount; index++ ) {
			_layerRt[index].RenderToRt();
			_gl.Clear( (uint)GLEnum.ColorBufferBit );
			_gl.ClearColor( 0.0f, 0.0f, 0.0f, 0.0f );
			_rendererManager.Render( index );
		}
		
		for( int i = 0; i < _layerCount; i++ ) {
		    string texName = $"uTexture{i}";
		    _renderScreen.SetTexture(i, texName, _layerRt[i].RT);
		}
		
		//渲染到屏幕
		_gl.BindFramebuffer(GLEnum.Framebuffer, 0);
		_gl.Clear((uint)GLEnum.ColorBufferBit);
		_renderScreen.Draw();
	}

	public void OnClose() {
		_renderScreen.Dispose();
		foreach( var rt in _layerRt ) {
			rt.Dispose();
		}
	}
}