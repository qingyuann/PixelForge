using Entitas;
using System.Numerics;


namespace PixelForge;

public sealed class CameraSystem : IInitializeSystem {
	public static Matrix4x4 MainCamViewMatrix;

	readonly Contexts _contexts;
	IGroup<GameEntity> _cameraGroup;

	public CameraSystem( Contexts contexts ) {
		_contexts = contexts;
	}

	public void Initialize() {
		_cameraGroup =_contexts.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentCamera, GameMatcher.ComponentPosition ) );
		SetCameraViewMatrix();
		_cameraGroup.OnEntityAdded += ( group, entity, index, component ) => {
			SetCameraViewMatrix();
		};
	}

	//对筛选出来的entity进行操作
	void SetCameraViewMatrix() {
		foreach( GameEntity gameEntity in _cameraGroup ) {
			if( gameEntity.componentCamera.IsMain ) {
				var posComponent = gameEntity.componentPosition;
				Vector2 pos = new Vector2( posComponent.X, posComponent.Y );
				var scale = gameEntity.componentCamera.Scale;
				Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation( new Vector3( -pos.X, -pos.Y,  0 ) );
				Matrix4x4 scaleMatrix = Matrix4x4.CreateScale( scale );
				MainCamViewMatrix = translationMatrix * scaleMatrix;
				//只有一个主相机
				return;
			}
		}
	}
}