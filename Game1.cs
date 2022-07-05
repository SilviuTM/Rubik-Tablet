using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rubik_s_Tablet
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.ApplyChanges();

            GameContent.LoadContent(Content);

            Menu.Initialize();
            Puzzle.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CustomMCursor.Update();

            if (Menu.isOn)
                Menu.Update();
            else
                Puzzle.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(GameContent.bg_color);
            
            //////////////////////
            spriteBatch.Begin(sortMode:SpriteSortMode.FrontToBack, samplerState:SamplerState.LinearClamp);
            CustomMCursor.Draw(spriteBatch);

            if (Menu.isOn)
                Menu.Draw(spriteBatch);
            else
                Puzzle.Draw(spriteBatch);

            spriteBatch.End();
            //////////////////////

            base.Draw(gameTime);
        }
    }
}
