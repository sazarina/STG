using Microsoft.Xna.Framework;
using Engine.Component;
using Microsoft.Xna.Framework.Graphics;

namespace STG.Engine.Component {
    /// <summary>
    /// 
    /// </summary>
    public class RuntimeManager {
        public void Initialize<T>(GraphicsDevice graphicsDevice,SpriteBatch  spriteBatch) where T : GameObjectManager {
            EntityManager.Instance().Initialize<T>();
            RenderManager.Instance(graphicsDevice, spriteBatch);
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
