using STG.Engine.Component;
using STG.Engine.Graphics;
using Point = Microsoft.Xna.Framework.Point;

namespace STG.Engine.Debugging.Scripts {
    internal class SpriteSheetTest : Behavior {
            public override void Start() {
            SpriteSheet sheet = new SpriteSheet("/mat_021.png", "planes", 8, 2, new Point(32, 32), new Point(8, 8), new Point(16, 8));

            var pad = new Point(100, 100);
            for (int y = 0; y < sheet.NumY; y++) {
                for (int x = 0; x < sheet.NumX; x++) {
                    Debug.Log($"{x * 40},{y * 24}");
                    var sprite = sheet.SpriteTextures[y * sheet.NumX + x];
                    var gameObject = GameObject.Instantiate(pad.X + x * sheet.Size.Width, pad.Y + y * sheet.Size.Height, $"{x + 1},{y + 1}");
                    gameObject.AddComponent<SpriteRenderer>().texture = sprite.texture;
                }
            }
        }
    
            public override void Update() {

        }
    }
}
