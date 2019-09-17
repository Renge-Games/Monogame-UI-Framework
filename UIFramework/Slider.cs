using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public class Slider : Element {
        double value;
        double changeRate;
        double minimum;
        double maximum;
        bool pressed;
        bool middlePressed;
        Vector2 prevMousePos;
        Vector2 currentMousePos;
        Vector2 startMousePos;
        int prevScroll;
        int currScroll;
        Text valueText;
        Color[] bgColor;
        Color[] fgColor;
        Texture2D fgTex;
        Texture2D bgTex;
        Vector2 position;

        public event ElementEvent ValueChanged;

        public double Value {
            get { return value; }
            set {
                this.value = Math.Round(value, 5);
                SetText(this.value.ToString());
            }
        }

        public override Vector2 Position {
            get { return position; }
            set {
                position = value;
                SetText(this.value.ToString());
            }
        }

        public Slider(Interface parent, Vector2 position, Vector2 size, double originValue, double changeRate, double min, double max, int textSize, TextOrientation orientation = TextOrientation.Center, Color? foreGroundColor = null, Color? backGroundColor = null, bool pollEvents = true, bool addToUI = true, bool isFrontElement = false)
            : base(parent, position, size, ButtonListSide.LeftRight, pollEvents, addToUI, isFrontElement) {
            fgColor = new Color[] { foreGroundColor ?? UI.SurfaceColor };
            bgColor = new Color[] { backGroundColor ?? UI.BGColor };
            value = originValue;
            this.changeRate = changeRate;
            minimum = min;
            maximum = max;
            valueText = new Text(value.ToString(), Vector2.Zero, Color.Black, TextOrientation.Center, textSize, 0);
            SetText(value.ToString());
            prevMousePos = Mouse.GetState().Position.ToVector2();
            currentMousePos = prevMousePos;
            prevScroll = Mouse.GetState().ScrollWheelValue;
            currScroll = prevScroll;
            prevValue = originValue;
        }

        double prevValue;
        public override bool PollEvents() {
            if (prevValue != Value) {
                prevValue = Value;
                OnValueChanged();
            }
            return base.PollEvents();
        }

        public void OnValueChanged() {
            ValueChanged?.Invoke(this);
        }

        public void SetText(string text) {
            if (valueText != null) {
                if (valueText.Orientation == TextOrientation.Center) {
                    valueText.SetText(text, TextOrientation.Center);
                    valueText.Position = Position + Size / 2.0f;
                } else if (valueText.Orientation == TextOrientation.Left) {
                    valueText.SetText(text, TextOrientation.Left);
                    valueText.Position = Position + new Vector2(5, -2);
                } else if (valueText.Orientation == TextOrientation.Right) {
                    valueText.SetText(text, TextOrientation.Right);
                    valueText.Position = Position + Size - new Vector2(5, 2);
                }
            }
        }

        public override void Update() {
            base.Update();
            if (pressing_)
                pressed = true;
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                pressed = false;
            if (hovering_ && Mouse.GetState().MiddleButton == ButtonState.Pressed && !middlePressed) {
                middlePressed = true;
                startMousePos = Mouse.GetState().Position.ToVector2();
            }
            if (Mouse.GetState().MiddleButton == ButtonState.Released)
                middlePressed = false;
            currScroll = Mouse.GetState().ScrollWheelValue;
            if (pressed) {
                currentMousePos = Mouse.GetState().Position.ToVector2();
                if (currentMousePos.X < prevMousePos.X)
                    Value -= changeRate;
                else if (currentMousePos.X > prevMousePos.X)
                    Value += changeRate;
                else if (currScroll < prevScroll)
                    Value -= changeRate;
                else if (currScroll > prevScroll)
                    Value += changeRate;
                prevMousePos = currentMousePos;
            } else if (middlePressed) {
                if (Mouse.GetState().Position.X < startMousePos.X)
                    Value -= changeRate;
                else if (Mouse.GetState().Position.X > startMousePos.X)
                    Value += changeRate;
            } else if (hovering_) {
                if (currScroll < prevScroll)
                    Value -= changeRate;
                else if (currScroll > prevScroll)
                    Value += changeRate;
            }
            prevScroll = currScroll;

            if (value < minimum)
                Value = minimum;
            else if (value > maximum)
                Value = maximum;
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (bgTex == null || fgTex == null) {
                bgTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                fgTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                bgTex.SetData(bgColor);
                fgTex.SetData(fgColor);
            }

            Color overlay = Color.White;
            if (disabled_)
                overlay = UI.DisabledColor;

            sb.Draw(bgTex, new Rectangle((Position - new Vector2(UI.BGOffset)).ToPoint(), (Size + new Vector2(UI.BGOffset * 2)).ToPoint()), overlay);
            sb.Draw(fgTex, new Rectangle(Position.ToPoint(), Size.ToPoint()), overlay);
            valueText.Draw(sb);
        }
    }
}