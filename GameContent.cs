using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rubik_s_Tablet
{
    public static class GameContent
    {
        public static Color bg_color;
        public static Texture2D blank;

        public static Texture2D square;
        public static Texture2D cursor_y;
        public static Texture2D cursor_n;
        public static Texture2D arrow_l;
        public static Texture2D arrow_r;
        public static Texture2D start_bg;

        public static SpriteFont arial20;
        public static SpriteFont arial72;

        public static void LoadContent(ContentManager Content)
        {
            bg_color = Color.MediumPurple;
            blank = Content.Load<Texture2D>("assets/blank");

            square = Content.Load<Texture2D>("assets/square");
            cursor_y = Content.Load<Texture2D>("assets/cursor_yes");
            cursor_n = Content.Load<Texture2D>("assets/cursor_no");
            arrow_l = Content.Load<Texture2D>("assets/arrow-left");
            arrow_r = Content.Load<Texture2D>("assets/arrow-right");
            start_bg = Content.Load<Texture2D>("assets/start-bg");

            arial20 = Content.Load<SpriteFont>("arial20");
            arial72 = Content.Load<SpriteFont>("arial72");
        }
    }
}
