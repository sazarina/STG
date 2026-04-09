using System;
using System.Collections.Generic;
using System.Linq;
using STG.Engine.Component;
using STG.Engine.Debugging;
using STG.Engine.Graphics;

namespace Engine.Component {
    public class RenderManager {
        #region シングルトン
        static RenderManager self = null;

        public static RenderManager Instance() {
            if (self == null) {
                self = new RenderManager();
            }

            return self;
        }

        #endregion

        public Dictionary<string, LayerGroup> Layers { get; set; } = new Dictionary<string, LayerGroup>();

        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        Dictionary<int, List<SpriteRenderer>> layerList =new Dictionary<int, List<SpriteRenderer>>();

        RenderManager() {
            //    OnLayerOrderChanged += (order, group) => {
            //    //    if (layerList.ContainsKey(order)) {
            //    //        layerList[order] =;
            //    //    } else {
            //    //        layerList.Add(order, group);
            //    //    }
            //    //};
            Debug.Log("RenderManager.ctor()");
        }

        internal void Register(SpriteRenderer renderer) {
            renderers.Add(renderer);
            //LayerListに登録されていない場合
            if (!layerList.ContainsKey(renderer.SortingOrder)) {
                layerList.Add(renderer.SortingOrder, new List<SpriteRenderer> {
                    renderer
                });
            } else {
                layerList[renderer.SortingOrder].Add(renderer);
            }
        }

        internal void Unregister(SpriteRenderer renderer) {
            renderers.Remove(renderer);
            if(layerList.ContainsKey(renderer.SortingOrder)) {
                layerList[renderer.SortingOrder].Remove(renderer);
                if (layerList[renderer.SortingOrder].Count == 0) {
                    layerList.Remove(renderer.SortingOrder);
                }
            }
        }

        public Action<int,LayerGroup> OnLayerOrderChanged;

        public void Draw() {
            foreach (var renderer in renderers) {
                if (renderer.isActive) {
                    renderer.Draw();
                }
            }


            var groupList = renderers.GroupBy(x => x.SortingLayer.LayerOrder).OrderBy(x => x.Key).ToList();

            Debug.Log($"GroupList Count: {groupList.Count}");
            groupList.ForEach(group => {
                var sorted = group.OrderBy(x => x.SortingLayer.LayerOrder).ToList();
                sorted.ForEach(sr => {
                    var gameObject = sr.gameObject;
                    if (gameObject.active) {
                        sr.Draw();
                    }
                });
            });
        }
    }
}
