using STG.Engine.Component;
using System.Collections.Generic;
using System.Linq;

namespace STG.Engine.Graphics {
    public class LayerGroup {
        /// <summary>
        /// レイヤー名
        /// </summary>
        public string Name;

        public int LayerOrder {
            get { 
                return layerOrder;
            } 
            set { 

                layerOrder = value;
            } 
        } 

        int layerOrder;


        public int Id { get; set; }

        public LayerGroup() {

        }

        public LayerGroup(string layerName, int layerOrder, int id) {
            Name = layerName;
            LayerOrder = layerOrder;
            Id = id;
        }


        public static LayerGroup Default {
            get {
                return new LayerGroup("Default", 0,0);
            }
        }
    }
}
