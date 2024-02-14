using PixelForge;
using System.Numerics;
using PixelForge.Tools;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Render;

public class RendererManager {
	List<Renderer> _renderers = new List<Renderer>();
	GL _gl;
	private Contexts _contexts;
	Vector3 pos = new Vector3( 0f, 0f, 0f );
	Vector2 size = new Vector2( 1, 1 );
	float rotation = 0f;


	public RendererManager( GL gl) {
		_gl = gl;
		_contexts = MainLoop._contexts;


		// var entities = _contexts.game.GetEntities();
		// var poses= new List<Vector3>(){new Vector3( 0.2f,0.5f,0.3f ),new Vector3( 0f,0f,0f )};
		// var sizes= new List<Vector2>(){new Vector2( 0.2f,0.2f ),new Vector2( 0.1f,0.1f )};
		// var rotations= new List<float>(){45,0};
		// _renderers.Add( new RenderQuadInstances( _gl, poses, sizes, rotations, 0 ) );
		// _renderers[0].SetUniform( "uColor", new Vector3( 0, 1f, 0f ) );
		var q=new RenderQuad(  new Vector3( 0f, 0f, 0f ), new Vector2( 1f, 1f ), 0f, 1 ) ;
		// _renderers.Add( new RenderQuad(  pos, size, rotation, 1 ) );
		// _renderers[0].SetUniform( "uColor", new Vector3( 1, 0, 0 ) );

		//
		// _renderers.Add(new RenderQuad(_gl, new Vector3(-0.51f, 0f,0f), 0.1f, 0.1f, 2));
		// _renderers[2].SetUniform("uColor", new Vector3(0, 0, 1));
	}

	public void Render( int layer ) {
		rotation += 0.1f;
		// ( _renderers[0] as RenderQuad ).UpdateQuad( pos, size, rotation);
		foreach( var renderer in _renderers ) {
			if( renderer.Layer == layer ) {
				renderer.Draw();
			}
		}
	}
}