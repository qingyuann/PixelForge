using Entitas;

namespace PixelForge;

public class BloomSystem : ReactiveSystem <GameEntity>
{
	public BloomSystem( IContext<GameEntity> context ) : base( context ) {
	}
	public BloomSystem( ICollector<GameEntity> collector ) : base( collector ) {
	}
	
	override protected ICollector<GameEntity> GetTrigger( IContext<GameEntity> context ) {
		throw new NotImplementedException();
	}
	override protected bool Filter( GameEntity entity ) {
		throw new NotImplementedException();
	}
	override protected void Execute( List<GameEntity> entities ) {
		throw new NotImplementedException();
	}

}