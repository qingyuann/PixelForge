using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Component;

[Game]
public class CameraComponent : IComponent {
	public int Id;
	public bool IsMain;
	public float Scale;
}