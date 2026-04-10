using Microsoft.Xna.Framework;
using Engine.Component;

namespace STG.Engine.Component {
    /// <summary>
    /// 
    /// </summary>
    public class RuntimeManager {
        public void Initialize<T>() where T : GameObjectManager {
            EntityManager.Instance().Initialize<T>();
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
