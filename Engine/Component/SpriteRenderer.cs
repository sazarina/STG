using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Graphics;

namespace STG.Engine.Component {
    public class SpriteRenderer : Component {
        public Texture2D texture { get; set; }

        public bool isVisible { get; internal set; } = true;

        public LayerGroup SortingLayer { get {
                return sortingLayer;
            } set {
                sortingLayer = value;
                RenderManager.Instance().Register(this);
            }
        }

        LayerGroup sortingLayer = LayerGroup.Default;

        public int SortingOrder { get; set; }


        //使用するつもりはない。
        void SetLayer(string layerName, int sortingOrder = 0) {
            SortingLayer = new LayerGroup(layerName, sortingOrder, 0);
            SortingOrder = sortingOrder;

            //GameObjectManager.UpdateLayerGroup(gameObject, SortingLayer);
        }

        /// <summary>
        /// スプライトの矩形。ワールド座標系で表される。
        /// </summary>
        public Rectangle Bound => new Rectangle() {
            X = (int)transform.position.X - texture.Width / 2,
            Y = (int)transform.position.Y - texture.Height / 2,
            Width = texture.Width,
            Height = texture.Height
        };

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
