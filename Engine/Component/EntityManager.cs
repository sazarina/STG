using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using STG.Engine.Helper;
using static STG.Engine.Graphics.GraphicsUltis;


namespace STG.Engine.Component {
    public class EntityManager {
        protected SpriteBatch spriteBatch;
        internal ScriptController scriptController = new ScriptController();
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
            gameObjectManager = (T)GameObjectManager.Instance<T>(this);

            scriptController.Initialize();
            gameObjectManager.Initialize();
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

            gameObjectManager.Draw();
            scriptController.Draw();
            fPSCounter.Draw();

            DrawText($"{mouseState.X},{mouseState.Y}", mouseState.Position.ToVector2().Add(y: 15), Color.Yellow);
        }
    }
}