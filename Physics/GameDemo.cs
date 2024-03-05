using System.Numerics;
using Entitas;
using PixelForge.Spawner.CellAuto;
using PixelForge.Tools;
using Silk.NET.Input;

namespace PixelForge.Physics;

public class GameDemo : IInitializeSystem, IExecuteSystem {
	private string _funcName = "Stone";
	List<GameEntity> lights = new List<GameEntity>();
	public void Initialize() {
		//InputSystem.AddMouseDownEvent( OnMouseDown );
	}

	public void Execute() {
		if( InputSystem.GetKey( Key.Number1 ) ) {
			_funcName = "Stone";
		}
		if( InputSystem.GetKey( Key.Number2 ) ) {
			_funcName = "Water";
		}
		if( InputSystem.GetKey( Key.Number3 ) ) {
			_funcName = "Sand";
		}
		if( InputSystem.GetKey( Key.Number4 ) ) {
			_funcName = "Oil";
		}
		if( InputSystem.GetKey( Key.Number5 ) ) {
			_funcName = "Acid";
		}
		if( InputSystem.GetKey( Key.Number6 ) ) {
			_funcName = "Lava";
		}
		if( InputSystem.GetKey( Key.Number7 ) ) {
			_funcName = "fire";
		}
		if( InputSystem.GetMouseDown( mouseButton:MouseButton.Middle, out var p ) ) {
			var worldPos = Transform.PixelToWorld( p );
			lights.Add( GlobalVariable.Contexts.game.CreateEntity() );
			lights.Last().AddComponentPosition( worldPos.X, worldPos.Y, 0 );
			lights.Last().AddLightShadowLight( true, new[]{ 1 }, lights.Count+1, 3*(RandomTool.Float()*0.5f+0.5f), 1f, (RandomTool.Float()*0.3f+0.7f), new Vector3( ( RandomTool.Float() *0.3f)+0.6f,( RandomTool.Float() *0.3f)+0.6f,( RandomTool.Float() *0.3f)+0.6f),  1f, 0f );
		}

		if( InputSystem.GetMouse( MouseButton.Right, out var pos ) ) {
			OnMousePressed( pos );
		}
	}

	private void OnMousePressed( Vector2 pos ) {
		//Generate some cells of certain type at mouse position
		var x = (int)( pos.X / GameSetting.WindowWidth * CellAutomationSystem._width );
		var y = (int)( pos.Y / GameSetting.WindowHeight * CellAutomationSystem._height );
		switch( _funcName ) {
			case "Stone":
				CellAutomationSystem.TestGenerateStone( x, y );
				break;
			case "Water":
				CellAutomationSystem.TestGenerateWater( x, y );
				break;
			case "Sand":
				CellAutomationSystem.TestGenerateSand( x, y );
				break;
			case "Oil":
				CellAutomationSystem.TestGenerateOli( x, y );
				break;
			case "Acid":
				CellAutomationSystem.TestGenerateAcid( x, y );
				break;
			case "Lava":
				CellAutomationSystem.TestGenerateLava( x, y );
				break;
			case "fire":
				CellAutomationSystem.TestGenerateFire( x, y );
				break;
		}

	}
}