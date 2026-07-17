using System;
using Microsoft.Xna.Framework;
using STG.Engine.Component;
using STG.Engine.Graphics;

namespace STG {
    class Bullet : Behavior {
        public enum Type {
            PlayerBullet,
            EnemyBullet
        }

        public Type BulletType { get; private set; }

        float speed = 5f;

        int angle;

        public void Shot(Type type, int angle) { 
            BulletType = type;
            if (type == Type.PlayerBullet) {
                this.angle = angle - 90;
            }else if(type == Type.EnemyBullet) {
                //TODO:まだ敵を実装していないため、敵を作成したときに確認する必要あり
                this.angle = angle + 90;
            }
        }

        public override void Start() {
            gameObject.tag = "Bullet";
            var renderer = gameObject.AddComponent<SpriteRenderer>();
            var spritesheet = new SpriteSheet("shot", 4, 1, new Point(12, 48));

            renderer.texture = spritesheet.GetTexture(0);

            renderer.SortingLayer = Layer("Bullet");
        }

        public override void Update() {
            var position = transform.position;

            var rad = MathHelper.ToRadians(angle);

            position.X += (float)Math.Cos(rad) * speed;
            position.Y += (float)Math.Sin(rad) * speed;
            transform.position = position;
        }
    }
}
