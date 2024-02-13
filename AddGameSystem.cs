using Entitas;
using PixelForge.Spawner;

namespace PixelForge;

public sealed class AddGameSystem : Systems
{
    public AddGameSystem(Contexts contexts)
    {
        Add(new EnemySystem(contexts, 10));
        Console.WriteLine("AddGameSystem Created!");
    }
}