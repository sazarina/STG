using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using STG;
using STG.Engine;
using STG.Engine.Component;
using STG.Engine.Debugging; 
using STG.Engine.Graphics;

namespace Editor {
    public class MyGame : WpfGame {
        GameManager? gameManager;
        DebugWindowForm debugWindow = new DebugWindowForm();

        private IGraphicsDeviceService _graphicsDeviceManager;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;

        protected override void Initialize() {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            // wpf and keyboard need reference to the host control in order to receive input
            // this means every WpfGame control will have it's own keyboard & mouse manager which will only react if the mouse is in the control
            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();

            // content loading now possible
            Content.RootDirectory = "Content";

            Screen.Width = (int)Width;
            Screen.Height = (int)Height;

            LoadHelper.Initialize(Content, GraphicsDevice);
            GraphicsUltis.Initialize();

            KeyInput.Initialize();

            Debug.Log("WPFGame.initialize()");

            debugWindow.Show();

            gameManager = new GameManager();
            gameManager.Initialize<GameObjectManager>(GraphicsDevice, Content);
        }

        protected override void Update(GameTime time) {
            if (gameManager == null) {
                throw new InvalidOperationException("GameManager is not initialized.");
            }

            // WPFの入力状態をグローバルなKeyInputに写す
            KeyInput.OldKeyboard = KeyInput.CurrentKeyboard;
            KeyInput.CurrentKeyboard = _keyboard.GetState();
            KeyInput.OldMouseState = KeyInput.CurrentMouseState;
            KeyInput.CurrentMouseState = _mouse.GetState();

            gameManager.Update(time);
        }

        protected override void Draw(GameTime time) {
            if (gameManager == null) {
                throw new InvalidOperationException("GameManager is not initialized.");
            }

            gameManager.Draw();
        }
    }
}
