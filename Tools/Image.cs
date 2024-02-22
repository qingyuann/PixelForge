using System.Numerics;

namespace PixelForge.Tools;

public static partial class Image {
	/// <summary>
	/// if index is not reachable, return Vector4(0,0,0,0)
	/// </summary>
	/// <param name="data"></param>
	/// <param name="pixelX"></param>
	/// <param name="pixelY"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <returns></returns>
	public static Vector4 TryGetColorPixelRGBA( byte[] data, int pixelX, int pixelY, int width, int height ) {
		var index = TryGetIndex( pixelX, pixelY, width, height );
		return index is null ? new Vector4( 0, 0, 0, 1 ) : new Vector4( data[index.Value * 4], data[index.Value * 4 + 1], data[index.Value * 4 + 2], data[index.Value * 4 + 3] );
	}

	/// <summary>
	/// if index is not reachable, do nothing
	/// </summary>
	/// <param name="data"></param>
	/// <param name="pixelX"></param>
	/// <param name="pixelY"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <param name="color"></param>
	public static void TrySetColorRGBA( ref byte[] data, int pixelX, int pixelY, int width, int height, Vector4 color ) {
		var index = TryGetIndex( pixelX, pixelY, width, height );
		if( index is not null ) {
			data[index.Value * 4] = (byte)color.X;
			data[index.Value * 4 + 1] = (byte)color.Y;
			data[index.Value * 4 + 2] = (byte)color.Z;
			data[index.Value * 4 + 3] = (byte)color.W;
		}
	}

	/// <summary>
	/// return null if index is not reachable
	/// </summary>
	/// <param name="pixelX"></param>
	/// <param name="pixelY"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <returns></returns>
	public static int? TryGetIndex( int pixelX, int pixelY, int width, int height ) {
		if( pixelY < 0 || pixelY >= height || pixelX < 0 || pixelX >= width ) {
			return null;
		}
		return pixelX + pixelY * width;
	}
}