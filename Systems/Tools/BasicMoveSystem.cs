using Entitas;
using Silk.NET.Input;

namespace PixelForge;

public class BasicMoveSystem : IInitializeSystem, IExecuteSystem {

	Contexts _context;
	int _accelerator = 1;
	IGroup<GameEntity> _controllerGroup;

	public BasicMoveSystem( Contexts contexts ) {
		_context = contexts;
	}

	public void Initialize() {
		_controllerGroup = _context.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentBasicMove, GameMatcher.ComponentPosition ) );
	}

	public void Execute() {
		foreach( GameEntity e in _controllerGroup.GetEntities() ) {
			var move = e.componentBasicMove;
			if( move.IsActive ) {
				var pos = e.componentPosition;
				var update = false;
				if( InputSystem.GetKey( Key.W ) ) {
					pos.Y += move.Speed * GameSetting.DeltaTime*_accelerator;
				}
				if( InputSystem.GetKey( Key.S ) ) {
					pos.Y -= move.Speed * GameSetting.DeltaTime*_accelerator;
				}
				if( InputSystem.GetKey( Key.A ) ) {
					pos.X -= move.Speed * GameSetting.DeltaTime*_accelerator;
				}
				if( InputSystem.GetKey( Key.D ) ) {
					pos.X += move.Speed * GameSetting.DeltaTime*_accelerator;
				}
				_accelerator = InputSystem.GetKey( Key.ShiftLeft ) ? 2 : 1;
			}
		}
	}
}