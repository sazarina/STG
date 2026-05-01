using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Component;
using STG.Engine.Debugging;
using STG.Engine.Graphics;

namespace STG {
    class GameManager {
        RuntimeManager runtimeManager = new RuntimeManager();

        public GameManager() {

        }

        /// <summary>
        /// ゲームで使用するエンティティの初期化をここで行う。
        /// </summary>
        public void Initialize(GraphicsDevice graphicsDevice, ContentManager content) {
            runtimeManager.Initialize(graphicsDevice, content);

            var layers = RenderManager.Instance().Layers;
            layers["Default"] = new LayerGroup() {
                Name = "Default",
                LayerOrder = 0,
            };

            layers["Character"] = new LayerGroup() {
                Name = "Character",
                LayerOrder = 1,
            };

            layers["UI"] = new LayerGroup() {
                Name = "UI",
                LayerOrder = 2,
            };


            var player = GameObject.Instantiate<Player>(0, 200, "player");

            //GameObject.Instantiate<SortingLayerTest>(0, 0, "sortingLayerTest");
            var obj = GameObjectManager.Instance().FindWithName("player");
            Debug.Log(obj);

            Debug.Log("GameManager.Initialize() Ended");
        }

        public void Update(GameTime gameTime) {
            runtimeManager.Update(gameTime);
        }

        public void Draw() {
            runtimeManager.Draw();
        }
    }
}