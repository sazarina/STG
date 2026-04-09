using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Debugging.Scripts;
using STG.Engine.Component;
using Engine.Component;
using STG.Engine.Graphics;

namespace STG.Engine.Debugging {
    class GameManager :EntityManager {
        DebugClient debugClient;

        public GameManager(SpriteBatch spriteBatch) :base(spriteBatch) {
            debugClient = new DebugClient(ScriptController);
        }

        /// <summary>
        /// 既定クラスの'Initialize'でT型の`GameObjectManager`のインスタンスを作成する。
        /// ゲームで使用するエンティティの初期化もここで行う。
        /// </summary>
        /// <typeparam name="T">
        /// GameObjectManagerのインスタンスの型。
        /// 例: フォームデバッグ時は `DebugClient`、
        /// 通常実行時は `GameObjectManager`。</typeparam>
        public override void Initialize<T>() {
            base.Initialize<T>();

            var layers = RenderManager.Instance().Layers;
            layers["Default"] = new LayerGroup() {
                Name = "Default",
                LayerOrder = 0,
            };

            layers["Chacter"] = new LayerGroup() {
                Name = "Character",
                LayerOrder = 1,
            };

            layers["UI"] = new LayerGroup() {
                Name = "UI",
                LayerOrder = 2,
            };


            GameObject player = GameObject.Instantiate<Player>(0, 0, "player");

            Debug.Log("GameManager.Initialize() Ended");
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw() {
            base.Draw();

        }
    }
}
