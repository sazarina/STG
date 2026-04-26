using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Graphics;

namespace STG.Engine.Component {
    public class SpriteRenderer : Component {
        #region Texture
        public Texture2D texture { get; set; }
        public LayerGroup SortingLayer { get {
                return sortingLayer;
            } set {
                sortingLayer = value;
                RenderManager.Instance.Register(this);
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

        public Rectangle Rect {
            get {
                Rectangle rect = texture.Bounds;
                rect.Location = transform.position.ToPoint();
                return rect;
            }
        }
        #endregion
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
