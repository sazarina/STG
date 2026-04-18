using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine.Debugging;

namespace STG.Engine.Component {
    /// <summary>
    /// 
    /// </summary>
    public class RuntimeManager {
        public void Initialize<T>(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) where T : GameObjectManager {
            EntityManager.Instance().Initialize<T>();
            RenderManager.Instance(graphicsDevice, spriteBatch);
            Debug.Log("RuntimeManager initialize().");
        }

        public void Initialize(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) {
            EntityManager.Instance().Initialize<GameObjectManager>();
            RenderManager.Instance(graphicsDevice, spriteBatch);
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