using Entitas;
using Render;
using Silk.NET.OpenGL;
using System.Numerics;

namespace PixelForge;

public class RenderSystem : IInitializeSystem {
	Contexts _contexts;
	static IGroup<GameEntity> _RenderGroup;
	static IGroup<GameEntity> _RenderInstanceGroup;
	GL _gl;
	List<Renderer> _renderers = new List<Renderer>();

	public RenderSystem( Contexts contexts ) {
		_contexts = contexts;
		_gl = GlobalVariable.Gl;
	}

	public void Initialize() {
		//渲染单个的sprite
		_RenderGroup = _contexts.game.GetGroup(
			GameMatcher.AllOf(
				GameMatcher.ComponentRenderSinglePara,
				GameMatcher.ComponentPosition,
				GameMatcher.ComponentSize,
				GameMatcher.ComponentRotation ).NoneOf(
				GameMatcher.ComponentSpriteInstanceRenderer ) );

		foreach( GameEntity e in _RenderGroup.GetEntities() ) {
			var pos = new Vector3( e.componentPosition.X, e.componentPosition.Y, e.componentPosition.Z );
			var size = new Vector2( e.componentSize.X, e.componentSize.Y );
			var rotation = e.componentRotation.Rot;
			var spriteAttribute = e.componentRenderSinglePara;

			e.AddComponentSingleRenderer( new RenderQuad( pos, size, rotation, spriteAttribute.Layer ) );
			var renderer = e.componentSingleRenderer.Renderer;
			renderer.SetUniform( "uColor", new Vector3( 1, 1, 0 ) );
		}

		// //渲染批量的sprite
		// _RenderInstanceGroup = _contexts.game.GetGroup(
		// 	GameMatcher.AllOf(
		// 		GameMatcher.ComponentSpriteRenderer,
		// 		GameMatcher.ComponentSpriteInstanceRenderer,
		// 		GameMatcher.ComponentPosition,
		// 		GameMatcher.ComponentSize,
		// 		GameMatcher.ComponentRotation ) );
		// //获得批量渲染的种类
		// var entities = _RenderInstanceGroup.GetEntities();
		// var spriteNames = entities.Select( e => e.componentSpriteRenderer.SpriteName ).GroupBy( x => x ).Select( x => x.Key ).ToArray();
		// foreach( var spriteName in spriteNames ) {
		// 	var instances = entities.Where( e => e.componentSpriteRenderer.SpriteName == spriteName ).ToList();
		// 	var poses = instances.Select( e => new Vector3( e.componentPosition.X, e.componentPosition.Y, e.componentPosition.Z ) ).ToList();
		// 	var sizes = instances.Select( e => new Vector2( e.componentSize.X, e.componentSize.Y ) ).ToList();
		// 	var rotations = instances.Select( e => e.componentRotation.Rot ).ToList();
		// 	var layer = instances[0].componentSpriteRenderer.Layer;
		// 	_renderers.Add( new RenderQuadInstances( _gl, poses, sizes, rotations, layer ) );
		// }
	}

	public static void Render( int layer ) {
		//渲染单个的sprite
		//更新渲染参数
		foreach( GameEntity e in _RenderGroup.GetEntities() ) {
			var pos = new Vector3( e.componentPosition.X, e.componentPosition.Y, e.componentPosition.Z );
			var size = new Vector2( e.componentSize.X, e.componentSize.Y );
			var rotation = e.componentRotation.Rot;
			if( e.hasComponentSingleRenderer ) {
				 e.componentSingleRenderer.Renderer.UpdateQuad( pos, size, rotation );
			}
		}

		foreach( var e in _RenderGroup ) {
			var eLayer = e.componentRenderSinglePara.Layer;
			if( eLayer == layer ) {
				if( e.hasComponentSingleRenderer ) {
					e.componentSingleRenderer.Renderer.Draw();
				}
			}
		}
	}
}