﻿using Entitas;
using Render;
using System.Numerics;

namespace Component;

[Game]
public class SpriteInstanceRendererComponent : IComponent {
	public int ColumnNum;
	public int RowNum;
	public int SpriteIndex;
	public string SpriteName = "silk.png";

}