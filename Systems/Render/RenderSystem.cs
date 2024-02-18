using Entitas;
using Mat;
using Render;
using Silk.NET.OpenGL;
using System.Numerics;
using Texture = Render.Texture;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace PixelForge;

public class RenderSystem : IInitializeSystem {
	static Contexts _contexts;
	static IGroup<GameEntity> _renderGroup;
	static IGroup<GameEntity> _renderInstanceGroup;

	public RenderSystem( Contexts contexts ) {
		_contexts = contexts;
	}

	public void Initialize() {
		SetupRendererGroup();
	}
	
	public static void Render( int layer ) {
		UpdateRenderGroupTransform();
		foreach( var e in _renderGroup ) {
			var eLayer = e.matRenderSingle.Layer;
			if( eLayer == layer ) {
				if( e.hasMatRenderSingle ) {
					e.matRenderSingle.Renderer?.Draw();
				}
			}
		}
	}

	static void SetupRendererGroup() {
		//渲染单个的sprite
		_renderGroup = _contexts.game.GetGroup(
			GameMatcher.AllOf(
				GameMatcher.MatRenderSingle,
				GameMatcher.ComponentPosition,
				GameMatcher.ComponentSize,
				GameMatcher.ComponentRotation ).NoneOf(
				GameMatcher.MatSpriteInstanceRenderer ) );

		foreach( GameEntity e in _renderGroup.GetEntities() ) {
			SetUpSingleRenderer( e );
		}

		_renderGroup.OnEntityAdded += ( _, entity, _, _ ) => {
			SetUpSingleRenderer( entity );
		};
	}

	static void SetUpSingleRenderer( GameEntity e ) {
		var pos = new Vector3( e.componentPosition.X, e.componentPosition.Y, e.componentPosition.Z );
		var size = new Vector2( e.componentSize.X, e.componentSize.Y );
		var rotation = e.componentRotation.Rot;
		var spriteAttribute = e.matRenderSingle;

		e.matRenderSingle.Renderer ??= new RenderQuad( pos, size, rotation, spriteAttribute.Layer );
		if( e.hasMatPara ) {
			SetRendererPara( e );
		} else {
			//响应para的添加
			e.OnComponentAdded += ( _, _, component ) => {
				if( component is ParaComponent ) {
					SetRendererPara( e );
				}
			};
		}
		//响应para变化
		e.OnComponentReplaced += ( _, _, _, newComponent ) => {
			if( newComponent is ParaComponent ) {
				SetRendererPara( e );
			}
		};
	}

	/// <summary>
	/// 响应式设置渲染参数
	/// </summary>
	/// <param name="e"></param>
	static void SetRendererPara( GameEntity e ) {
		//set para
		if( e is{ hasMatPara: true, hasMatRenderSingle: true } ) {
			RenderQuad? renderer = e.matRenderSingle.Renderer;
			if( e.matPara.ParaDict != null ) {
				foreach( var para in e.matPara.ParaDict ) {
					renderer?.SetUniform( para.Key, para.Value );
				}
			}
			if( e.matPara.TextureDict != null ) {
				foreach( var texture in e.matPara.TextureDict ) {
					if( texture.Value is string value ) {
						renderer?.SetTexture( texture.Key, value );
					} else if( texture.Value is Texture textureValue ) {
						renderer?.SetTexture( texture.Key, textureValue );
					}
				}
			}
		}
	}

	/// <summary>
	/// 更新单例渲染组的属性
	/// </summary>
	static void UpdateRenderGroupTransform() {
		//渲染单个的sprite
		//更新渲染参数
		foreach( GameEntity e in _renderGroup.GetEntities() ) {
			var pos = new Vector3( e.componentPosition.X, e.componentPosition.Y, e.componentPosition.Z );
			var size = new Vector2( e.componentSize.X, e.componentSize.Y );
			var rotation = e.componentRotation.Rot;
			if( e.hasMatRenderSingle ) {
				e.matRenderSingle.Renderer?.UpdateTransform( pos, size, rotation );
			}
		}
	}

	//TODO: 构思一下批量渲染的架构
	static void SetupInstanceRenderGroup() {
		//渲染批量的sprite
		_renderInstanceGroup = _contexts.game.GetGroup(
			GameMatcher.AllOf(
				GameMatcher.MatSpriteInstanceRenderer,
				GameMatcher.ComponentPosition,
				GameMatcher.ComponentSize,
				GameMatcher.ComponentRotation ) );
		//获得批量渲染的种类
		var entities = _renderInstanceGroup.GetEntities();
	}
	
}