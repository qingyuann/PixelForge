using System.Numerics;

namespace Render;

public interface IRenderInstanceObject {
	protected float[] Vertices { get; set; }
	protected uint[] Indices{ get; set; }
	protected BufferObject<float> Vbo{ get; set; }
	protected BufferObject<uint> Ebo{ get; set; }
	protected VertexArrayObject<float, uint> Vao{ get; set; }
	public void UpdateInstance(List<Vector3> pos, List<Vector2> scale, List<float> rotations, Anchor anchor = Anchor.Center);
}