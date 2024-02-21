﻿using System.Numerics;

namespace PixelForge.Tools;

public static class Transform {
	public static Vector2 WorldToPixel( Vector2 position ) {
		var ratio = WorldToScreen( new Vector2( position.X, position.Y ) );
		var pos = new Vector2( ratio.X * GameSetting.WindowWidth, ratio.Y * GameSetting.WindowHeight );
		return pos;
	}

	public static Vector2 WorldToScreen( Vector2 position ) {
		CameraSystem.GetMainCamPara( out var camPos, out var camScale );
		position -= new Vector2( camPos.X, camPos.Y );
		Vector2 screenPos = new Vector2( position.X * GlobalVariable.XUnit, position.Y * GlobalVariable.YUnit );
		screenPos /= camScale;
		screenPos = screenPos * new Vector2( 0.5f, 0.5f ) + new Vector2( 0.5f, 0.5f );
		screenPos.Y = 1 - screenPos.Y;
		return screenPos;
	}

	public static Vector2 ScreenToWorld( Vector2 position ) {
		CameraSystem.GetMainCamPara( out var camPos, out var camScale );
		position.Y = 1 - position.Y;
		position = position * new Vector2( 2, 2 ) - new Vector2( 1, 1 );
		position *= camScale;
		position = new Vector2( position.X / GlobalVariable.XUnit, position.Y / GlobalVariable.YUnit );
		position += new Vector2( camPos.X, camPos.Y );
		return position;
	}

	public static Vector2 PixelToWorld( Vector2 position ) {
		var ratio = new Vector2( position.X / GameSetting.WindowWidth, position.Y / GameSetting.WindowHeight );
		var pos = ScreenToWorld( ratio );
		return pos;
	}

	public static float WorldToPixelSize( float value ) {
		return value * GlobalVariable.UnitPixel;
	}
}