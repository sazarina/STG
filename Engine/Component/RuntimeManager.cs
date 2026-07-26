using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Debugging;
using System;

namespace STG.Engine.Component {
    /// <summary>
    /// 
    /// </summary>
    public class RuntimeManager {
        AssetManager? assetManager;
        EntityManager? entityManager;
        RenderManager? renderManager;

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
            if(entityManager == null || renderManager == null) {
                throw new InvalidOperationException("RuntimeManager is not initialized. Call Initialize() before Update().");
            }

            entityManager.Update(gameTime);
            entityManager.LateUpdate();
            renderManager.Update();
        }

        public void Draw() {
            if(renderManager == null) {
                throw new InvalidOperationException("RuntimeManager is not initialized. Call Initialize() before Draw().");
            }
            renderManager.Draw();
        }
    }
}