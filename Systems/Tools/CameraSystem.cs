using Entitas;
using System.Numerics;


namespace PixelForge;

public sealed class CameraSystem : IInitializeSystem, IExecuteSystem {
	public static Matrix4x4 MainCamViewMatrix;

	readonly Contexts _contexts;
	IGroup<GameEntity> _cameraGroup;
	static GameEntity? _mainCamera;

	public CameraSystem( Contexts contexts ) {
		_contexts = contexts;
	}

	public void Initialize() {
		_cameraGroup = _contexts.game.GetGroup( GameMatcher.AllOf( GameMatcher.ComponentCamera, GameMatcher.ComponentPosition ) );
		SetCameraViewMatrix();
	}

	//对筛选出来的entity进行操作
	void SetCameraViewMatrix() {
		foreach( GameEntity gameEntity in _cameraGroup ) {
			if( gameEntity.componentCamera.IsMain ) {
				_mainCamera = gameEntity;
				var posComponent = gameEntity.componentPosition;
				Vector2 pos = new Vector2( posComponent.X, posComponent.Y );
				var scale = 1 / gameEntity.componentCamera.Scale;
				Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation( new Vector3( -pos.X * GlobalVariable.XUnit, -pos.Y * GlobalVariable.YUnit, 0 ) );
				Matrix4x4 scaleMatrix = Matrix4x4.CreateScale( scale );
				MainCamViewMatrix = translationMatrix * scaleMatrix;
				//只有一个主相机
				return;
			}
		}
	}

	/// <summary>
	/// return a camera view matrix by given pos, scale
	/// </summary>
	/// <returns></returns>
	public static Matrix4x4 GetCamMatrix( Vector3 pos, float scale ) {
		Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation( new Vector3( -pos.X * GlobalVariable.XUnit, -pos.Y * GlobalVariable.YUnit, 0 ) );
		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale( scale );
		return translationMatrix * scaleMatrix;
	}

	public static void GetMainCamPara( out Vector2 pos, out float scale ) {
		if( _mainCamera is{ hasComponentCamera: true, hasComponentPosition: true } ) {
			pos = new Vector2( _mainCamera.componentPosition.X, _mainCamera.componentPosition.Y );
			scale = _mainCamera.componentCamera.Scale;
		} else {
			pos = new Vector2( 0, 0 );
			scale = 1;
		}
	}

	public void Execute() {
		SetCameraViewMatrix();
	}
}