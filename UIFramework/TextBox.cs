using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityFramework;

namespace UIFramework {
    public class TextBox : Element {
        Texture2D surfaceTex;
        Texture2D borderTex;
        Texture2D pointerTex;
        Color[] surfaceColor;
        Color[] borderColor;
        Color[] pointerColor;
        KeyComboList keyCombos;

        Vector2 position;
        Text tbText;
        bool active;
        bool pointerShowing;
        int pointerBlinkInterval;
        int pointerBlinkCounter;
        int pointerPos;
        public event ElementEvent LeftButtonPressed;
        public event ElementEvent RightButtonPressed;
        public event ElementEvent EnterPressed;
        public event ElementEvent BackPressed;
        public event ElementEvent TextChanged;

        public override Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
                if (tbText != null)
                    SetText(tbText.String);
            }
        }

        public string Text {
            get {
                return tbText.String;
            }
            set {
                SetText(value);
                pointerPos = value.Length;
            }
        }

        public TextBox(Interface parent, string originText, int textSize, int? pointerBlinkInterval, Vector2 position, Vector2 size, Color? textColor, Color? innerColor, Color? outerColor, Color? pointerColor,
            ButtonListSide side = ButtonListSide.TopBottom, bool pollEvents = true, bool addToUI = true, bool isFrontElement = false)
            : base(parent, position, size, side, pollEvents, addToUI, isFrontElement) {
            tbText = new Text(originText, position, textColor.HasValue ? textColor.Value : Color.Black, TextOrientation.Left, textSize, 0);
            SetText(originText);
            pointerPos = tbText.String.Length > 0 ? tbText.String.Length : 0;
            this.pointerBlinkInterval = pointerBlinkInterval.HasValue ? pointerBlinkInterval.Value : 12;
            pointerBlinkCounter = 0;
            pointerShowing = false;
            active = false;
            surfaceColor = new Color[] { innerColor.HasValue ? innerColor.Value : UI.BGColor };
            borderColor = new Color[] { outerColor.HasValue ? outerColor.Value : UI.SurfaceColor };
            this.pointerColor = new Color[] { pointerColor.HasValue ? pointerColor.Value : Color.Black };

            keyCombos = new KeyComboList(30, 0);
            keyCombos.AddKeyCombo(new Keys[] { Keys.Left }, "LEFT");
            keyCombos.AddKeyCombo(new Keys[] { Keys.Right }, "RIGHT");
            keyCombos.AddKeyCombo(new Keys[] { Keys.Enter }, "ENTER");
            keyCombos.AddKeyCombo(new Keys[] { Keys.Back }, "BACK");

            Clicked += (sender) => { active = true; };
            ResetState += (sender) => { active = false; };
            LeftButtonPressed += (sender) => {
                if (pointerPos > 0)
                    pointerPos--;
                pointerShowing = true;
            };
            RightButtonPressed += (sender) => {
                if (pointerPos < tbText.String.Length)
                    pointerPos++;
                pointerShowing = true;
            };
            BackPressed += (sender) => {
                if (pointerPos > 0) {
                    SetText(tbText.String.Remove(pointerPos - 1, 1));
                    pointerPos--;
                    pointerShowing = true;
                }
            };
        }

        public void Clear() {
            SetText("");
            pointerPos = 0;
        }

        public void SetText(string text) {
            while (UI.Font.MeasureString(tbText.TextSize, text).X > Size.X - 5) {
                text = text.Remove(text.Length - 1);
                pointerPos--;
            }

            if (tbText != null) {
                string txt = tbText.String;
                tbText.SetText(text, TextOrientation.Left);
                tbText.Position = Position + new Vector2(5, -2);
                if (txt != tbText.String)
                    OnTextChanged();
            }
        }

        bool pressedOnTB;
        public override void Update() {
            base.Update();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !pressedOnTB && !IntersectingMouse()) {
                active = false;
            }

            if (pressing_)
                pressedOnTB = true;
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                pressedOnTB = false;

            if (active) {
                pointerBlinkCounter++;
                if (pointerBlinkCounter >= pointerBlinkInterval) {
                    pointerBlinkCounter = 0;
                    pointerShowing = !pointerShowing;
                }

                keyCombos.Update();
                char c;
                if (KeyList.TryConvertKeyboardInput(out c, true)) {
                    SetText(tbText.String + c);
                    pointerPos++;
                }
            } else {
                pointerShowing = false;
            }
        }

        public override bool PollEvents() {
            if (active) {
                if (keyCombos.CheckKeyComboStateWithPriorities("LEFT"))
                    OnLeftButtonPressed();
                if (keyCombos.CheckKeyComboStateWithPriorities("RIGHT"))
                    OnRightButtonPressed();
                if (keyCombos.CheckKeyComboStateWithPriorities("ENTER"))
                    OnEnterPressed();
                if (keyCombos.CheckKeyComboStateWithPriorities("BACK"))
                    OnBackPressed();
            }

            return base.PollEvents();
        }

        public void OnLeftButtonPressed() {
            if (LeftButtonPressed != null)
                LeftButtonPressed(this);
        }

        public void OnRightButtonPressed() {
            if (RightButtonPressed != null)
                RightButtonPressed(this);
        }

        public void OnEnterPressed() {
            if (EnterPressed != null)
                EnterPressed(this);
        }

        public void OnBackPressed() {
            if (BackPressed != null)
                BackPressed(this);
        }

        public void OnTextChanged() {
            if (TextChanged != null)
                TextChanged(this);
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (surfaceTex == null || borderTex == null || pointerTex == null) {
                surfaceTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                borderTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                pointerTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                surfaceTex.SetData(surfaceColor);
                borderTex.SetData(borderColor);
                pointerTex.SetData(pointerColor);
            }

            Color overlay = Color.White;
            if (disabled_)
                overlay = UI.DisabledColor;

            sb.Draw(borderTex, new Rectangle((Position - new Vector2(UI.BGOffset)).ToPoint(), (Size + new Vector2(UI.BGOffset * 2)).ToPoint()), overlay);
            sb.Draw(surfaceTex, new Rectangle(Position.ToPoint(), Size.ToPoint()), overlay);
            tbText.Draw(sb);
            string reference = tbText.String;
            if (pointerPos < tbText.String.Length)
                reference = tbText.String.Remove(pointerPos);
            if (pointerShowing)
                sb.Draw(pointerTex, new Rectangle((Position + new Vector2(UI.Font.MeasureString(tbText.TextSize, reference).X + 5, 2)).ToPoint(), new Point(UI.BGOffset, (int)Size.Y - 4)), overlay);
        }
    }
}