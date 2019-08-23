using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public class Window : Element {
        protected Element parent;
        protected Button minimize;
        protected Button exit;
        protected int buttonSize;
        public Text Title { get; set; }
        protected Color[] surfaceColor;
        protected Color[] bgColor;
        protected Color[] tbColor;
        protected Texture2D surfaceTex;
        protected Texture2D bgTex;
        protected Texture2D titleBarTex;
        protected List<Element> elements;
        protected List<Vector2> elementPositions;
        protected int tbSize;
        protected Vector2 position;
        protected Vector2 size;
        protected bool moving;
        protected Vector2 mouseDif;
        protected Vector2 lastSize;
        protected Vector2 lastPosition;
        protected bool minimized;
        protected bool maximized;
        protected bool closed;
        protected TextOrientation orientation;
        public event ElementEvent Closed;
        public event ElementEvent Opened;
        public event ElementEvent Minimized;
        public event ElementEvent Maximized;

        public Vector2 MinimizedPosition { get; set; }

        public Element this[int i] {
            get { return elements[i]; }
            set { elements[i] = value; }
        }

        public int this[Element e] {
            get { return elements.IndexOf(e); }
        }

        public bool HasTitleBar { get; set; }
        public bool Moveable { get; set; }
        public bool Minimizeable { get; set; }
        public bool Closeable { get; set; }
        public TextOrientation Orientation {
            get {
                return orientation;
            }

            set {
                orientation = value;
                if (Title != null)
                    SetText(Title.String);
            }
        }

        public override Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
                if (Title != null)
                    SetText(Title.String);
                if (minimize != null)
                    minimize.Position = position + new Vector2(Size.X - 2 * (buttonSize + 1), 1);
                if (exit != null)
                    exit.Position = position + new Vector2(Size.X - (buttonSize + 1), 1);
                if (elements != null)
                    for (int i = 0; i < elements.Count; i++) {
                        elements[i].Position = Position + new Vector2(UI.BGOffset, (HasTitleBar ? tbSize : 0) + UI.BGOffset) + elementPositions[i];
                    }
            }
        }

        public override Vector2 Size {
            get {
                return size;
            }
            set {
                size = value;
                if (Title != null)
                    SetText(Title.String);
                if (minimize != null)
                    minimize.Position = position + new Vector2(Size.X - 2 * (buttonSize + 1), 1);
                if (exit != null)
                    exit.Position = position + new Vector2(Size.X - (buttonSize + 1), 1);
            }
        }

        public int Count { get { return elements.Count; } }

        /// <summary>
        /// Initialize a new window element
        /// </summary>
        /// <param name="parentInterface">is nullable</param>
        /// <param name="parent">is nullable</param>
        public Window(Interface parentInterface, Element parent, string title, TextOrientation titleorientation, Vector2 position, Vector2 size, bool hasTitleBar, bool moveable, bool minimizeable, bool closeable)
            : base(parentInterface != null ? parentInterface : parent != null ? parent.ParentInterface : UI.CurrentInterface, position, size, ButtonListSide.LeftRight, true, true, true) {
            if (parent != null)
                this.parent = parent;
            UI.windowCount++;
            lastSize = size;
            buttonSize = 23;
            Title = new Text(title, Vector2.Zero, Color.Black, TextOrientation.Center, 14, 0);
            elements = new List<Element>();
            elementPositions = new List<Vector2>();
            SetText(title);
            bgColor = new Color[] { UI.BGColor };
            surfaceColor = new Color[] { UI.HighlightColor };
            tbColor = new Color[] { UI.SurfaceColor };
            tbSize = 25;
            minimize = new Button(ParentInterface, "-", 14, TextOrientation.Center, Position + new Vector2(Size.X - 2 * (buttonSize + 1), 1),
                new Vector2(23, 23), ButtonListSide.TopBottom, false, true, true);
            exit = new Button(ParentInterface, "X", 14, TextOrientation.Center, Position + new Vector2(Size.X - (buttonSize + 1), 1),
                new Vector2(23, 23), new Color(200, 70, 70), new Color(200, 50, 50), new Color(150, 40, 40), ButtonListSide.TopBottom, false, true, true);
            minimize.Clicked += (sender) => { if (maximized) Minimize(); else Maximize(true); };
            exit.Clicked += (sender) => { Close(); };
            minimized = false;
            maximized = true;
            HasTitleBar = hasTitleBar;
            Moveable = moveable;
            Minimizeable = minimizeable;
            Closeable = closeable;
            if (parent != null)
                MinimizedPosition = new Vector2(5, parent.Size.Y - 30);
            else
                MinimizedPosition = new Vector2(5 + (UI.windowCount - 1) * 155, UI.ScreenSize.Y - 30);
            Close();
        }

        public virtual void AddElement(Element element) {
            elementPositions.Add(element.Position);
            element.Position = Position + new Vector2(UI.BGOffset, (HasTitleBar ? tbSize : 0) + UI.BGOffset) + element.Position;
            elements.Add(element);
            element.TransferToInterface(ParentInterface);
            if (ParentInterface.BEContains(element)) {
                element.TransferToFront();
            } else {
                element.MoveToFront();
            }
            if (closed)
                Close();
        }

        public virtual void RemoveElement(Element element) {
            elementPositions.RemoveAt(elements.IndexOf(element));
            elements[elements.IndexOf(element)].Dispose();
            elements.Remove(element);
        }

        public virtual void Clear() {
            for (int i = 0; i < elements.Count; i++) {
                elements[i].Dispose();
            }
            elementPositions.Clear();
            elements.Clear();
        }

        public virtual void SetText(string text) {
            if (Title != null) {
                if (Orientation == TextOrientation.Center) {
                    this.Title.SetText(text, TextOrientation.Center);
                    this.Title.Position = Position + new Vector2((Size.X - 2 * (buttonSize + 1)) / 2.0f, tbSize / 2.0f);
                } else if (Orientation == TextOrientation.Left) {
                    this.Title.SetText(text, TextOrientation.Left);
                    this.Title.Position = Position + new Vector2(5, -2);
                } else if (Orientation == TextOrientation.Right) {
                    this.Title.SetText(text, TextOrientation.Right);
                    this.Title.Position = Position + Size - new Vector2(5, 2);
                }
            }
        }

        public override void Dispose() {
            foreach (Element e in elements)
                e.Dispose();
            minimize.Dispose();
            exit.Dispose();

            base.Dispose();
        }

        public override void Update() {
            base.Update();

            if (DrawEnabled && EventsEnabled && Moveable) {
                Rectangle tbRect = new Rectangle(Position.ToPoint(), new Point((int)Size.X, tbSize));
                if (!moving && pressing_ && tbRect.Intersects(new Rectangle(Mouse.GetState().Position, new Point(1)))) {
                    MoveToFront();
                    moving = true;
                    mouseDif = Mouse.GetState().Position.ToVector2() - Position;
                }
                if (moving && Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    Position = Mouse.GetState().Position.ToVector2() - mouseDif;
                } else {
                    moving = false;
                }
            }

            for (int i = 0; i < elements.Count; i++) {
                if (elements[i].Position - (Position + new Vector2(UI.BGOffset, (HasTitleBar ? tbSize : 0) + UI.BGOffset)) != elementPositions[i])
                    elementPositions[i] = elements[i].Position - (Position + new Vector2(UI.BGOffset, (HasTitleBar ? tbSize : 0) + UI.BGOffset));
            }

            if ((DrawEnabled || EventsEnabled) && closed)
                Open(Position);
            if ((!DrawEnabled || !EventsEnabled) && !closed)
                Close();

            if (!Minimizeable) {
                minimize.DrawEnabled = false;
                minimize.EventsEnabled = false;
            } else if (!closed) {
                minimize.DrawEnabled = true;
                minimize.EventsEnabled = true;
            }
            if (!Closeable) {
                exit.DrawEnabled = false;
                exit.EventsEnabled = false;
            } else if (!closed) {
                exit.DrawEnabled = true;
                exit.EventsEnabled = true;
            }
        }

        public virtual void Open(Vector2 position) {
            OnOpened();
            closed = false;
            Maximize(false);
            Position = position;
            DrawEnabled = true;
            EventsEnabled = true;
            minimize.DrawEnabled = true;
            minimize.EventsEnabled = true;
            exit.DrawEnabled = true;
            exit.EventsEnabled = true;
            for (int i = 0; i < elements.Count; i++) {
                elements[i].DrawEnabled = true;
                elements[i].EventsEnabled = true;
            }
        }

        public virtual void Close() {
            OnClosed();
            closed = true;
            lastPosition = Position;
            DrawEnabled = false;
            EventsEnabled = false;
            minimize.DrawEnabled = false;
            minimize.EventsEnabled = false;
            exit.DrawEnabled = false;
            exit.EventsEnabled = false;
            for (int i = 0; i < elements.Count; i++) {
                elements[i].DrawEnabled = false;
                elements[i].EventsEnabled = false;
            }
        }

        public virtual void Minimize() {
            OnMinimized();
            if (parent != null)
                lastPosition = Position - parent.Position;
            else
                lastPosition = Position;
            lastSize = Size;
            minimized = true;
            maximized = false;
            for (int i = 0; i < elements.Count; i++) {
                elements[i].DrawEnabled = false;
                elements[i].EventsEnabled = false;
            }
            Size = new Vector2(Math.Min(150, Size.X), Size.Y);
            if (parent != null)
                Position = MinimizedPosition + parent.Position;
            else
                Position = MinimizedPosition;
            minimize.SetText("+");
        }

        public virtual void Maximize(bool setmPos) {
            OnMaximized();
            maximized = true;
            minimized = false;
            if (setmPos) {
                if (parent != null)
                    MinimizedPosition = Position - parent.Position;
                else
                    MinimizedPosition = Position;
            }
            if (parent != null)
                Position = lastPosition + parent.Position;
            else
                Position = lastPosition;
            for (int i = 0; i < elements.Count; i++) {
                elements[i].DrawEnabled = true;
                elements[i].EventsEnabled = true;
            }
            Size = lastSize;
            minimize.SetText("-");
            MoveToFront();
        }

        private void OnOpened() {
            if (Opened != null)
                Opened(this);
        }

        private void OnClosed() {
            if (Closed != null)
                Closed(this);
        }
        private void OnMinimized() {
            if (Minimized != null)
                Minimized(this);
        }
        private void OnMaximized() {
            if (Maximized != null)
                Maximized(this);
        }

        public override void MoveToFront() {
            base.MoveToFront();

            minimize.MoveToFront();
            exit.MoveToFront();
            for (int i = 0; i < elements.Count; i++) {
                elements[i].MoveToFront();
            }
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (surfaceTex == null || titleBarTex == null || bgTex == null) {
                surfaceTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                titleBarTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                bgTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                surfaceTex.SetData(surfaceColor);
                titleBarTex.SetData(tbColor);
                bgTex.SetData(bgColor);
            }

            if (!minimized) {
                sb.Draw(bgTex, new Rectangle((Position - new Vector2(UI.BGOffset)).ToPoint(), (Size + new Vector2(UI.BGOffset * 2)).ToPoint()), Color.White);
                sb.Draw(surfaceTex, new Rectangle(Position.ToPoint(), Size.ToPoint()), Color.White);
            }
            if (minimized || HasTitleBar) {
                sb.Draw(titleBarTex, new Rectangle(Position.ToPoint(), new Point((int)Size.X, tbSize)), Color.White);
                Title.Draw(sb);
            }
        }
    }

    public class GridWindow : Window {
        int xCount;
        int yCount;
        Vector2 tileSize;

        public GridWindow(Interface parentInterface, Element parent, string title, TextOrientation titleOrientation, Vector2 position, Vector2 size, int xGridCount, int yGridCount, bool hasTitleBar, bool moveable, bool minimizeable, bool closeable)
            : base(parentInterface, parent, title, titleOrientation, position, size, hasTitleBar, moveable, minimizeable, closeable) {
            xCount = xGridCount;
            yCount = yGridCount;
            tileSize = new Vector2(size.X / xCount, hasTitleBar ? (size.Y - tbSize) / yCount : size.Y / yCount);
        }

        public void AddElement(Element element, int x, int y) {
            elementPositions.Add(new Vector2(x * tileSize.X, y * tileSize.Y));
            element.Position = Position + new Vector2(UI.BGOffset, (HasTitleBar ? tbSize : 0) + UI.BGOffset) + elementPositions.Last();
            element.Size = tileSize - new Vector2(UI.BGOffset * 2);
            elements.Add(element);
            element.TransferToInterface(ParentInterface);
            if (ParentInterface.BEContains(element))
                element.TransferToFront();
            else
                element.MoveToFront();
            if (closed)
                Close();
        }
    }
}