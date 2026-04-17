using Microsoft.Xna.Framework;

namespace STG.Engine.Component {
    public class Camera : Component {
        public float Zoom { get; set; } = 1.5f;
        public float Rotation { get; set; } = 0f;


        /// <summary>
        /// 画面上に表示されている範囲を表す矩形。ワールド座標系で表される。
        /// </summary>
        public Rectangle ViewRect => new Rectangle() {
            X = (int)(transform.position.X - (Screen.Width / Zoom) / 2),
            Y = (int)(transform.position.Y - (Screen.Height / Zoom) / 2),
            Width = (int)(Screen.Width/Zoom),
            Height = (int)(Screen.Height/Zoom)
        };

        /// <summary>
        /// カメラの位置、回転、ズームを考慮した変換行列を取得する。
        /// </summary>
        /// <returns>カメラのビュー行列</returns>
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