using Entitas;
using Render;

namespace PixelForge.Spawner;

public class CellularAutomatonSystem : IInitializeSystem, IExecuteSystem {

	IGroup<GameEntity> _cellularAutomatonGroup;
	
	
	public CellularAutomatonSystem( Contexts contexts) {
		_cellularAutomatonGroup = GlobalVariable.Contexts.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentCellularAutomaton, GameMatcher.MatSingleRenderer ) );
	}

	public void Initialize() {
		foreach( var e in _cellularAutomatonGroup.GetEntities() ) {
			if( !e.hasMatPara ) {
				e.AddMatPara( null, new Dictionary<string, object>(){
					{
						"MainTex", "silk2.png"
					}
				} );
			}
		}
	}
	public void Execute() {
		throw new NotImplementedException();
	}
}