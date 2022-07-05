using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rubik_s_Tablet
{
    public static class Menu
    {
        public static MouseState prevState, curState;

        public static Rectangle row_la, row_ra, column_la, column_ra;
        public static Rectangle start_button;

        public static bool somethingClicked;
        public static bool isOn;
        
        public static void Initialize()
        {
            column_la = new Rectangle(225, 300, GameContent.arrow_l.Width, GameContent.arrow_l.Height);
            column_ra = new Rectangle(525, 300, GameContent.arrow_l.Width, GameContent.arrow_l.Height);

            row_ra = new Rectangle(1600 - 225 - GameContent.arrow_l.Width, 300, GameContent.arrow_l.Width, GameContent.arrow_l.Height);
            row_la = new Rectangle(1600 - 525 - GameContent.arrow_l.Width, 300, GameContent.arrow_l.Width, GameContent.arrow_l.Height);

            start_button = new Rectangle(535, 675, GameContent.start_bg.Width, GameContent.start_bg.Height);

            prevState = curState = Mouse.GetState();
            somethingClicked = false;
            isOn = true;
        }

        public static void Update()
        {
            prevState = curState;
            curState = Mouse.GetState();
            somethingClicked = false;

            if (curState.LeftButton == ButtonState.Pressed) // random click and hold
                somethingClicked = true;

            if (curState.LeftButton == ButtonState.Pressed && // singular click
              prevState.LeftButton == ButtonState.Released)
            {
                if (column_la.Contains(curState.X, curState.Y))
                    Puzzle.columns--;
                
                if (column_ra.Contains(curState.X, curState.Y))
                    Puzzle.columns++;

                if (row_la.Contains(curState.X, curState.Y))
                    Puzzle.rows--;

                if (row_ra.Contains(curState.X, curState.Y))
                    Puzzle.rows++;

                if (start_button.Contains(curState.X, curState.Y))
                    Puzzle.StartGame();

                if (Puzzle.columns < 2) Puzzle.columns = 2;
                if (Puzzle.columns > 10) Puzzle.columns = 10;

                if (Puzzle.rows < 2) Puzzle.rows = 2;
                if (Puzzle.rows > 10) Puzzle.rows = 10;
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            /// row left arrow
            if (row_la.Contains(curState.X, curState.Y) && !somethingClicked)
                sb.Draw(GameContent.arrow_l, row_la.Location.ToVector2() + (row_la.Size.ToVector2() / 2), null, Color.White, 0f, row_la.Size.ToVector2() / 2, 1.1f, SpriteEffects.None, 0f);
            else
                sb.Draw(GameContent.arrow_l, row_la.Location.ToVector2() + (row_la.Size.ToVector2() / 2), null, Color.White, 0f, row_la.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);


            /// row right arrow
            if (row_ra.Contains(curState.X, curState.Y) && !somethingClicked)
                sb.Draw(GameContent.arrow_r, row_ra.Location.ToVector2() + (row_ra.Size.ToVector2() / 2), null, Color.White, 0f, row_ra.Size.ToVector2() / 2, 1.1f, SpriteEffects.None, 0f);
            else
                sb.Draw(GameContent.arrow_r, row_ra.Location.ToVector2() + (row_ra.Size.ToVector2() / 2), null, Color.White, 0f, row_ra.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);


            /// column left arrow
            if (column_la.Contains(curState.X, curState.Y) && !somethingClicked)
                sb.Draw(GameContent.arrow_l, column_la.Location.ToVector2() + (column_la.Size.ToVector2() / 2), null, Color.White, 0f, column_la.Size.ToVector2() / 2, 1.1f, SpriteEffects.None, 0f);
            else
                sb.Draw(GameContent.arrow_l, column_la.Location.ToVector2() + (column_la.Size.ToVector2() / 2), null, Color.White, 0f, column_la.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);


            /// column right arrow
            if (column_ra.Contains(curState.X, curState.Y) && !somethingClicked)
                sb.Draw(GameContent.arrow_r, column_ra.Location.ToVector2() + (column_ra.Size.ToVector2() / 2), null, Color.White, 0f, column_ra.Size.ToVector2() / 2, 1.1f, SpriteEffects.None, 0f);
            else
                sb.Draw(GameContent.arrow_r, column_ra.Location.ToVector2() + (column_ra.Size.ToVector2() / 2), null, Color.White, 0f, column_ra.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);


            /// start button
            if (start_button.Contains(curState.X, curState.Y) && !somethingClicked)
            {
                sb.Draw(GameContent.start_bg, new Vector2(800, 750), null, Color.White, 0f, start_button.Size.ToVector2() / 2, 1.05f, SpriteEffects.None, 0f);
                sb.DrawString(GameContent.arial72, "Play", new Vector2(800, 750), Color.Black, 0f, GameContent.arial72.MeasureString("Play") / 2, 1.05f, SpriteEffects.None, 0.1f);
            }

            else
            {
                sb.Draw(GameContent.start_bg, new Vector2(800, 750), null, Color.White, 0f, start_button.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
                sb.DrawString(GameContent.arial72, "Play", new Vector2(800, 750), Color.Black, 0f, GameContent.arial72.MeasureString("Play") / 2, 1f, SpriteEffects.None, 0.1f);
            }

            /// columns text
            sb.DrawString(GameContent.arial72, "Columns:", new Vector2(250, 150), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
            sb.DrawString(GameContent.arial72, Puzzle.columns.ToString(), new Vector2(450, 360), Color.Black, 0f, GameContent.arial72.MeasureString(Puzzle.columns.ToString()) / 2, 1f, SpriteEffects.None, 0.1f);

            /// rows text
            sb.DrawString(GameContent.arial72, "Rows:", new Vector2(1020, 150), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
            sb.DrawString(GameContent.arial72, Puzzle.rows.ToString(), new Vector2(1145, 360), Color.Black, 0f, GameContent.arial72.MeasureString(Puzzle.rows.ToString()) / 2, 1f, SpriteEffects.None, 0.1f);

            /// misc
            sb.DrawString(GameContent.arial72, "Select number of columns and rows, then start playing!", new Vector2(800, 575), Color.Black, 0f, GameContent.arial72.MeasureString("Select number of columns and rows, then start playing!") / 2, 0.55f, SpriteEffects.None, 0.1f);
            sb.DrawString(GameContent.arial20, "(Made in 2 days.)", new Vector2(1600, 900), Color.Black, 0f, GameContent.arial20.MeasureString("(Made in 2 days.)"), 0.75f, SpriteEffects.None, 0.1f);
            sb.DrawString(GameContent.arial72, "Rubik's Tablet", new Vector2(800, 0), Color.Black, 0f, new Vector2(GameContent.arial72.MeasureString("Rubik's Tablet").X / 2, 0), 0.7f, SpriteEffects.None, 0.1f);
        }
    }
}
