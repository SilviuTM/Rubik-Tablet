using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rubik_s_Tablet
{
    public static class Puzzle
    {
        struct VisualSquare
        {
            public int value;
            public Rectangle hitbox;
        }

        public static int rows = 3, columns = 3;
        static float square_size;
        static VisualSquare[,] grid;

        static MouseState curState, prevState;
        static MouseState initialState;
        static int deltaX = 0;
        static int deltaY = 0;
        static int moveSet = 0;
        static int elapsed = 0;

        static Vector2 affectedCoord;

        public static bool isDone = false;
        static Rectangle back_button;

        public static void Initialize()
        {
            back_button = new Rectangle(1400 - (GameContent.start_bg.Width / 2), 450 - (GameContent.start_bg.Height / 2), GameContent.start_bg.Width, GameContent.start_bg.Height);
        }

        public static void StartGame()
        {
            Menu.isOn = false;
            elapsed = 0;

            MersenneTwister randGen = new MersenneTwister();
            int[] values = new int[rows * columns + 1]; for (int i = 1; i <= rows * columns; i++) values[i] = 0;

            if (rows > columns) square_size = 800f / rows;
            else square_size = 800f / columns;

            int lessc = rows - columns;
            int lessr = columns - rows;

            if (lessc < 0) lessc = 0;
            if (lessr < 0) lessr = 0;

            grid = new VisualSquare[rows, columns];
            for (int ir = 0; ir < rows; ir++)
                for (int ic = 0; ic < columns; ic++)
                {
                    grid[ir, ic].hitbox = new Rectangle((int)(ic * square_size + 400 + lessc * (square_size / 2)), 
                                                        (int)(ir * square_size + 50 + lessr * (square_size / 2)), 
                                                        (int)square_size, (int)square_size);
                    int aux = randGen.Next(1, columns * rows);

                    while (values[aux] == 1)
                        aux = randGen.Next(1, columns * rows);

                    values[aux] = 1;
                    grid[ir, ic].value = aux;
                }
        }

        public static bool CheckDone()
        {
            for (int i = 0; i < rows * columns; i++)
                if (grid[i / columns, i % columns].value != i + 1)
                    return false;

            return true;
        }

        public static void Update()
        {
            if (elapsed < 7200) elapsed++;
            prevState = curState;
            curState = Mouse.GetState();

            if(!isDone)
            {
                if (curState.LeftButton == ButtonState.Pressed && prevState.LeftButton != ButtonState.Pressed)
                {
                    initialState = curState;
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < columns; j++)
                        {
                            if (grid[i, j].hitbox.Contains(curState.Position))
                                affectedCoord = new Vector2(i, j);
                        }
                }

                else if (curState.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Pressed)
                {
                    if (moveSet == 0) // none
                    {
                        deltaX = curState.X - initialState.X;
                        deltaY = curState.Y - initialState.Y;

                        if (Abs(deltaX) > 10 || Abs(deltaY) > 10)
                            if (Abs(deltaX) > Abs(deltaY))
                            {
                                deltaY = 0; moveSet = 1;
                            }
                            else
                            {
                                deltaX = 0; moveSet = 2;
                            }
                    }

                    else if (moveSet == 1) // X
                    {
                        deltaX = curState.X - initialState.X;
                    }

                    else if (moveSet == 2) // Y
                    {
                        deltaY = curState.Y - initialState.Y;
                    }

                    if (Abs(deltaX) > square_size * columns)
                        if (deltaX > 0) deltaX -= (int)(square_size * columns);
                        else deltaX += (int)(square_size * columns);

                    if (Abs(deltaY) > square_size * rows)
                        if (deltaY > 0) deltaY -= (int)(square_size * rows);
                        else deltaY += (int)(square_size * rows);
                }
                
                else if (curState.LeftButton != ButtonState.Pressed && prevState.LeftButton == ButtonState.Pressed)
                {
                    // code to change matrix
                    if (moveSet == 1) // X (scroll a set row left-right)
                    {
                        if (deltaX > 0) // from left to right
                        {
                            if (deltaX >= square_size / 2)
                            {
                                deltaX -= (int)(square_size / 2);
                                PermuteLR((int)affectedCoord.X);
                            }    

                            while (deltaX >= square_size)
                            {
                                deltaX -= (int)square_size;
                                PermuteLR((int)affectedCoord.X);
                            }
                        }
                        else // from right to left
                        {
                            deltaX = -deltaX;

                            if (deltaX >= square_size / 2)
                            {
                                deltaX -= (int)(square_size / 2);
                                PermuteRL((int)affectedCoord.X);
                            }

                            while (deltaX >= square_size)
                            {
                                deltaX -= (int)square_size;
                                PermuteRL((int)affectedCoord.X);
                            }
                        }
                    }

                    else if (moveSet == 2) // Y (scroll a set column up-down)
                    {
                        if (deltaY > 0) // from up to down
                        {
                            if (deltaY >= square_size / 2)
                            {
                                deltaY -= (int)(square_size / 2);
                                PermuteUD((int)affectedCoord.Y);
                            }

                            while (deltaY >= square_size)
                            {
                                deltaY -= (int)square_size;
                                PermuteUD((int)affectedCoord.Y);
                            }
                        }
                        else // from down to up
                        {
                            deltaY = -deltaY;

                            if (deltaY >= square_size / 2)
                            {
                                deltaY -= (int)(square_size / 2);
                                PermuteDU((int)affectedCoord.Y);
                            }

                            while (deltaY >= square_size)
                            {
                                deltaY -= (int)square_size;
                                PermuteDU((int)affectedCoord.Y);
                            }
                        }
                    }

                    // reset
                    moveSet = 0;
                    deltaX = deltaY = 0;
                }

                isDone = CheckDone();
            }

            else 
            {
                if (curState.LeftButton == ButtonState.Pressed && back_button.Contains(curState.Position))
                {
                    isDone = false;
                    Menu.isOn = true;
                }
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            for (int ir = 0; ir < rows; ir++)
                for (int ic = 0; ic < columns; ic++)
                {
                    int auxX = 0, auxY = 0;
                    if (moveSet == 1 && ir == affectedCoord.X) // X
                        auxX = deltaX;

                    if (moveSet == 2 && ic == affectedCoord.Y) // Y
                        auxY = deltaY;

                    #region TRUE GRID
                    if (grid[ir, ic].value == ir * columns + ic + 1)
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX, grid[ir, ic].hitbox.Y + auxY, grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height), 
                                                    null, Color.LightGreen, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    else
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX, grid[ir, ic].hitbox.Y + auxY, grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height),
                                                    null, Color.LightGray, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    sb.DrawString(GameContent.arial72, grid[ir, ic].value.ToString(), 
                             grid[ir, ic].hitbox.Location.ToVector2() + (grid[ir, ic].hitbox.Size.ToVector2() / 2) + new Vector2(auxX, auxY), 
                          Color.Black, 0f, GameContent.arial72.MeasureString(grid[ir, ic].value.ToString()) / 2, square_size / 200f, SpriteEffects.None, 0.8f);
                    #endregion

                    #region region above1
                    if (grid[ir, ic].value == ir * columns + ic + 1)
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX, grid[ir, ic].hitbox.Y + auxY - (int)(square_size * rows), grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height),
                                                    null, Color.LightGreen, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    else
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX, grid[ir, ic].hitbox.Y + auxY - (int)(square_size * rows), grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height),
                                                    null, Color.LightGray, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    sb.DrawString(GameContent.arial72, grid[ir, ic].value.ToString(),
                             grid[ir, ic].hitbox.Location.ToVector2() + (grid[ir, ic].hitbox.Size.ToVector2() / 2) + new Vector2(auxX, auxY) + new Vector2(0, -(int)(square_size * rows)),
                          Color.Black, 0f, GameContent.arial72.MeasureString(grid[ir, ic].value.ToString()) / 2, square_size / 200f, SpriteEffects.None, 0.8f);
                    #endregion

                    #region region below1
                    if (grid[ir, ic].value == ir * columns + ic + 1)
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX, grid[ir, ic].hitbox.Y + auxY + (int)(square_size * rows), grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height),
                                                    null, Color.LightGreen, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    else
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX, grid[ir, ic].hitbox.Y + auxY + (int)(square_size * rows), grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height),
                                                    null, Color.LightGray, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    sb.DrawString(GameContent.arial72, grid[ir, ic].value.ToString(),
                             grid[ir, ic].hitbox.Location.ToVector2() + (grid[ir, ic].hitbox.Size.ToVector2() / 2) + new Vector2(auxX, auxY) + new Vector2(0, (int)(square_size * rows)),
                          Color.Black, 0f, GameContent.arial72.MeasureString(grid[ir, ic].value.ToString()) / 2, square_size / 200f, SpriteEffects.None, 0.8f);
                    #endregion

                    #region region left1
                    if (grid[ir, ic].value == ir * columns + ic + 1)
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX - (int)(square_size * columns), grid[ir, ic].hitbox.Y + auxY, grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height),
                                                    null, Color.LightGreen, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    else
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX - (int)(square_size * columns), grid[ir, ic].hitbox.Y + auxY, grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height),
                                                    null, Color.LightGray, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    sb.DrawString(GameContent.arial72, grid[ir, ic].value.ToString(),
                             grid[ir, ic].hitbox.Location.ToVector2() + (grid[ir, ic].hitbox.Size.ToVector2() / 2) + new Vector2(auxX, auxY) + new Vector2(-(int)(square_size * columns), 0),
                          Color.Black, 0f, GameContent.arial72.MeasureString(grid[ir, ic].value.ToString()) / 2, square_size / 200f, SpriteEffects.None, 0.8f);
                    #endregion

                    #region region right1
                    if (grid[ir, ic].value == ir * columns + ic + 1)
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX + (int)(square_size * columns), grid[ir, ic].hitbox.Y + auxY, grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height),
                                                    null, Color.LightGreen, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    else
                        sb.Draw(GameContent.square, new Rectangle(grid[ir, ic].hitbox.X + auxX + (int)(square_size * columns), grid[ir, ic].hitbox.Y + auxY, grid[ir, ic].hitbox.Width, grid[ir, ic].hitbox.Height),
                                                    null, Color.LightGray, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);

                    sb.DrawString(GameContent.arial72, grid[ir, ic].value.ToString(),
                             grid[ir, ic].hitbox.Location.ToVector2() + (grid[ir, ic].hitbox.Size.ToVector2() / 2) + new Vector2(auxX, auxY) + new Vector2((int)(square_size * columns), 0),
                          Color.Black, 0f, GameContent.arial72.MeasureString(grid[ir, ic].value.ToString()) / 2, square_size / 200f, SpriteEffects.None, 0.8f);
                    #endregion
                }

            // overcover
            // above and below
            var aux = columns > rows ? columns - rows : 0;
            sb.Draw(GameContent.blank, new Rectangle(0, 0, 1600, 50 + (int)(square_size * aux / 2)), null, GameContent.bg_color, 0f, Vector2.Zero, SpriteEffects.None, 0.9999f);
            sb.Draw(GameContent.blank, new Rectangle(0, 900 - 50 - (int)(square_size * aux / 2), 1600, 900), null, GameContent.bg_color, 0f, Vector2.Zero, SpriteEffects.None, 0.9999f);

            // left and right
            aux = columns < rows ? -columns + rows : 0;
            sb.Draw(GameContent.blank, new Rectangle(0, 0, 400 + (int)(square_size * aux / 2), 900), null, GameContent.bg_color, 0f, Vector2.Zero, SpriteEffects.None, 0.9999f);
            sb.Draw(GameContent.blank, new Rectangle(1600 - 400 - (int)(square_size * aux / 2), 0, 1600, 900), null, GameContent.bg_color, 0f, Vector2.Zero, SpriteEffects.None, 0.9999f);

            // isDone
            if (!isDone)
            {
                sb.DrawString(GameContent.arial72, " Swipe from\n   left-right\n or up-down\n  to sort all\n  numbers!", new Vector2(200, 450), Color.Black * (elapsed > 3600 ? (7200 - elapsed) / 3600f : 1f), 0f,
                              GameContent.arial72.MeasureString(" Swipe from\n   left-right\n or up-down\n  to sort all\n  numbers!") / 2, 0.55f, SpriteEffects.None, 0.999999f);

                sb.DrawString(GameContent.arial72, "Sort all numbers\n       like this\n     | 1 | 2 | 3 |\n     | 4 | 5 | 6 |\n     | 7 | 8 | 9 |", new Vector2(1400, 450), 
                              Color.Black * (elapsed > 3600 ? (7200 - elapsed) / 3600f : 1f), 0f,
                              GameContent.arial72.MeasureString("Sort all numbers\n       like this\n     | 1 | 2 | 3 |\n     | 4 | 5 | 6 |\n     | 7 | 8 | 9 |") / 2,
                              0.5f, SpriteEffects.None, 0.999999f);
            }

            else
            {
                sb.DrawString(GameContent.arial72, " Good job!\n  Now try\nsomething\n   harder!", new Vector2(200, 450), Color.Black, 0f,
                              GameContent.arial72.MeasureString(" Good job!\n  Now try\nsomething\n   harder!") / 2, 0.65f, SpriteEffects.None, 0.999999f);

                // back button
                if (back_button.Contains(curState.Position))
                {
                    sb.DrawString(GameContent.arial72, "Back to menu", new Vector2(1400, 450), Color.Black, 0f, GameContent.arial72.MeasureString("Back to menu") / 2, 0.45f, SpriteEffects.None, 0.999999f);
                    sb.Draw(GameContent.start_bg, new Vector2(1400, 450), null, Color.White, 0f, GameContent.start_bg.Bounds.Size.ToVector2() / 2, 0.65f, SpriteEffects.None, 0.999998f);
                }
                else
                {
                    sb.DrawString(GameContent.arial72, "Back to menu", new Vector2(1400, 450), Color.Black, 0f, GameContent.arial72.MeasureString("Back to menu") / 2, 0.4f, SpriteEffects.None, 0.999999f);
                    sb.Draw(GameContent.start_bg, new Vector2(1400, 450), null, Color.White, 0f, GameContent.start_bg.Bounds.Size.ToVector2() / 2, 0.6f, SpriteEffects.None, 0.999998f);
                }
            }
        }

        static int Abs(int x)
        {
            return x > 0 ? x : -x;
        }

        static void PermuteRL(int row) // right to left
        {
            int aux = grid[row, 0].value;
            for (int i = 0; i < columns - 1; i++)
                grid[row, i].value = grid[row, i + 1].value;

            grid[row, columns - 1].value = aux;
        }

        static void PermuteLR(int row) // left to right
        {
            int aux = grid[row, columns - 1].value;
            for (int i = columns - 1; i > 0; i--)
                grid[row, i].value = grid[row, i - 1].value;

            grid[row, 0].value = aux;
        }

        static void PermuteDU(int col) // down to up
        {
            int aux = grid[0, col].value;
            for (int i = 0; i < rows - 1; i++)
                grid[i, col].value = grid[i + 1, col].value;

            grid[rows - 1, col].value = aux;
        }

        static void PermuteUD(int col) // up to down
        {
            int aux = grid[rows - 1, col].value;
            for (int i = rows - 1; i > 0; i--)
                grid[i, col].value = grid[i - 1, col].value;

            grid[0, col].value = aux;
        }
    }
}
