using PixelForge;
using Silk.NET.OpenGL;

namespace Render;

public static class RenderTexturePool {
	static Stack<RenderTexture> _pool = new Stack<RenderTexture>();

	public static RenderTexture Get( uint width, uint height ) {
		if( _pool.Count > 0 ) {
			var rt = _pool.Pop();
			if( rt.Width == width && rt.Height == height ) {
				return rt;
			} else {
				rt.Dispose();
			}
		}

		return new RenderTexture( GlobalVariable.GL, width, height, 0 );
	}

	public static void Return( RenderTexture rt ) {
		if (_pool.Count < 10)
		{
			_pool.Push(rt);
		}
		else
		{
			rt.Dispose();
		}
	}
}