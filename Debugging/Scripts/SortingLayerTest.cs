using Microsoft.Xna.Framework;
using STG.Engine.Component;
using STG.Engine.Graphics;
using Point = Microsoft.Xna.Framework.Point;

namespace STG.Engine.Debugging.Scripts {
    internal class SortingLayerTest : Behavior {
        public override void Start() {
            var cameraObject = GameObject.Find("Camera");

            if (cameraObject == null) {
                Debug.LogException("SortingLayerTest", new Exception("Cameraオブジェクトが見つかりませんでした。"));
                return;
            }

            var camera = cameraObject.GetComponent<Camera>();
            if (camera == null) {
                Debug.LogException("SortingLayerTest", new Exception("Cameraコンポーネントが見つかりませんでした。"));
                return;
            }

            Vector2 screenPos = Vector2.Transform(
                    transform.position,
                    camera.GetViewMatrix()
                );
            Debug.Log(screenPos);

            var sprite = new SpriteSheet("planes", 8, 2, new Point(32, 32), new Point(8, 8), new Point(16, 8)).SpriteTextures;

            var obj = GameObject.Instantiate(0, 0, "enemy");
            var sr = obj.AddComponent<SpriteRenderer>();
            sr.SortingLayer = Layer("UI");
            sr.SortingOrder = 0;
            sr.texture = sprite[4].texture;
        }

        public override void Update() {

        }
    }
}