using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Component;
using STG.Engine.Debugging.Scripts;
using STG.Engine.Graphics;

namespace STG.Engine.Debugging {
    class GameManager {
        DebugClient debugClient;
        RuntimeManager runtimeManager = new RuntimeManager();

        public GameManager() {
            debugClient = new DebugClient(ScriptController.Instance());
        }

        /// <summary>
        /// 既定クラスの'Initialize'でT型の`GameObjectManager`のインスタンスを作成する。
        /// ゲームで使用するエンティティの初期化もここで行う。
        /// </summary>
        /// <typeparam name="T">
        /// GameObjectManagerのインスタンスの型。
        /// 例: フォームデバッグ時は `DebugClient`、
        /// 通常実行時は `GameObjectManager`。</typeparam>
        public void Initialize<T>(GraphicsDevice graphicsDevice, ContentManager content) where T : GameObjectManager {
            runtimeManager.Initialize<T>(graphicsDevice, content);

            var layers = RenderManager.Instance.Layers;
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

            var test = GameObject.Instantiate<SortingLayerTest>(0, 200, "test");

            Debug.Log("GameManager.Initialize() Ended");
        }

        public void Update(GameTime gameTime) {
            runtimeManager.Update(gameTime);
        }
    }
}