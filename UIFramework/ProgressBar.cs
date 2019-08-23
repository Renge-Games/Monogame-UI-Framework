using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public class ProgressBar : Element {
        Texture2D borderTex;
        Texture2D surfaceTex;
        Texture2D valueTex;
        Color[] borderColor;
        Color[] surfaceColor;
        Color[] valueColor;

        Vector2 position;
        Text valueText;
        double value;

        public bool ShowText { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double Value {
            get { return value; }
            set {
                if (value > MaxValue)
                    this.value = MaxValue;
                else if (value < MinValue)
                    this.value = MinValue;
                else
                    this.value = value;
                this.value = Math.Round(this.value, 5);
            }
        }
        public double Range { get { return Math.Abs(MinValue) + Math.Abs(MaxValue); } }

        public override Vector2 Position {
            get { return position; }
            set {
                position = value;
                if (valueText != null)
                    SetText();
            }
        }

        public Color ValueColor { get; set; }

        public ProgressBar(Interface parent, Vector2 position, Vector2 size, double min, double max, double originValue, int textSize, bool showText, Color? outer, Color? inner, Color? value, Color? text,
            ButtonListSide side = ButtonListSide.LeftRight, bool pollEvents = true, bool addToUI = true, bool isFrontElement = false)
            : base(parent, position, size, side, pollEvents, addToUI, isFrontElement) {
            MinValue = min;
            MaxValue = max;
            Value = originValue;
            ShowText = showText;

            valueText = new Text("", Vector2.Zero, text.HasValue ? text.Value : Color.Black, TextOrientation.Center, textSize, 0);
            SetText();

            borderColor = new Color[] { outer.HasValue ? outer.Value : UI.BGColor };
            surfaceColor = new Color[] { inner.HasValue ? inner.Value : UI.SurfaceColor };
            valueColor = new Color[] { value.HasValue ? value.Value : UI.BlueHighlightColor };
            ValueColor = valueColor[0];
        }

        public void SetText() {
            valueText.SetText(Value.ToString(), TextOrientation.Center);
            valueText.Position = Position + Size / 2.0f;
        }

        public override void Update() {
            base.Update();

            SetText();
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (borderTex == null || surfaceTex == null || valueTex == null || valueColor[0] != ValueColor) {
                valueColor[0] = ValueColor;
                borderTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                surfaceTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                valueTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                borderTex.SetData(borderColor);
                surfaceTex.SetData(surfaceColor);
                valueTex.SetData(valueColor);
            }

            int maxSize = (int)(Size.X - UI.BGOffset * 2);
            var vts = (Value / Range) * maxSize; // vts = Value Texture Size

            Color overlay = Color.White;
            if (disabled_)
                overlay = UI.DisabledColor;

            sb.Draw(borderTex, new Rectangle((Position - new Vector2(UI.BGOffset)).ToPoint(), (Size + new Vector2(UI.BGOffset * 2)).ToPoint()), overlay);
            sb.Draw(surfaceTex, new Rectangle(Position.ToPoint(), Size.ToPoint()), overlay);
            sb.Draw(valueTex, new Rectangle((Position + new Vector2(UI.BGOffset)).ToPoint(), (new Vector2((float)vts, Size.Y - UI.BGOffset * 2)).ToPoint()), overlay);
            if (ShowText)
                valueText.Draw(sb);
        }
    }
}