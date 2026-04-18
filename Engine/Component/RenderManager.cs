using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Debugging;
using STG.Engine.Graphics;

namespace STG.Engine.Component {
    public class RenderManager {
        Camera camera;

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

            camera = GameObject.Instantiate(0, 0, "Camera").AddComponent<Camera>();

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

        //List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        Dictionary<int, List<SpriteRenderer>> layerList = new Dictionary<int, List<SpriteRenderer>>();



        internal void Register(SpriteRenderer renderer) {
            //renderers.Add(renderer);
            //LayerListに登録されていない場合
            if (!layerList.ContainsKey(renderer.SortingLayer.LayerOrder)) {
                layerList.Add(renderer.SortingLayer.LayerOrder, new List<SpriteRenderer> { renderer });
                
            } else {
                var index = layerList[renderer.SortingLayer.LayerOrder].IndexOf(renderer);
                Debug.Log(index);

                if (layerList[renderer.SortingLayer.LayerOrder].IndexOf(renderer) != -1) {
                    Debug.Log($"RenderManager: SpriteRenderer {renderer.gameObject.name} は既にレイヤー {renderer.SortingLayer.Name} に登録されています。");
                } else {
                    layerList[renderer.SortingLayer.LayerOrder].Add(renderer);
                }
            }
        }

        internal void Unregister(SpriteRenderer renderer) {
            //renderers.Remove(renderer);
            if (layerList.ContainsKey(renderer.SortingLayer.LayerOrder)) {
                layerList[renderer.SortingLayer.LayerOrder].Remove(renderer);
                if (layerList[renderer.SortingLayer.LayerOrder].Count == 0) {
                    layerList.Remove(renderer.SortingLayer.LayerOrder);
                }
            }
        }

        public Action<int, LayerGroup> OnLayerOrderChanged;

        public void Update() {

        }

        public void Draw() {
            GraphicsDevice.Clear(Color.DarkBlue);
            SpriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

            foreach (var kv in layerList) {
                var sorted = kv.Value.OrderBy(x => x.SortingOrder);
                foreach (var sr in sorted) {
                    sr.isVisible = camera.ViewRect.Intersects(sr.Bound);

                    if (sr.gameObject.active && sr.isVisible) {
                        sr.Draw();
                    }

                }
            }

            SpriteBatch.End();
        }
    }
}