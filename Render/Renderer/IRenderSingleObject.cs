using System.Numerics;

namespace Render;

public interface IRenderSingleObject {
	float[] Vertices { get; set; }
	protected uint[] Indices{ get; set; }
	protected BufferObject<float> Vbo{ get; set; }
	protected BufferObject<uint> Ebo{ get; set; }
	protected VertexArrayObject<float, uint> Vao{ get; set; }
	public void UpdateTransform(Vector3 pos, Vector2 scale, float rotation, Anchor anchor = Anchor.Center);
	public void Draw();
	public void Dispose();
}