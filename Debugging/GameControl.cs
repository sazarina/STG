using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.NET.Controls;
using STG.Engine.Graphics;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace STG.Engine.Debugging {
    public class GameControl :  MonoGameControl{
        GameManager gameManager;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.InitializeInernal will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            Editor.Content.RootDirectory = "Content";

            LoadHelper.Initialize(Editor.Content, Editor.GraphicsDevice);
            GraphicsUltis.Initialize();

            KeyInput.Initialize();

            Debug.Log("GameControl.initialize()");

            gameManager = new GameManager();
            gameManager.Initialize<DebugClient>(Editor.GraphicsDevice, Editor.spriteBatch);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// 
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            
            TimeManager.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Application.Exit();
            gameManager.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw() {
            gameManager.Draw();
        }
    }
}
