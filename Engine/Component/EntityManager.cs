using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using STG.Engine.Debugging;
using STG.Engine.Helper;
using static STG.Engine.Graphics.GraphicsUltis;


namespace STG.Engine.Component {
    public class EntityManager {
        protected SpriteBatch spriteBatch;
        internal ScriptController scriptController = ScriptController.Instance();
        public ScriptController ScriptController => scriptController;

        internal GameObjectManager gameObjectManager = null;

        FPSCounter fPSCounter;

        public EntityManager(SpriteBatch spriteBatch) {
            this.spriteBatch = spriteBatch;

            fPSCounter = new FPSCounter();

            //if (!isDebug) {
            //    gameObjectManager = GameObjectManager.Instance(this);
            //} else {
            //    gameObjectManager = GameObjectManager.Instance<>(this);
            //}


            
            
        }

        public virtual void Initialize<T>()where T:GameObjectManager {
            scriptController.Initialize();

            gameObjectManager = (T)GameObjectManager.Instance<T>(scriptController);
            gameObjectManager.Initialize();

            Debug.Log("GameObjectManager Initialized");
        }

        public virtual void Update(GameTime gameTime) {
            KeyInput.OldKeyboard = KeyInput.CurrentKeyboard;
            KeyInput.CurrentKeyboard = Keyboard.GetState();

            KeyInput.OldMouseState = KeyInput.CurrentMouseState;
            KeyInput.CurrentMouseState = Mouse.GetState();

            scriptController.Update(gameTime);
            gameObjectManager.Update();
        }


        public virtual void Draw() {
            MouseState mouseState = Mouse.GetState();

            //gameObjectManager.Draw();
            scriptController.Draw();
            fPSCounter.Draw();

            DrawText($"{mouseState.X},{mouseState.Y}", mouseState.Position.ToVector2().Add(y: 15), Color.Yellow);
        }
    }
}