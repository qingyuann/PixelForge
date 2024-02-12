using System;
using System.Numerics;

namespace Render
{
    public class Camera
    {
        public Vector2 Position;
        public float Scale;

        public Camera(Vector2 position, float scale)
        {
            Position = position;
            Scale = scale;
        }

        public Matrix4x4 GetViewMatrix()
        {
            Matrix4x4 translation = Matrix4x4.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0));
            Matrix4x4 scale = Matrix4x4.CreateScale(Scale);
            return translation * scale;
        }
    }
}