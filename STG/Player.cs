using System.Collections;
using STG.Engine;
using STG.Engine.Component;
using STG.Engine.Debugging;
using STG.Engine.Graphics;
using Point = Microsoft.Xna.Framework.Point;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace STG {
    class Player : Behavior {
        override public void Start() {
            SpriteSheet sheet = new SpriteSheet("mat_021", 8, 2, new Point(32, 32), new Point(8, 8), new Point(16, 8));
            var sprite = sheet.SpriteTextures;

            var sr = AddComponent<SpriteRenderer>();

            sr.SortingLayer = Layer("Character");
            sr.SortingOrder = 1;
            sr.texture = sprite[0].texture;
        }

        public override void Update() {
            var position = transform.position;

            if (KeyInput.IsHeld(Keys.Space)) {
                GameObject.Instantiate<Bullet>((int)position.X, (int)position.Y, "playerBullet");
            }

            if (KeyInput.IsHeld(Keys.W)) {
                position.Y -= 1;
            }
            if (KeyInput.IsHeld(Keys.S)) {
                position.Y += 1;
            }
            if (KeyInput.IsHeld(Keys.A)) {
                position.X -= 1;
            }
            if (KeyInput.IsHeld(Keys.D)) {
                position.X += 1;
            }

            transform.position = position;
        }
    }
}
