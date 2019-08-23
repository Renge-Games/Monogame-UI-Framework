using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public enum TextOrientation {
        Center,
        Left,
        Right
    }

    public class Button : Element {
        Text text;
        Vector2 originTextPos;
        Color originColor;
        Color hoverColor;
        Color pressedColor;
        Color drawColor;
        Color[] btnTexArr;
        Color[] bgTexArr;
        Texture2D btnTex;
        Texture2D btnBGTex;
        Vector2 position;
        Vector2 textOffset;
        bool hasBorder;
        TextOrientation orientation;

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
                    SetText(text.String);
            }
        }

        public string Text {
            get {
                return text.String;
            }
            set { SetText(value); }
        }

        public Button(Interface parent, string text, int textSize, TextOrientation orientation, Vector2 position, Vector2 size,
            ButtonListSide side = ButtonListSide.LeftRight, bool hasBorder = true, bool addToUI = true, bool addToFront = false)
            : base(parent, side, true, addToUI, addToFront) {
            Position = position;
            Size = size;
            this.hasBorder = hasBorder;
            Init(text, textSize, orientation, UI.SurfaceColor, UI.SurfaceHoverColor, UI.SurfacePressedColor);
        }

        public Button(Interface parent, string text, int textSize, TextOrientation orientation, Vector2 position, Vector2 size, Color originColor, Color hoverColor, Color pressedColor,
            ButtonListSide side = ButtonListSide.LeftRight, bool hasBorder = true, bool addToUI = true, bool addToFront = false)
            : base(parent, side, true, addToUI, addToFront) {
            Position = position;
            Size = size;
            this.hasBorder = hasBorder;
            Init(text, textSize, orientation, originColor, hoverColor, pressedColor);
        }

        private void Init(string text, int textSize, TextOrientation orientation, Color originColor, Color hoverColor, Color pressedColor) {
            Orientation = orientation;
            this.text = new Text(text, Vector2.Zero, Color.Black, TextOrientation.Center, textSize, 0);
            textOffset = Vector2.Zero;
            originTextPos = this.text.Position;
            SetText(text);
            this.originColor = originColor;
            this.hoverColor = hoverColor;
            this.pressedColor = pressedColor;
            drawColor = originColor;
            btnTexArr = new Color[] { this.originColor };
            bgTexArr = new Color[] { new Color(btnTexArr[0].R / 2, btnTexArr[0].G / 2, btnTexArr[0].B / 2, btnTexArr[0].A) };

            Hover += (sender) => { btnTexArr[0] = this.hoverColor; bgTexArr[0] = new Color(btnTexArr[0].R / 2, btnTexArr[0].G / 2, btnTexArr[0].B / 2, btnTexArr[0].A); };
            HoverLeft += (sender) => { btnTexArr[0] = this.originColor; bgTexArr[0] = new Color(btnTexArr[0].R / 2, btnTexArr[0].G / 2, btnTexArr[0].B / 2, btnTexArr[0].A); };
            Pressed += (sender) => { btnTexArr[0] = this.pressedColor; bgTexArr[0] = new Color(btnTexArr[0].R / 2, btnTexArr[0].G / 2, btnTexArr[0].B / 2, btnTexArr[0].A); };
            Clicked += (sender) => { btnTexArr[0] = this.hoverColor; bgTexArr[0] = new Color(btnTexArr[0].R / 2, btnTexArr[0].G / 2, btnTexArr[0].B / 2, btnTexArr[0].A); };
            ResetState += (sender) => { btnTexArr[0] = this.originColor; bgTexArr[0] = new Color(btnTexArr[0].R / 2, btnTexArr[0].G / 2, btnTexArr[0].B / 2, btnTexArr[0].A); };
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

        public override void Disable() {
            base.Disable();
            text.Color = new Color(100, 100, 100);
        }

        public override void Enable() {
            base.Enable();
            text.Color = Color.Black;
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (btnTex == null || btnTexArr[0] != drawColor) {
                drawColor = btnTexArr[0];
                btnTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                btnTex.SetData(btnTexArr);
                btnBGTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                btnBGTex.SetData(bgTexArr);
            }

            Color overlay = Color.White;
            if (disabled_)
                overlay = UI.DisabledColor;

            if (hasBorder)
                sb.Draw(btnBGTex, new Rectangle((Position - new Vector2(UI.BGOffset)).ToPoint(), (Size + new Vector2(UI.BGOffset * 2)).ToPoint()), overlay);
            sb.Draw(btnTex, new Rectangle(Position.ToPoint(), Size.ToPoint()), overlay);
            text.Draw(sb);
        }
    }
}