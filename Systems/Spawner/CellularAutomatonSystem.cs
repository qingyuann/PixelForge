using Entitas;
using Render;

namespace PixelForge.Spawner;

public class CellularAutomatonSystem : IInitializeSystem, IExecuteSystem {

	private	readonly Contexts _contexts;
	IGroup<GameEntity> _cellularAutomatonGroup;
	byte color = 255;
	Byte[] data;

	public CellularAutomatonSystem( Contexts contexts )
	{
		_contexts = contexts;
	}

	public void Initialize() {
		//entity 创建时添加trigger-> render system识别trigger添加renderer->_cellularAutomatonGroup识别到renderer后添加成员->识别到成员添加后加入para->renderer识别到para后将纹理添加到shader
		_cellularAutomatonGroup = _contexts.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentCellularAutomation, GameMatcher.MatRenderSingle ) );

		//分配一块256*256的纹理内存,基于内存数据创建纹理
		data = new Byte[256 * 256 * 4];
		for( int y = 0; y < 256; y++ ) {
			for( int x = 0; x < 256; x++ ) {
				// 计算在字节数组中的偏移量
				int offset = ( y * 256 + x ) * 4;
				// 设置像素的RGBA值
				data[offset] = color; // 红色通道
				data[offset + 1] = color; // 绿色通道
				data[offset + 2] = color; // 蓝色通道
				data[offset + 3] = color; // 透明度通道
			}
		}

		foreach( var e in _cellularAutomatonGroup.GetEntities() ) {
			if( e.hasMatRenderSingle && e.matRenderSingle.Renderer is not null ) {
				//添加纹理
				SetTexture( e );
			}
		}
		
		_cellularAutomatonGroup.OnEntityAdded += ( _,_,_,_ ) => {
			//添加纹理
			SetTexture( _cellularAutomatonGroup.GetEntities()[0] );
		};
	}
	
	void SetTexture( GameEntity e ) {
		//这里新建纹理，添加到entity的matPara中，render system识别到para后将纹理添加到shader
		//shader中会储存纹理的引用，所以这里不需要储存纹理的引用
		Texture tempTexture = new Texture( GlobalVariable.GL, data, 256, 256 );
		if( !e.hasMatPara ) {
			//不添加属性参数，添加纹理参数
			e.AddMatPara( null, new Dictionary<string, object>(){
				{
					"MainTex", tempTexture
				}
			} );
		}
	}
	
	public void Execute() {
		foreach( var e in _cellularAutomatonGroup.GetEntities() ) {
			//逐帧更新纹理，这里只是每帧将纹理数值-1
			if( color > 0 ) {
				color -= 1;
			}
			if( e.matRenderSingle.Renderer is not null ) {
				for( int y = 0; y < 256; y++ ) {
					for( int x = 0; x < 256; x++ ) {
						// 计算在字节数组中的偏移量
						int offset = ( y * 256 + x ) * 4;
						// 设置像素的RGBA值
						data[offset] = color; // 红色通道
						data[offset + 1] = color; // 绿色通道
						data[offset + 2] = color; // 蓝色通道
						data[offset + 3] = color; // 透明度通道
					}
				}
				e.matRenderSingle.Renderer.Textures["MainTex"].UpdateImageContent( data, 256, 256 );
			}
		}
	}
}