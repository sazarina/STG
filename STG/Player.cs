using System.Collections;
using STG.Engine;
using STG.Engine.Component;
using STG.Engine.Debugging;
using STG.Engine.Graphics;
using Point = Microsoft.Xna.Framework.Point;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace STG {
    class Player : Behavior {
        bool isShot = false;

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

            if (KeyInput.IsHeld(Keys.Space) && !isShot) {
                StartCoroutine(Shot());
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

        IEnumerator Shot() {
            isShot = true;
            while (KeyInput.IsHeld(Keys.Space)) {
                var bullet = GameObject.Instantiate<Bullet>((int)transform.position.X, (int)transform.position.Y+4, "playerBullet");
                bullet.Shot(Bullet.Type.PlayerBullet, -90);
                yield return WaitForSeconds(0.1f);
            }

            isShot = false;
            var lst = RenderManager.Instance.LayerList;
            for (int i = 0; i < lst.Count; i++) {
                if (lst.ContainsKey(i)) {
                    var items = lst[i];
                    var obj = GameObject.Instantiate(0, 0, "UI");
                    var sr = obj.AddComponent<SpriteRenderer>();

                    sr.SortingLayer = Layer("UI");

                    Debug.Log($"{items[0].SortingLayer.Name}  ,  {i} ,  {items[0].Name}");
                }
            }
        }
    }
}
