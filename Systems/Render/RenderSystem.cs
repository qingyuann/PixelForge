using Entitas;
using Render;
using Silk.NET.OpenGL;
using System.Numerics;

namespace PixelForge;

public class RenderSystem : IInitializeSystem {
	static Contexts _contexts;
	static IGroup<GameEntity> _RenderGroup;
	static IGroup<GameEntity> _RenderInstanceGroup;
	GL _gl;
	List<Renderer> _renderers = new List<Renderer>();

	public RenderSystem( Contexts contexts ) {
		_contexts = contexts;
		_gl = GlobalVariable.Gl;
	}

	public void Initialize() {
		SetupRendererGroup();
	}
	
	public static void Render( int layer ) {
		UpdateRenderGroup();
		foreach( var e in _RenderGroup ) {
			var eLayer = e.componentRenderSinglePara.Layer;
			if( eLayer == layer ) {
				if( e.hasComponentSingleRenderer ) {
					e.componentSingleRenderer.Renderer.Draw();
				}
			}
		}
	}
	
	static void SetupRendererGroup() {

		void SetUpSingleRenderer( GameEntity e ) {

			var pos = new Vector3( e.componentPosition.X, e.componentPosition.Y, e.componentPosition.Z );
			var size = new Vector2( e.componentSize.X, e.componentSize.Y );
			var rotation = e.componentRotation.Rot;
			var spriteAttribute = e.componentRenderSinglePara;

			e.AddComponentSingleRenderer( new RenderQuad( pos, size, rotation, spriteAttribute.Layer ) );
			var renderer = e.componentSingleRenderer.Renderer;
			renderer.SetUniform( "uColor", new Vector3( 1, 1, 0 ) );
		}

		//渲染单个的sprite
		_RenderGroup = _contexts.game.GetGroup(
			GameMatcher.AllOf(
				GameMatcher.ComponentRenderSinglePara,
				GameMatcher.ComponentPosition,
				GameMatcher.ComponentSize,
				GameMatcher.ComponentRotation ).NoneOf(
				GameMatcher.ComponentSpriteInstanceRenderer ) );

		foreach( GameEntity e in _RenderGroup.GetEntities() ) {
			SetUpSingleRenderer( e );
		}

		_RenderGroup.OnEntityAdded += ( group, entity, index, component ) => {
			SetUpSingleRenderer( entity );
		};
	}
	
	static void UpdateRenderGroup() {
		//渲染单个的sprite
		//更新渲染参数
		foreach( GameEntity e in _RenderGroup.GetEntities() ) {
			var pos = new Vector3( e.componentPosition.X, e.componentPosition.Y, e.componentPosition.Z );
			var size = new Vector2( e.componentSize.X, e.componentSize.Y );
			var rotation = e.componentRotation.Rot;
			if( e.hasComponentSingleRenderer ) {
				e.componentSingleRenderer.Renderer.Update( pos, size, rotation );
			}
		}
	}

	
	//TODO: 构思一下批量渲染的架构
	static void SetupInstanceRenderGroup() {
		//渲染批量的sprite
		_RenderInstanceGroup = _contexts.game.GetGroup(
			GameMatcher.AllOf(
				GameMatcher.ComponentSpriteInstanceRenderer,
				GameMatcher.ComponentPosition,
				GameMatcher.ComponentSize,
				GameMatcher.ComponentRotation ) );
		//获得批量渲染的种类
		var entities = _RenderInstanceGroup.GetEntities();
	}
}