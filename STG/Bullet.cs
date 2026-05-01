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

        /// <summary>
        /// ラジアン
        /// </summary>
        float angle;

        public void Shot(Type type, float angle) {
            BulletType = type;
            this.angle = MathHelper.ToRadians(angle);
        }

        public override void Start() {
            var sr = AddComponent<SpriteRenderer>();

            sr.SortingLayer = Layer("Bullet");
            sr.SortingOrder = 1;

            var spritesheet = new SpriteSheet("shot", 4, 1, new Point(12, 48));
            sr.texture = spritesheet.GetTexture(1);
        }

        public override void Update() {
            speed = (float)(new Random().NextDouble() * speed);
            var position = transform.position;
            position.X += (float)Math.Cos(angle) * speed;
            position.Y += (float)Math.Sin(angle) * speed;
            transform.position = position;
        }
    }
}
