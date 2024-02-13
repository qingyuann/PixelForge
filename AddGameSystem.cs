namespace PixelForge;
using Entitas;
using PixelForge.Spawner;

public sealed class AddGameSystem : Systems
{
    public AddGameSystem(Contexts contexts)
    {
        Add(new EnemySystem(contexts, 10));
    }
}