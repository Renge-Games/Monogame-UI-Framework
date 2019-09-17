using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public enum ButtonListSide {
        LeftRight,
        TopBottom,
        Indent //Later
    }

    /// <summary>
    /// Context Menu
    /// </summary>
    public class ButtonList {
        Element parent;
        List<Button> buttons;
        Color[] bgColArr;
        Color buttonListOverlay;
        Texture2D bgTex;
        Rectangle bgRect;
        Vector2 originPos;
        Vector2 actualPos;
        Vector2 buttonSize;
        int textSize;
        bool down; // determines whether the buttonlist will scroll downwards or upwards
        bool right; // determines whether the buttonlist will scroll right or left
        bool reversed; // if the direction is switched the list has to be reversed, this makes sure the list is only reversed once

        public bool Showing { get; protected set; }
        public ButtonListSide Side { get; set; }
        public Button this[int i] {
            get { return buttons[i]; }
            set { buttons[i] = value; }
        }

        public int Count {
            get { return buttons.Count; }
        }

        public ButtonList(Element parent, ButtonListSide side, Vector2 origin, Vector2 buttonSize, int textSize) {
            Side = side;
            buttons = new List<Button>();
            this.parent = parent;
            originPos = origin;
            actualPos = origin;
            this.buttonSize = buttonSize;
            this.textSize = textSize;
            down = true;
            right = true;
            reversed = false;
            Showing = false;
            bgColArr = new Color[] { Color.White };
            buttonListOverlay = UI.BGColor;
        }

        public void Add(string text) {
            buttons.Add(new Button(parent.ParentInterface, text, textSize, TextOrientation.Left, Vector2.Zero, buttonSize, ButtonListSide.LeftRight, false, true, true));
            buttons.Last().DrawEnabled = false;
            buttons.Last().EventsEnabled = false;
            buttons.Last().Hover += (sender) => { buttonListOverlay = UI.BGHoverColor; };
            buttons.Last().HoverLeft += (sender) => { buttonListOverlay = UI.BGColor; };
            buttons.Last().Pressed += (sender) => { buttonListOverlay = UI.BGPressedColor; };
            buttons.Last().Clicked += (sender) => { buttonListOverlay = UI.BGHoverColor; };
            UpdateOrigin();
        }

        public void RemoveAt(int index) {
            buttons[index].Dispose();
            buttons.RemoveAt(index);
        }

        public void Show() {
            Showing = true;
            for (int i = 0; i < Count; i++) {
                buttons[i].DrawEnabled = true;
                buttons[i].EventsEnabled = true;
            }
            UpdateOrigin();
            MoveToFront();
        }

        public void Hide() {
            Showing = false;
            for (int i = 0; i < Count; i++) {
                buttons[i].DrawEnabled = false;
                buttons[i].EventsEnabled = false;
            }
        }

        public void MoveToFront() {
            for (int i = 0; i < Count; i++) {
                parent.ParentInterface.PushToFront(buttons[i]);
            }
        }

        public bool IntersectingMouse() {
            Point p = Mouse.GetState().Position;
            for (int i = 0; i < Count; i++) {
                if (buttons[i].IntersectingMouse())
                    return true;
                if (buttons[i].DrawEnabled && (buttons[i].ClickButtonList.IntersectingMouse() || buttons[i].HoverButtonList.IntersectingMouse()))
                    return true;
            }
            return false;
        }

        public void UpdatePosition(Vector2 pos) {
            originPos = pos;
            UpdateOrigin();
        }

        protected void UpdateOrigin() {
            int xSize = (int)buttonSize.X;
            int ySize = 0;
            for (int i = 0; i < Count; i++) {
                ySize += (int)buttonSize.Y;
            }
            bgRect.Size = new Point(xSize + UI.BGOffset * 2, ySize + UI.BGOffset * 2);
            if (Side == ButtonListSide.LeftRight)
                xSize += (int)parent.Size.X;
            else
                ySize += (int)parent.Size.Y;

            if (originPos.X + xSize > UI.ScreenSize.X)
                right = false;
            else
                right = true;
            if (originPos.Y + ySize > UI.ScreenSize.Y)
                down = false;
            else
                down = true;

            actualPos = originPos;
            if (!right)
                actualPos -= new Vector2(buttonSize.X, 0);

            if (Side == ButtonListSide.LeftRight) {
                if (right)
                    actualPos += new Vector2(parent.Size.X, 0);

                if (down) {
                    if (reversed) { buttons.Reverse(); reversed = false; }
                    for (int i = 0; i < Count; i++)
                        buttons[i].Position = actualPos + new Vector2(0, i * buttonSize.Y);
                } else {
                    if (!reversed) { buttons.Reverse(); reversed = true; }
                    actualPos += new Vector2(0, parent.Size.Y);
                    for (int i = 0; i < Count; i++)
                        buttons[i].Position = actualPos - new Vector2(0, (i + 1) * buttonSize.Y);
                }
            } else {
                if (down) {
                    if (reversed) { buttons.Reverse(); reversed = false; }
                    for (int i = 0; i < Count; i++)
                        buttons[i].Position = actualPos + new Vector2(0, (i + 1) * buttonSize.Y);
                } else {
                    if (!reversed) { buttons.Reverse(); reversed = true; }
                    actualPos -= new Vector2(0, parent.Size.Y);
                    for (int i = 0; i < Count; i++)
                        buttons[i].Position = actualPos - new Vector2(0, i * buttonSize.Y);
                }
            }

            if (down && buttons.Count > 0) {
                bgRect.Location = new Point((int)buttons[0].Position.X - UI.BGOffset, (int)buttons[0].Position.Y - UI.BGOffset);
            } else if (buttons.Count > 0) {
                bgRect.Location = new Point((int)buttons.Last().Position.X - UI.BGOffset, (int)buttons.Last().Position.Y - UI.BGOffset);
            }
        }

        public void Draw(SpriteBatch sb) {
            if (bgTex == null) {
                bgTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                bgTex.SetData(bgColArr);
            }

            if (Showing && buttons.Count > 0)
                sb.Draw(bgTex, bgRect, buttonListOverlay);
        }
    }
}