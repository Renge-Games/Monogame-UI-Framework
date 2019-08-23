using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public class CheckBox : Element {
        Texture2D surfaceTex;
        Texture2D checkedTex;
        Texture2D borderTex;
        Color[] surfaceColor;
        Color[] checkedColor;
        Color[] borderColor;
        Vector2 position;
        Vector2 cbSize;
        public event ElementEvent CheckedChanged;

        public Text Text { get; set; }
        public bool Checked { get; private set; }
        public bool HasBorder { get; set; }

        public override Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
                if (Text != null)
                    SetText(Text.String);
            }
        }

        public CheckBox(Interface parent, string text, int textSize, Vector2 position, float cbSize, bool isChecked, bool hasBorder, bool pollEvents, bool addToUI, bool isFrontElement)
            : base(parent, position, ButtonListSide.TopBottom, pollEvents, addToUI, isFrontElement) {
            Text = new Text(text, position, Color.Black, TextOrientation.Left, textSize, 0);
            Checked = isChecked;
            prevCheckState = isChecked;
            HasBorder = hasBorder;
            Size = new Vector2(cbSize + 10 + UI.Font.MeasureString(textSize, text).X, cbSize);
            this.cbSize = new Vector2(cbSize);
            SetText(text);

            surfaceColor = new Color[] { UI.SurfaceColor };
            borderColor = new Color[] { UI.BGColor };
            checkedColor = new Color[] { UI.HighlightColor };

            Clicked += (sender) => { Checked = !Checked; };
        }

        bool prevCheckState;
        public override bool PollEvents() {
            if (prevCheckState != Checked) {
                prevCheckState = Checked;
                OnCheckedChanged();
            }
            return base.PollEvents();
        }

        public void OnCheckedChanged() {
            if (CheckedChanged != null)
                CheckedChanged(this);
        }

        public void SetText(string text) {
            if (Text != null) {
                Text.SetText(text, TextOrientation.Left);
                Text.Position = Position + new Vector2(cbSize.X + 10, 0);
                Size = new Vector2(cbSize.X + 10 + UI.Font.MeasureString(Text.TextSize, text).X, Size.Y);
            }
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (surfaceTex == null || checkedTex == null || borderTex == null) {
                surfaceTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                checkedTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                borderTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                surfaceTex.SetData(surfaceColor);
                checkedTex.SetData(checkedColor);
                borderTex.SetData(borderColor);
            }

            Color overlay = Color.White;
            if (disabled_)
                overlay = UI.DisabledColor;

            if (HasBorder)
                sb.Draw(borderTex, new Rectangle((Position - new Vector2(UI.BGOffset)).ToPoint(), (cbSize + new Vector2(UI.BGOffset * 2)).ToPoint()), overlay);
            sb.Draw(surfaceTex, new Rectangle(Position.ToPoint(), cbSize.ToPoint()), overlay);
            if (Checked)
                sb.Draw(checkedTex, new Rectangle((Position + cbSize / 6.0f).ToPoint(), (cbSize - cbSize / 3.0f + new Vector2(1)).ToPoint()), overlay);
            Text.Draw(sb);
        }
    }
}