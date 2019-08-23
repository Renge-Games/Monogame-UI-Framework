using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public class Label : Element {
        Text text;
        Vector2 originTextPos;
        Color[] bgTexArr;
        Texture2D bgTex;
        Vector2 position;
        Vector2 textOffset;
        TextOrientation orientation;

        public Color Color { get; set; }

        public string Text { get { return text.String; } set { SetText(value); } }

        public TextOrientation Orientation {
            get {
                return orientation;
            }

            set {
                orientation = value;
                if (this.text != null)
                    SetText(text.String);
            }
        }

        public override Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
                if (this.text != null)
                    SetText(text.String);
            }
        }

        public Vector2 TextOffset {
            get { return textOffset; }
            set {
                textOffset = value;
                if (this.text != null)
                    this.text.Position = position + TextOffset + originTextPos + Size / 2.0f;
            }
        }

        public Label(Interface parent, string text, int textSize, TextOrientation orientation, Vector2 position, Vector2 size, Color textColor, Color bgColor, ButtonListSide side = ButtonListSide.LeftRight, bool addToUI = true, bool addToFront = false)
            : base(parent, side, true, addToUI, addToFront) {
            Orientation = orientation;
            Color = bgColor;
            Position = position;
            Size = size;
            Init(text, textSize, textColor);
        }

        private void Init(string text, int textSize, Color textColor) {
            this.text = new Text(text, Vector2.Zero, Color.Black, TextOrientation.Center, textSize, 0);
            textOffset = Vector2.Zero;
            originTextPos = this.text.Position;
            this.text.Color = textColor;
            SetText(text);
            bgTexArr = new Color[] { Color.White };
        }

        public void SetText(string text) {
            if (this.text != null) {
                if (Orientation == TextOrientation.Center) {
                    this.text.SetText(text, TextOrientation.Center);
                    this.text.Position = Position + TextOffset + originTextPos + Size / 2.0f;
                } else if (Orientation == TextOrientation.Left) {
                    this.text.SetText(text, TextOrientation.Left);
                    this.text.Position = Position + TextOffset + originTextPos + new Vector2(5, -2);
                } else if (Orientation == TextOrientation.Right) {
                    this.text.SetText(text, TextOrientation.Right);
                    this.text.Position = Position + TextOffset + originTextPos + Size - new Vector2(5, 2);
                }
            }
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (bgTex == null) {
                bgTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                bgTex.SetData(bgTexArr);
            }

            sb.Draw(bgTex, new Rectangle(Position.ToPoint(), Size.ToPoint()), Color);
            text.Draw(sb);
        }
    }
}