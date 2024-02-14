namespace PixelForge;
using Entitas;
using PixelForge.Spawner;

public sealed class AddGameSystem : Systems
{
    public AddGameSystem(Contexts contexts) {
        Add( new HierarchySystem( contexts ) );
        
        Add( new CameraSystem( contexts ) );
        Add( new BasicMoveSystem(contexts) );
        Add(new EnemySystem(contexts, 10));
        Add( new RenderSystem(contexts));
    }
}