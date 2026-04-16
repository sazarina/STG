using Microsoft.Xna.Framework;

namespace STG.Engine.Component {
    public class Camera : Component {
        public float Zoom { get; set; } = 1.5f;
        public float Rotation { get; set; } = 0f;

        public Matrix GetViewMatrix() {
            return
                Matrix.CreateTranslation(new Vector3(-transform.position, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                //原点(0,0)を画面の真ん中に持ってくる
                Matrix.CreateTranslation(Screen.Width / 2f, Screen.Height / 2f, 0);
        }
    }
}