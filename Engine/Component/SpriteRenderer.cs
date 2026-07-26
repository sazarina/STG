using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Graphics;

namespace STG.Engine.Component {
    public class SpriteRenderer : Component {
        public Texture2D? texture { get; set; } = null;
        public LayerGroup SortingLayer { 
            get {
                return sortingLayer;
            } set {
                // Unregisterは現在値(古いレイヤー)を見て削除するため、値の代入より前に呼ぶ必要がある
                RenderManager.Instance.Unregister(this);
                sortingLayer = value;
                RenderManager.Instance.Register(this);
            }
        }

        LayerGroup sortingLayer = LayerGroup.Default;

        public int SortingOrder { get; set; }

        public Rectangle Rect {
            get {
                if (texture != null) {
                    Rectangle rect = texture.Bounds;
                    rect.Location = transform.position.ToPoint();
                    return rect;
                } else {
                    return new Rectangle();
                }
            }
        }

        public SpriteRenderer() {
            OnDestroy += () => {
            RenderManager.Instance.Unregister(this);
            };
        }

        public override void Initialize() {
            base.Initialize();
        }

        public override void Update() {
            base.Update();
        }

        public void Draw() {
            if (texture != null) {
                GraphicsUltis.DrawSprite(texture, transform);
            }
        }
    }
}
