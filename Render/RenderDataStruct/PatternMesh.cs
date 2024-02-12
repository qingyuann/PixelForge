using System.Numerics;

namespace Render;


public enum Anchor {
	Center,
	Bottom,
	Top
}

public static class PatternMesh {
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <param name="vertices"></param>
	/// <param name="indices"></param>
	/// <param name="anchor">Center->pos为中间，Bottom->下方，Top->上方</param>
	public static void CreateQuad( Vector2 pos, float width, float height, out float[] vertices, out uint[] indices, Anchor anchor=Anchor.Center ) {
		if( anchor==Anchor.Center) {
			vertices = new float[] {
				//X    Y      Z     U   V
				pos.X-width/2, pos.Y-height/2, 0f, 0.0f, 0.0f,
				pos.X+width/2, pos.Y-height/2, 0f, 1.0f, 0.0f,
				pos.X+width/2, pos.Y+height/2, 0f, 1.0f, 1.0f,
				pos.X-width/2, pos.Y+height/2, 0f, 0.0f, 1.0f,
			};
		} else if( anchor==Anchor.Bottom ) {
			vertices = new float[] {  
				//X    Y      Z     U   V
				pos.X-width/2, pos.Y, 0f, 0.0f, 0.0f,
				pos.X+width/2, pos.Y, 0f, 1.0f, 0.0f,
				pos.X+width/2, pos.Y+height, 0f, 1.0f, 1.0f,
				pos.X-width/2, pos.Y+height, 0f, 0.0f, 1.0f,
			};
		} else {
			vertices = new float[] {  
				//X    Y      Z     U   V
				pos.X-width/2, pos.Y-height, 0f, 0.0f, 0.0f,
				pos.X+width/2, pos.Y-height, 0f, 1.0f, 0.0f,
				pos.X+width/2, pos.Y, 0f, 1.0f, 1.0f,
				pos.X-width/2, pos.Y, 0f, 0.0f, 1.0f,
			};
		}
		indices = new uint[] {
			0, 1, 3,
			1, 2, 3
		};
	}
}