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
	public static void CreateQuad( Vector3 pos, Vector2 size, float rotation,out float[] vertices, out uint[] indices, Anchor anchor=Anchor.Center ) {
		var width = size.X*2;
		var height = size.Y*2;
		float[] tempVertices;
		uint[] tempIndices;
		if( anchor==Anchor.Center) {
			tempVertices = new float[] {
				//X    Y      Z     U   V
				pos.X-width/2, pos.Y-height/2, pos.Z, 0.0f, 0.0f,
				pos.X+width/2, pos.Y-height/2, pos.Z, 1.0f, 0.0f,
				pos.X-width/2, pos.Y+height/2, pos.Z, 0.0f, 1.0f,
				pos.X+width/2, pos.Y-height/2, pos.Z, 1.0f, 0.0f,
				pos.X+width/2, pos.Y+height/2, pos.Z, 1.0f, 1.0f,
				pos.X-width/2, pos.Y+height/2, pos.Z, 0.0f, 1.0f,
			};
		} else if( anchor==Anchor.Bottom ) {
			tempVertices = new float[] {  
				//X    Y      Z     U   V
				pos.X-width/2, pos.Y, pos.Z, 0.0f, 0.0f,
				pos.X+width/2, pos.Y, pos.Z, 1.0f, 0.0f,
				pos.X-width/2, pos.Y+height,pos.Z, 0.0f, 1.0f,
				pos.X+width/2, pos.Y, pos.Z, 1.0f, 0.0f,
				pos.X+width/2, pos.Y+height, pos.Z, 1.0f, 1.0f,
				pos.X-width/2, pos.Y+height,pos.Z, 0.0f, 1.0f,
			};
		} else {
			tempVertices = new float[] {  
				//X    Y      Z     U   V
				pos.X-width/2, pos.Y-height, pos.Z, 0.0f, 0.0f,
				pos.X+width/2, pos.Y-height, pos.Z, 1.0f, 0.0f,
				pos.X-width/2, pos.Y, pos.Z, 0.0f, 1.0f,
				pos.X+width/2, pos.Y-height, pos.Z, 1.0f, 0.0f,
				pos.X+width/2, pos.Y, pos.Z, 1.0f, 1.0f,
				pos.X-width/2, pos.Y, pos.Z, 0.0f, 1.0f,
			};
		}
		
		float angleInRadians = (float)(rotation * Math.PI / 180);
		Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(angleInRadians);
		for( int i = 0; i < 6; i++ ) {
			Vector2 point = new Vector2(tempVertices[i * 5], tempVertices[i * 5 + 1]);
			point = Vector2.Transform(point, rotationMatrix);
			tempVertices[i * 5] = point.X;
			tempVertices[i * 5 + 1] = point.Y;
		};
			
		tempIndices = new uint[] {
			0,1,2,3,4,5
		};
		vertices = tempVertices;
		indices = tempIndices;
	}	
	
	public static void CreateQuadDefault( out float[] vertices, out uint[] indices) {
		CreateQuad( new Vector3( 0, 0, 0 ), new Vector2( 1, 1 ), 0, out vertices, out  indices );
	}
	
}
	
		
	
