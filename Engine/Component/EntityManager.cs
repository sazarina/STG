using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using STG.Engine.Debugging;
using STG.Engine.Helper;
using System;
using static STG.Engine.Graphics.GraphicsUltis;


namespace STG.Engine.Component {
    internal class EntityManager {

        static EntityManager? instance = null;

        internal static EntityManager Instance {
            get {
                if (instance == null) {
                    instance = new EntityManager();
                }
                return instance;
            }
        }

        internal ScriptController scriptController = ScriptController.Instance;
        public ScriptController ScriptController => scriptController;

        internal GameObjectManager? gameObjectManager = null;


        EntityManager() {
            
        }

        public virtual void Initialize<T>()where T:GameObjectManager {
            scriptController.Initialize();

            gameObjectManager = GameObjectManager.CreateInstance<T>();
            gameObjectManager.Initialize(scriptController);
            Debug.Log($"EntityManager.Initialize()");
        }

        public virtual void Update(GameTime gameTime) {
            if(gameObjectManager == null) {
                throw new InvalidOperationException("GameObjectManager が初期化されていません。Initialize() を先に呼び出してください。");
            }

            gameObjectManager.Update();

            scriptController.Update(gameTime);
        }

        internal void LateUpdate() {
            if (gameObjectManager == null) {
                throw new InvalidOperationException("GameObjectManager が初期化されていません。Initialize() を先に呼び出してください。");
            }

            gameObjectManager.LateUpdate();
            scriptController.LateUpdate();
        }

        public virtual void Draw() {
            MouseState mouseState = Mouse.GetState();

            DrawText($"{mouseState.X},{mouseState.Y}", mouseState.Position.ToVector2().Add(y: 15), Color.Yellow);
        }
    }
}