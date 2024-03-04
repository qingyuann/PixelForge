using PixelForge.Physics;

namespace PixelForge;

using Entitas;
using PixelForge.Spawner;
using PixelForge.Spawner.CellAuto;
public sealed class AddGameSystem : Systems {
	/// <summary>
	/// 运行式会先顺序执行一遍构造函数，再顺序执行一遍Initialize函数
	/// 所以创建system时候想清楚把内容放在哪里
	/// </summary>
	/// <param name="contexts"></param>
	public AddGameSystem( Contexts contexts ) {
		//先添加场景物体
		Add( new HierarchySystem( contexts ) );
		//然后把渲染器设置好
		Add( new RenderSystem( contexts ) );
		Add( new PostProcessSystem( contexts ) );


		//处理事件
		Add( new BasicMoveSystem( contexts ) );
		Add( new EnemySystem( contexts, 10 ) );
		//Add( new CellularAutomatonSystem( contexts ) );
		Add( new GameDemo() );
		Add( new CellAutomationSystem( contexts ) );
		Add( new BoxCollisionSystem( contexts ) );

		//最后处理相机
		Add( new CameraSystem( contexts ) );
	}
}