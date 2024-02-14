using PixelForge;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Render;


public class RenderPipeline
{
    public static GL Gl;
    RenderFullscreen _renderScreen;
    readonly int _layerCount = GameSetting.MaxRenderLayer;
    List<RenderTexture> _layerRt = new List<RenderTexture>();
 
    private readonly Contexts _contexts;
    RenderQuadInstances _quads;
    
    public RenderPipeline(IWindow window)
    {
        Gl = GL.GetApi(window);
        SetupLayerRt(); }

	void SetupLayerRt() {
		for( int i = 0; i < _layerCount; i++ ) {
			_layerRt.Add( new RenderTexture( Gl, GameSetting.WindowWidth, GameSetting.WindowHeight) );
		}

		_renderScreen = new RenderFullscreen(  );
	}

	public void OnRender() {
		Gl.Viewport( 0, 0, GameSetting.WindowWidth, GameSetting.WindowHeight );
		Gl.Clear( (uint)GLEnum.ColorBufferBit );
		Gl.ClearColor( 0.0f, 0.0f, 0.0f, 0.0f );

		for( var index = 0; index < _layerCount; index++ ) {
			_layerRt[index].RenderToRt();
			Gl.Clear( (uint)GLEnum.ColorBufferBit );
			Gl.ClearColor( 0.0f, 0.0f, 0.0f, 0.0f );
			RenderSystem.Render( index );
		}
		
		for( int i = 0; i < _layerCount; i++ ) {
		    string texName = $"uTexture{i}";
		    _renderScreen.SetTexture(i, texName, _layerRt[i].RT);
		}
		
		//渲染到屏幕
		Gl.BindFramebuffer(GLEnum.Framebuffer, 0);
		Gl.Clear((uint)GLEnum.ColorBufferBit);
		_renderScreen.Draw();
	}

	public void OnClose() {
		_renderScreen.Dispose();
		foreach( var rt in _layerRt ) {
			rt.Dispose();
		}
	}
}