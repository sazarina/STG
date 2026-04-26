using STG.Engine.Component;
using System;

namespace STG {
    class Bullet : Behavior {
        public enum Type {
            PlayerBullet,
            EnemyBullet
        }

        public Type BulletType { get; private set; }

        float speed = 5f;

        float angle;

        public void Shot(Type type, float angle) { 
            BulletType = type;
            this.angle = angle;
        }

        public override void Start() {


            var spritesheet = new SpriteSheet("shot", 4, 1, new Point(12, 48));
        }

        public override void Update() {
            var position = transform.position;
            position.X += (float)Math.Cos(angle) * speed;
            position.Y += (float)Math.Sin(angle) * speed;
            transform.position = position;
        }
    }
}
