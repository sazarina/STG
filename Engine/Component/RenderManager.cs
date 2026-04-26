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

        RenderManager() {
            //    OnLayerOrderChanged += (order, group) => {
            //    //    if (layerList.ContainsKey(order)) {
            //    //        layerList[order] =;
            //    //    } else {
            //    //        layerList.Add(order, group);
            //    //    }
            //    //};
        }

        public static RenderManager Instance {
            get {
                if (self == null) {
                    self = new RenderManager();
                }

                return self;
            }
        }

        #endregion

        public void Initialize(GraphicsDevice graphicsDevice) {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(graphicsDevice);

            camera = GameObject.Instantiate(0, 0, "Camera").AddComponent<Camera>();

            Debug.Log("RenderManager.Initialize()");
        }

        internal SpriteBatch SpriteBatch { get; private set; }

        internal GraphicsDevice GraphicsDevice {
            get {
                if (graphicsDevice == null) { 
                    throw new NullReferenceException("GraphicsDeviceがnullです。RenderManager.Initialize()が呼び出されていることを確認してください。");
                }
                return graphicsDevice;
            }
            private set {
                graphicsDevice = value;
            }
        }

        GraphicsDevice graphicsDevice = null;

        public Dictionary<string, LayerGroup> Layers { get; set; } = new Dictionary<string, LayerGroup>();

        //List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        Dictionary<int, List<SpriteRenderer>> layerList = new Dictionary<int, List<SpriteRenderer>>();

        //Debug用
        public Dictionary<int, List<SpriteRenderer>> LayerList => layerList;

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
            GraphicsDevice.Clear(Color.White);
            SpriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

            foreach (var kv in layerList) {
                var sorted = kv.Value.OrderBy(x => x.SortingOrder);
                foreach (var sr in sorted) {
                    if (sr.gameObject.active) {
                        sr.Draw();
                    }
                }
            }

            SpriteBatch.End();
        }
    }
}