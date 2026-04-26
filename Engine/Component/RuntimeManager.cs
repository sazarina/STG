using Microsoft.Xna.Framework;
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

        public void Initialize<T>(GraphicsDevice graphicsDevice, ContentManager content) where T : GameObjectManager {
            InitializeInternal<T>(graphicsDevice, content);
        }

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager content) {
            InitializeInternal<GameObjectManager>(graphicsDevice, content);
        }

        void InitializeInternal<T>(GraphicsDevice graphicsDevice, ContentManager content) where T : GameObjectManager {
            assetManager = AssetManager.Instance;
            assetManager.Initialize(content);

            entityManager = EntityManager.Instance;
            entityManager.Initialize<T>();

            renderManager = RenderManager.Instance;
            renderManager.Initialize(graphicsDevice);

            Debug.Log("RuntimeManager initialize().");
        }

        public void Update(GameTime gameTime) {
            entityManager.Update(gameTime);
            renderManager.Update();
        }

        public void Draw() {
            renderManager.Draw();
        }
    }
}