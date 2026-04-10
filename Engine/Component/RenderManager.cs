using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Component;
using STG.Engine.Debugging;
using STG.Engine.Graphics;

namespace Engine.Component {
    public class RenderManager {
        #region シングルトン
        static RenderManager self = null;

        RenderManager(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) {
            //    OnLayerOrderChanged += (order, group) => {
            //    //    if (layerList.ContainsKey(order)) {
            //    //        layerList[order] =;
            //    //    } else {
            //    //        layerList.Add(order, group);
            //    //    }
            //    //};
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
            Debug.Log("RenderManager.ctor()");
        }

        public static RenderManager Instance(GraphicsDevice graphicsDevice = null, SpriteBatch spriteBatch = null) {
            if (self == null) {
                if (graphicsDevice == null) {
                    throw new ArgumentNullException(nameof(graphicsDevice), "GraphicsDeviceはnullにできません。");
                }
                if (spriteBatch == null) {
                    throw new ArgumentNullException(nameof(spriteBatch), "SpriteBatchはnullにできません。");
                }

                self = new RenderManager(graphicsDevice, spriteBatch);
            }

            return self;
        }

        #endregion

        internal SpriteBatch SpriteBatch { get; private set; }

        internal GraphicsDevice GraphicsDevice { get; private set; }

        public Dictionary<string, LayerGroup> Layers { get; set; } = new Dictionary<string, LayerGroup>();

        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        Dictionary<int, List<SpriteRenderer>> layerList = new Dictionary<int, List<SpriteRenderer>>();



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
            if (layerList.ContainsKey(renderer.SortingOrder)) {
                layerList[renderer.SortingOrder].Remove(renderer);
                if (layerList[renderer.SortingOrder].Count == 0) {
                    layerList.Remove(renderer.SortingOrder);
                }
            }
        }

        public Action<int, LayerGroup> OnLayerOrderChanged;

        public void Update() {

        }

        public void Draw() {
            GraphicsDevice.Clear(Color.Blue);
            SpriteBatch.Begin();
            foreach (var renderer in renderers) {
                if (renderer.isActive) {
                    renderer.Draw();
                }
            }


            var groupList = renderers.GroupBy(x => x.SortingLayer.LayerOrder).OrderBy(x => x.Key).ToList();

            //Debug.Log($"GroupList Count: {groupList.Count}");
            groupList.ForEach(group => {
                var sorted = group.OrderBy(x => x.SortingLayer.LayerOrder).ToList();
                sorted.ForEach(sr => {
                    var gameObject = sr.gameObject;
                    if (gameObject.active) {
                        sr.Draw();
                    }
                });
            });
            SpriteBatch.End();
        }
    }
}