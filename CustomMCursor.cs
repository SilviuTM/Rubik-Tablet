using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rubik_s_Tablet
{
    public static class CustomMCursor
    {
        public static int cType = 0;

        public static void Update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed || Puzzle.isDone)
                cType = 1;
            else
                cType = 0;
        }

        public static void Draw(SpriteBatch sb)
        {
            if(cType == 0)
                sb.Draw(GameContent.cursor_n, Mouse.GetState().Position.ToVector2(), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            else
                sb.Draw(GameContent.cursor_y, Mouse.GetState().Position.ToVector2(), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
    }
}
