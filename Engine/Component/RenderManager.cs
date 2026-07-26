using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Debugging;
using STG.Engine.Graphics;

namespace STG.Engine.Component {
    public class RenderManager {
        SpriteBatch? spriteBatch;
        internal static SpriteBatch SpriteBatch { 
            get {
                if (Instance.spriteBatch == null) { 
                    throw new NullReferenceException("SpriteBatchがnullです。RenderManager.Initialize()が呼び出されていることを確認してください。");
                }    
                return Instance.spriteBatch;
            }
        }


        GraphicsDevice? graphicsDevice = null;
        internal static GraphicsDevice GraphicsDevice {
            get {
                if (Instance.graphicsDevice == null) {
                    throw new NullReferenceException("GraphicsDeviceがnullです。RenderManager.Initialize()が呼び出されていることを確認してください。");
                }
                return Instance.graphicsDevice;
            }
        }


        Dictionary<string, LayerGroup> sortingLayers = new Dictionary<string, LayerGroup>();
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, LayerGroup> SortingLayers => Instance.sortingLayers;


        SortedDictionary<int, List<SpriteRenderer>> layerList = new SortedDictionary<int, List<SpriteRenderer>>();

        /// <summary>
        /// RenderManagerに登録されているSpriteRendererをレイヤー順で管理するリスト
        /// </summary>
        internal static SortedDictionary<int, List<SpriteRenderer>> LayerList => Instance.layerList;

        Camera? camera;
        FPSCounter fpsCounter = new FPSCounter();

        #region シングルトン
        static RenderManager? instance = null;

        RenderManager() {
            
        }

        internal static RenderManager Instance {
            get {
                if (instance == null) {
                    instance = new RenderManager();
                }

                return instance;
            }
        }

        #endregion

        public void Initialize(GraphicsDevice graphicsDevice) {
            this.graphicsDevice = graphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);

            camera = GameObject.Instantiate(0, 0, "Camera").AddComponent<Camera>();

            Debug.Log("RenderManager.Initialize()");
        }

        internal void Register(SpriteRenderer renderer) {
            //LayerListに登録されていない場合
            if (!layerList.ContainsKey(renderer.SortingLayer.LayerOrder)) {
                layerList.Add(renderer.SortingLayer.LayerOrder, new List<SpriteRenderer> { renderer });
                
            } else {
                var index = layerList[renderer.SortingLayer.LayerOrder].IndexOf(renderer);
                if (index != -1) {
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

        public void Update() {

        }

        public void Draw() {
            GraphicsDevice.Clear(Color.White);

            if (camera == null) { 
                throw new InvalidOperationException("Cameraがnullです。RenderManager.Initialize()でCameraを設定してください。");
            }

            SpriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

            fpsCounter.Draw();

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