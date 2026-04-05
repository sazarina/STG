using STG.Engine.Component;

namespace STG.Engine.Graphics {
    public class Layer {

        /// <summary>
        /// レイヤー名
        /// </summary>
        public string layerName { get; set; }
        /// <summary>
        /// レイヤーの順番
        /// </summary>
        public int layerOrder { get; set; }

        public Layer(string layerName, int layerOrder) {
            this.layerName = layerName;
            this.layerOrder = layerOrder;
        }
    }

    public class LayerGroup {

        public GameObject gameObject { get; private set; }
        public void SetGameObject(GameObject gameObject) {
            this.gameObject = gameObject;
        }

        public Layer layer { get; set; }

        public string Name => layer.layerName;

        public int orderInLayer { get; set; }


        public LayerGroup(Layer layer, int orderInLayer) {
            this.layer = layer;
            this.orderInLayer = orderInLayer;
        }

        public LayerGroup(string layerName, int layerOrder, int orderInLayer) {
            layer = new Layer(layerName, layerOrder);
            this.orderInLayer = orderInLayer;
        }

        public static LayerGroup Default {
            get {
                return new LayerGroup("Default", 0, 0);
            }
        }
    }
}
