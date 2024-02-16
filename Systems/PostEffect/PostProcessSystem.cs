using Entitas;
using Render;

namespace PixelForge.PostEffect;

public sealed class PostProcessSystem : IExecuteSystem {
	readonly Contexts _contexts;
	Entity _postEffectEntity;

	public PostProcessSystem() {
		_contexts = GlobalVariable.Contexts;
		if( _contexts.game.hasPPPostProcess ) {
			_postEffectEntity = _contexts.game.pPPostProcessEntity;
		} 
	
	}

	public void Render() {

	}
	public void Execute() {
		throw new NotImplementedException();
	}
}