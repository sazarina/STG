using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using STG.Engine;
using STG.Engine.Debugging;
using STG.Engine.Graphics;

namespace STG {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        GameManager gameManager;

        DebugWindowForm debugWindow = new DebugWindowForm();

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            debugWindow.Show();
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;

            _graphics.ApplyChanges();

            Screen.Width = _graphics.PreferredBackBufferWidth;
            Screen.Height = _graphics.PreferredBackBufferHeight;

            LoadHelper.Initialize(Content, GraphicsDevice);
            GraphicsUltis.Initialize();

            KeyInput.Initialize();

            Debug.Log("GameControl.initialize()");

            gameManager = new GameManager();
            

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            gameManager.Initialize(GraphicsDevice, _spriteBatch);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            TimeManager.Update(gameTime);
            gameManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            // TODO: Add your drawing code here
            gameManager.Draw();
            base.Draw(gameTime);
        }
    }
}
