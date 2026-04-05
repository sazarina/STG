using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Component;

namespace STG.Engine.Debugging {
    class GameManager :EntityManager {
        DebugClient debugClient;

        public GameManager(SpriteBatch spriteBatch) :base(spriteBatch) {
            debugClient = new DebugClient(this);
        }

        /// <summary>
        /// 既定クラスの'Initialize'でT型の`GameObjectManager`のインスタンスを作成する。
        /// ゲームで使用するエンティティの初期化もここで行う。
        /// </summary>
        /// <typeparam name="T">
        /// GameObjectManagerのインスタンスの型。
        /// 例: フォームデバッグ時は `DebugClient`、
        /// 通常実行時は `GameObjectManager`。</typeparam>
        public override void Initialize<T> ()  {
            base.Initialize<T>();
            
            GameObject battleManager = GameObject.Instantiate(0,0,"sss");
            Debug.Log("GameObject 'sss' created at (0,0)");
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw() {
            base.Draw();

        }
    }
}
