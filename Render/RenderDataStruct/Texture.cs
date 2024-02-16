using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace Render {
	public class Texture : IDisposable {
		private uint _handle;
		public uint Handle {
			get {
				return _handle;
			}
		}
		private GL _gl;

		public unsafe Texture( GL gl, string path ) {
			_gl = gl;

			_handle = _gl.GenTexture();
			Bind(TextureUnit.Texture31);

			using( var img = Image.Load<Rgba32>( path ) ) {
				gl.TexImage2D( TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)img.Width, (uint)img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null );

				img.ProcessPixelRows( accessor => {
					for( int y = 0; y < accessor.Height; y++ ) {
						fixed (void* data = accessor.GetRowSpan( y )) {
							gl.TexSubImage2D( TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data );
						}
					}
				} );
			}

			SetParameters();
		}

		public void UpdateImage( Span<byte> data, uint width, uint height ) {
			unsafe {
				Bind(TextureUnit.Texture31);

				fixed (void* d = &data[0]) {
					_gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, d);
					SetParameters();
				}
			}

		}
		
		public unsafe Texture( GL gl, Span<byte> data, uint width, uint height ) {
			_gl = gl;

			_handle = _gl.GenTexture();
			Bind(TextureUnit.Texture31);

			fixed (void* d = &data[0]) {
				_gl.TexImage2D( TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d );
				SetParameters();
			}
		}

		private void SetParameters() {
			_gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge );
			_gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge );
			_gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear );
			_gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear );
			_gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0 );
			_gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8 );
			_gl.GenerateMipmap( TextureTarget.Texture2D );
		}

		public void Bind( TextureUnit textureSlot = TextureUnit.Texture0 ) {
			_gl.ActiveTexture( textureSlot );
			_gl.BindTexture( TextureTarget.Texture2D, _handle );
		}

		public void ReleaseBind() {
			_gl.BindTexture( TextureTarget.Texture2D, 0 );
		}
		
		public void Dispose() {
			_gl.DeleteTexture( _handle );
		}
	}
}