using STG.Engine.Component;
using STG.Engine.Graphics;
using Point = Microsoft.Xna.Framework.Point;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using STG.Engine;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;

namespace STG {
    class Player : Behavior {
        SpriteRenderer sr;
        Texture2D[] textures = new Texture2D[2];

        override public void Start() {
            SpriteSheet sheet = new SpriteSheet("/mat_021.png", "planes", 8, 2, new Point(32, 32), new Point(8, 8), new Point(16, 8));
            var sprite = sheet.SpriteTextures;

            textures[0] = sprite[2].texture;
            textures[1] = sprite[10].texture;

            sr = AddComponent<SpriteRenderer>();

            sr.SortingLayer = Layer("Character");
            sr.SortingOrder = 1;
            sr.texture = sprite[0].texture;
            StartCoroutine(ChangeTextureCorutine());
        }

        IEnumerator ChangeTextureCorutine() { 
            while (true) {
                sr.texture = textures[0];
                yield return WaitForSeconds(0.5f); 
                sr.texture = textures[1];
                yield return WaitForSeconds(0.5f);
            }
        }

        public override void Update() {
            Move();

        }

        void Move() {
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
