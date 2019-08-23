using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {

    public class Element {
        public delegate void ElementEvent(Element sender);
        public event ElementEvent Pressed;
        public event ElementEvent RightPressed;
        public event ElementEvent Hover;
        public event ElementEvent HoverLeft;
        public event ElementEvent Clicked;
        public event ElementEvent RightClicked;
        public event ElementEvent ResetState;
        public event ElementEvent Disabled;
        public event ElementEvent Enabled;

        protected bool hovering_;
        protected bool pressing_;
        protected bool rightPressing_;
        protected bool mouseDown_;
        protected bool rightMouseDown_;
        protected bool reset_;
        protected bool eventsPolled_;
        protected bool disabled_;

        public Interface ParentInterface { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual Vector2 Size { get; set; }
        public virtual Vector2 Origin { get; set; }
        public ButtonList ClickButtonList { get; set; }
        public ButtonList HoverButtonList { get; set; }
        public bool DrawEnabled { get; set; }
        public bool EventsEnabled { get; set; }

        public Element(Interface parent, Vector2 position, Vector2 size, Vector2 origin, ButtonListSide side, bool pollEvents = true, bool addToUI = true, bool isFrontElement = false) {
            Init(parent, position, size, origin, side, pollEvents, addToUI, isFrontElement);
        }

        public Element(Interface parent, Vector2 position, Vector2 size, ButtonListSide side, bool pollEvents = true, bool addToUI = true, bool isFrontElement = false) {
            Init(parent, position, size, Vector2.Zero, side, pollEvents, addToUI, isFrontElement);
        }

        public Element(Interface parent, Vector2 position, ButtonListSide side, bool pollEvents = true, bool addToUI = true, bool isFrontElement = false) {
            Init(parent, position, new Vector2(1), Vector2.Zero, side, pollEvents, addToUI, isFrontElement);
        }

        public Element(Interface parent, ButtonListSide side, bool pollEvents = true, bool addToUI = true, bool isFrontElement = false) {
            Init(parent, Vector2.Zero, new Vector2(1), Vector2.Zero, side, pollEvents, addToUI, isFrontElement);
        }

        private void Init(Interface parent, Vector2 position, Vector2 size, Vector2 origin, ButtonListSide side, bool pollEvents, bool addToUI, bool isFrontElement) {
            if (parent != null)
                ParentInterface = parent;
            else
                ParentInterface = UI.CurrentInterface;
            Position = position;
            Size = size;
            Origin = origin;
            EventsEnabled = pollEvents;
            if (addToUI && !isFrontElement)
                UI.AddBackElement(this, ParentInterface);
            else if (addToUI)
                UI.AddFrontElement(this, ParentInterface);
            DrawEnabled = true;
            ClickButtonList = new ButtonList(this, side, position, new Vector2(200, 20), 12);
            HoverButtonList = new ButtonList(this, side, position, new Vector2(200, 20), 12);
        }

        private Vector2 newPos = Vector2.Zero;
        public virtual void Update() {
            if (!eventsPolled_ && !reset_ && (hovering_ || pressing_ || rightPressing_)) {
                OnResetState();
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && ClickButtonList.Showing && !ClickButtonList.IntersectingMouse() ||
                Mouse.GetState().RightButton == ButtonState.Pressed && ClickButtonList.Showing && !ClickButtonList.IntersectingMouse())
                HideClickButtonList();
            HideHoverButtonList();
            if (newPos != Position) {
                ClickButtonList.UpdatePosition(Position);
                HoverButtonList.UpdatePosition(Position);
            }
            newPos = Position;
            eventsPolled_ = false;
        }

        public void HideClickButtonList() {
            bool hide = true;
            if (ClickButtonList.IntersectingMouse())
                hide = false;
            for (int i = 0; i < ClickButtonList.Count; i++) {
                if (ClickButtonList[i].ClickButtonList.IntersectingMouse() ||
                    ClickButtonList[i].HoverButtonList.IntersectingMouse()) {
                    hide = false;
                    break;
                }
            }
            if (!DrawEnabled)
                hide = true;
            if (hide)
                ClickButtonList.Hide();
        }

        public void HideHoverButtonList() {
            bool hide = true;
            if (IntersectingMouse())
                hide = false;
            if (HoverButtonList.IntersectingMouse())
                hide = false;
            for (int i = 0; i < HoverButtonList.Count; i++) {
                if (HoverButtonList[i].ClickButtonList.IntersectingMouse() ||
                    HoverButtonList[i].HoverButtonList.IntersectingMouse()) {
                    hide = false;
                    break;
                }
            }
            if (!DrawEnabled)
                hide = true;
            if (hide)
                HoverButtonList.Hide();
        }

        public virtual bool PollEvents() {
            eventsPolled_ = true;
            ButtonState lbs = Mouse.GetState().LeftButton;
            ButtonState rbs = Mouse.GetState().RightButton;
            if (IntersectingMouse()) {
                if (!hovering_) {
                    OnHover();
                }
                if (lbs == ButtonState.Released) {
                    mouseDown_ = false;
                    if (pressing_ && hovering_)
                        OnClicked();
                    else if (pressing_)
                        pressing_ = false;
                }
                if (rbs == ButtonState.Released) {
                    rightMouseDown_ = false;
                    if (rightPressing_ && hovering_)
                        OnRightClicked();
                    else if (rightPressing_)
                        rightPressing_ = false;
                }
                if (!mouseDown_ && !pressing_ && lbs == ButtonState.Pressed)
                    OnPressed();
                if (!rightMouseDown_ && !rightPressing_ && rbs == ButtonState.Pressed)
                    OnRightPressed();
            } else if (hovering_) {
                OnHoverLeft();
                pressing_ = false;
                rightPressing_ = false;
            }

            if (lbs == ButtonState.Pressed)
                mouseDown_ = true;
            if (rbs == ButtonState.Pressed)
                rightMouseDown_ = true;
            reset_ = false;
            return true;
        }

        public bool IntersectingMouse() {
            Point p = Mouse.GetState().Position;
            if (DrawEnabled && EventsEnabled && p.X > Position.X && p.Y > Position.Y && p.X <= Position.X + Size.X && p.Y <= Position.Y + Size.Y)
                return true;
            return false;
        }

        public virtual void Disable() {
            OnDisabled();
        }

        public virtual void Enable() {
            OnEnabled();
        }

        public virtual void Dispose() {
            ParentInterface.Remove(this);
        }

        public virtual void MoveToFront() {
            ParentInterface.PushToFront(this);
        }

        public virtual void TransferToInterface(Interface intf) {
            UI.TransferElementToOtherInterface(this, ParentInterface, intf);
        }

        public virtual void TransferToFront() {
            ParentInterface.Transfer(this, true);
        }

        public virtual void OnPressed() {
            pressing_ = true;
            if (Pressed != null)
                Pressed(this);
        }

        public virtual void OnRightPressed() {
            rightPressing_ = true;
            if (RightPressed != null)
                RightPressed(this);
        }

        public virtual void OnHover() {
            hovering_ = true;
            HoverButtonList.Show();
            if (Hover != null)
                Hover(this);
        }

        public virtual void OnHoverLeft() {
            hovering_ = false;
            HideHoverButtonList();
            if (HoverLeft != null)
                HoverLeft(this);
        }

        public virtual void OnClicked() {
            pressing_ = false;
            ClickButtonList.Hide();
            HoverButtonList.Hide();
            if (Clicked != null)
                Clicked(this);
        }

        public virtual void OnRightClicked() {
            rightPressing_ = false;
            ClickButtonList.Show();
            HoverButtonList.Hide();
            if (RightClicked != null)
                RightClicked(this);
        }

        public virtual void OnResetState() {
            pressing_ = false;
            hovering_ = false;
            reset_ = true;
            if (ResetState != null)
                ResetState(this);
        }

        public virtual void OnDisabled() {
            EventsEnabled = false;
            disabled_ = true;
            if (Disabled != null)
                Disabled(this);
        }

        public virtual void OnEnabled() {
            EventsEnabled = true;
            disabled_ = false;
            if (Enabled != null)
                Enabled(this);
        }

        public virtual void Draw(SpriteBatch sb) {
            if (DrawEnabled) {
                ClickButtonList.Draw(sb);
                HoverButtonList.Draw(sb);
            }
        }
    }
}