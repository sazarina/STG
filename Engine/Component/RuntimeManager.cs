using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Debugging;

namespace STG.Engine.Component {
    /// <summary>
    /// 
    /// </summary>
    public class RuntimeManager {
            EntityManager.Instance().Initialize<T>();
            RenderManager.Instance(graphicsDevice, spriteBatch);
            Debug.Log("RuntimeManager initialize().");
        public void Initialize<T>(GraphicsDevice graphicsDevice, ContentManager content) where T : GameObjectManager {
            InitializeInternal<T>(graphicsDevice, content);
        }

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager content) {
            InitializeInternal<GameObjectManager>(graphicsDevice, content);
        }

            EntityManager.Instance().Initialize<GameObjectManager>();
            RenderManager.Instance(graphicsDevice, spriteBatch);
        void InitializeInternal<T>(GraphicsDevice graphicsDevice, ContentManager content) where T : GameObjectManager {
            Debug.Log("RuntimeManager initialize().");
        }

        public void Update(GameTime gameTime) {
            EntityManager.Instance().Update(gameTime);
            RenderManager.Instance().Update();
        }

        public void Draw() {
            RenderManager.Instance().Draw();
        }
    }
}