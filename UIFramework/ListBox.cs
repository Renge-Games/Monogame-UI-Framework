using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public class ListBox : Element {
        Texture2D surfaceTex;
        Texture2D borderTex;
        Color[] surfaceColor;
        Color[] borderColor;
        Color selectColor;
        Color textColor;
        List<Label> items;
        Dictionary<Label, bool> selected;
        Vector2 position;
        TextOrientation orientation;
        int textSize;
        int singleHeight;
        bool front;
        public event ElementEvent ItemCountChanged;
        public event ElementEvent SelectedItemCountChanged;

        public List<string> SelectedItems {
            get {
                List<string> sitems = new List<string>();
                for (int i = 0; i < items.Count; i++) {
                    if (selected[items[i]])
                        sitems.Add(items[i].Text);
                }
                return sitems;
            }
        }

        public string this[int i] {
            get { return items[i].Text; }
            set { items[i].Text = value; }
        }

        public int Count { get { return items.Count; } }

        public TextOrientation Orientation {
            get {
                return orientation;
            }

            set {
                orientation = value;
                ReallignItems();
            }
        }
        public override Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
                ReallignItems();
            }
        }

        public ListBox(Interface parent, int textSize, TextOrientation to, Vector2 position, Vector2 size, Color? textColor, Color? innerColor, Color? outerColor, Color? selectColor,
            ButtonListSide side = ButtonListSide.TopBottom, bool pollEvents = true, bool addToUI = true, bool isFrontElement = false)
            : base(parent, position, size, side, pollEvents, addToUI, isFrontElement) {
            front = isFrontElement;
            this.textSize = textSize;
            orientation = to;
            items = new List<Label>();
            selected = new Dictionary<Label, bool>();
            this.selectColor = selectColor.HasValue ? selectColor.Value : UI.BlueHighlightColor;
            this.textColor = textColor.HasValue ? textColor.Value : Color.Black;
            surfaceColor = new Color[] { innerColor.HasValue ? innerColor.Value : UI.BGColor };
            borderColor = new Color[] { outerColor.HasValue ? outerColor.Value : UI.SurfaceColor };
            singleHeight = UI.Font.MeasureString(textSize, "Log").Y + 2;
        }

        public void SetText(int index, string text) {
            items[index].SetText(text);
        }

        public void AddItem(string text) {
            items.Add(new Label(ParentInterface, text, textSize, orientation, Vector2.Zero, new Vector2(Size.X, singleHeight), textColor, surfaceColor[0], ButtonListSide.LeftRight, true, front));
            items.Last().Clicked += (sender) => {
                selected[sender as Label] = !selected[sender as Label];
                if (selected[sender as Label])
                    (sender as Label).Color = selectColor;
                else
                    (sender as Label).Color = surfaceColor[0];
                OnSelectedItemCountChanged();
            };
            selected.Add(items.Last(), false);
            ReallignItems();
            OnItemCountChanged();
        }

        public void InsertItem(int index, string text) {
            items.Insert(index, new Label(ParentInterface, text, textSize, orientation, Vector2.Zero, new Vector2(Size.X, singleHeight), textColor, surfaceColor[0], ButtonListSide.LeftRight, true, front));
            items.Last().Clicked += (sender) => {
                selected[sender as Label] = !selected[sender as Label];
                if (selected[sender as Label])
                    (sender as Label).Color = selectColor;
                else
                    (sender as Label).Color = surfaceColor[0];
                OnSelectedItemCountChanged();
            };
            selected.Add(items[index], false);
            ReallignItems();
            OnItemCountChanged();
        }

        public void Remove(int index) {
            selected.Remove(items[index]);
            items[index].Dispose();
            items.RemoveAt(index);
            ReallignItems();
            OnItemCountChanged();
        }

        public void RemoveSelectedItems() {
            bool itemRemoved = false;
            while (SelectedItems.Count > 0)
                for (int i = 0; i < items.Count; i++) {
                    if (selected[items[i]]) {
                        Remove(i);
                        itemRemoved = true;
                    }
                }

            if (itemRemoved)
                OnItemCountChanged();
        }

        public void Clear() {
            for (int i = 0; i < items.Count; i++)
                items[i].Dispose();
            items.Clear();
            selected.Clear();
            OnItemCountChanged();
        }

        public void ReallignItems() {
            if (items != null && items.Count > 0) {
                items[0].Position = Position;
                for (int i = 1; i < items.Count; i++) {
                    items[i].Position = new Vector2(Position.X, items[i - 1].Position.Y + items[i - 1].Size.Y);
                }
            }
        }

        public override void MoveToFront() {
            base.MoveToFront();

            for (int i = 0; i < items.Count; i++) {
                items[i].MoveToFront();
            }
        }

        public override void TransferToInterface(Interface intf) {
            base.TransferToInterface(intf);

            for (int i = 0; i < items.Count; i++) {
                items[i].TransferToInterface(intf);
            }
        }

        public override void TransferToFront() {
            base.TransferToFront();

            for (int i = 0; i < items.Count; i++) {
                items[i].TransferToFront();
            }
            front = true;
        }

        public override void Disable() {
            base.Disable();

            for (int i = 0; i < items.Count; i++) {
                items[i].Disable();
            }
        }

        public override void Dispose() {
            base.Dispose();
            for (int i = 0; i < items.Count; i++)
                items[i].Dispose();
        }

        public override void Enable() {
            base.Enable();

            for (int i = 0; i < items.Count; i++) {
                items[i].Enable();
            }
        }

        public void OnItemCountChanged() {
            if (ItemCountChanged != null)
                ItemCountChanged(this);
        }

        public void OnSelectedItemCountChanged() {
            if (SelectedItemCountChanged != null)
                SelectedItemCountChanged(this);
        }

        public override void Update() {
            base.Update();

            if (disabled_)
                Disable();
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (surfaceTex == null || borderTex == null) {
                surfaceTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                borderTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                surfaceTex.SetData(surfaceColor);
                borderTex.SetData(borderColor);
            }

            Color overlay = Color.White;
            if (disabled_)
                overlay = UI.DisabledColor;

            sb.Draw(borderTex, new Rectangle((Position - new Vector2(UI.BGOffset)).ToPoint(), (Size + new Vector2(UI.BGOffset * 2)).ToPoint()), overlay);
            sb.Draw(surfaceTex, new Rectangle(Position.ToPoint(), Size.ToPoint()), overlay);
        }
    }
}