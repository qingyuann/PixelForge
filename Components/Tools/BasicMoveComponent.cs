using Entitas;

namespace Component;

[Game]
public class BasicMoveComponent : IComponent {
	public bool IsActive;
	public float Speed;
}