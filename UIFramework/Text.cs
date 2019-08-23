using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace UIFramework {
    public class TextFont {
        SpriteFont[] sf;

        /// <summary>
        /// Font sizes from 12 to 32 are mandatory
        /// </summary>
        /// <param name="content">ContentManager object</param>
        /// <param name="dir">directory to the font with Forward Slashes where applicable</param>
        public TextFont(ContentManager content, string dir) {
            LoadFont(content, dir);
        }

        public void LoadFont(ContentManager content, string dir) {
            sf = new SpriteFont[21];
            for (int i = 0; i < sf.Length; i++) {
                sf[i] = content.Load<SpriteFont>(dir + "/font" + (i + 12));
            }
        }

        public SpriteFont GetSpritefont(int size) {
            int index = size - 12;
            return sf[index];
        }

        public Point MeasureString(int size, string text) {
            int index = size - 12;
            return sf[index].MeasureString(text).ToPoint();
        }

        public Point MeasureChar(int size, char c) {
            int index = size - 12;
            return sf[index].MeasureString(c.ToString()).ToPoint();
        }
    }

    public class Text : Element {
        string text;
        Color color;
        float rotation;
        int size;

        public Text(string text, Vector2 pos, Color color, TextOrientation orientation, int size, float rotation)
            : base(null, pos, ButtonListSide.LeftRight, false, false, false) {
            Init(text, pos, color, orientation, size, rotation);
        }
        public void Init(string text, Vector2 pos, Color color, TextOrientation orientation, int size, float rotation) {
            Orientation = orientation;
            Position = pos;
            Color = color;
            Rotation = rotation;
            TextSize = size;
            SetText(text, orientation);
        }

        public void SetText(string text, TextOrientation orientation) {
            String = text;
            Size = UI.Font.MeasureString(size, text).ToVector2();

            if (orientation == TextOrientation.Center)
                Origin = Size / 2.0f;
            else if (orientation == TextOrientation.Left)
                Origin = Vector2.Zero;
            else if (orientation == TextOrientation.Right)
                Origin = new Vector2(Size.X, 0);
        }

        public override bool PollEvents() {
            return false;
        }
        public string String {
            get { return text; }
            set { text = value; }
        }
        public Color Color {
            get { return color; }
            set { color = value; }
        }
        public float Rotation {
            get { return rotation; }
            set { rotation = value; }
        }
        public int TextSize {
            get { return size; }
            set { size = value; }
        }
        public TextOrientation Orientation {
            get;
            set;
        }
        public int Count {
            get { return text.Length; }
        }

        public override void Draw(SpriteBatch sb) {
            sb.DrawString(UI.Font.GetSpritefont(size), text, Position, color, rotation, Origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}