namespace PixelForge;

using Entitas;
using PixelForge.Spawner;
public sealed class AddGameSystem : Systems {
	public AddGameSystem( Contexts contexts ) {
		//先添加场景物体
		Add( new HierarchySystem( contexts ) );

		//处理事件
		Add( new BasicMoveSystem( contexts ) );
		Add( new EnemySystem( contexts, 10 ) );
		Add( new CellularAutomatonSystem( contexts ) );

		//最后处理渲染
		Add( new CameraSystem( contexts ) );
		Add( new RenderSystem( contexts ) );
	}
}