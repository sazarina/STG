using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Debugging;

namespace STG.Engine.Component {
    /// <summary>
    /// 
    /// </summary>
    public class RuntimeManager {
        AssetManager assetManager;
        EntityManager entityManager;
        RenderManager renderManager;

        /// <summary>
        ///  EntityManagerでT型の`GameObjectManager`のインスタンスを作成する。
        /// ゲームで使用するエンティティの初期化もここで行う。
        /// </summary>
        /// <typeparam name="T">
        /// GameObjectManagerのインスタンスの型。
        /// 例: フォームデバッグ時は `DebugClient`、
        /// 通常実行時は `GameObjectManager`。</typeparam>
        public void Initialize<T>(GraphicsDevice graphicsDevice, ContentManager content) where T : GameObjectManager {
            assetManager = AssetManager.Instance;
            assetManager.Initialize(content);

            entityManager = EntityManager.Instance;
            entityManager.Initialize<T>();

            renderManager = RenderManager.Instance;
            renderManager.Initialize(graphicsDevice);

            //cameraなどのコンポーネントの追加をLateUpdateで行う
            entityManager.LateUpdate();
            Debug.Log("RuntimeManager initialize().");
        }

        public void Update(GameTime gameTime) {
            entityManager.Update(gameTime);
            entityManager.LateUpdate();
            renderManager.Update();
        }

        public void Draw() {
            renderManager.Draw();
        }
    }
}