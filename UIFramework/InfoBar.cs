using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public enum SupportedInfoBarElement {
        Button,
        Label,
        Slider,
        CheckBox,
        TextBox,
        Separator
    }

    public class InfoBar : Element {
        public Vector2 ElementSize { get; set; }

        List<Element> elements;

        Vector2 buttonSize;
        Color[] surfaceColor;
        Color[] bgColor;
        Texture2D surfaceTex;
        Texture2D bgTex;
        int fontSize;
        int ySize;

        public Element this[int i] {
            get { return elements[i]; }
            set { elements[i] = value; }
        }

        public int Count { get { return elements.Count; } }

        public InfoBar(Interface parent, int fontSize = 14)
            : base(parent, ButtonListSide.TopBottom, false, true, true) {
            elements = new List<Element>();
            ySize = 25;
            Size = new Vector2(UI.ScreenSize.X, ySize);
            Position = new Vector2(0, 0);

            surfaceColor = new Color[] { UI.SurfaceColor };
            bgColor = new Color[] { UI.BGColor };
            buttonSize = new Vector2(10, ySize);
            this.fontSize = fontSize;
        }

        protected void UpdateOrigin() {
            for (int i = 0; i < Count; i++)
                if (i != 0)
                    elements[i].Position = Position + new Vector2(elements[i - 1].Position.X + elements[i - 1].Size.X, 0);
                else
                    elements[i].Position = Position + new Vector2(0, 0);
        }

        public void AddElement(string elementText, SupportedInfoBarElement sibe = SupportedInfoBarElement.Button) {
            switch (sibe) {
                case SupportedInfoBarElement.Button:
                    elements.Add(new Button(ParentInterface, elementText, fontSize,
                        TextOrientation.Center, Vector2.Zero, new Vector2(UI.Font.MeasureString(fontSize, elementText).X + 15, buttonSize.Y), ButtonListSide.TopBottom, false, true, true));
                    break;
                case SupportedInfoBarElement.Label:
                    elements.Add(new Label(ParentInterface, elementText, 12,
                        TextOrientation.Center, Vector2.Zero, new Vector2(UI.Font.MeasureString(fontSize, elementText).X + 15, buttonSize.Y), Color.Black, UI.HighlightColor, ButtonListSide.TopBottom, true, true));
                    break;
                case SupportedInfoBarElement.Slider:
                    elements.Add(new Slider(ParentInterface, Vector2.Zero, new Vector2(100, buttonSize.Y), 1, 0.1f, -100, 100, 12, TextOrientation.Center, null, null, true, true, true));
                    break;
                case SupportedInfoBarElement.CheckBox:
                    elements.Add(new CheckBox(ParentInterface, elementText, 12, Vector2.Zero, 25, false, true, true, true, true));
                    break;
                case SupportedInfoBarElement.TextBox:
                    elements.Add(new TextBox(ParentInterface, elementText, 12, 20, Vector2.Zero, new Vector2(200, buttonSize.Y), null, null, null, null, ButtonListSide.TopBottom, true, true, true));
                    break;
                case SupportedInfoBarElement.Separator:
                    elements.Add(new Separator(ParentInterface, Vector2.Zero, new Vector2(5, buttonSize.Y), null, null, true, ButtonListSide.TopBottom, true, true, true));
                    break;
            }
            UpdateOrigin();
        }

        public void Insert(int index, string elementText, SupportedInfoBarElement sibe = SupportedInfoBarElement.Button) {
            switch (sibe) {
                case SupportedInfoBarElement.Button:
                    elements.Insert(index, new Button(ParentInterface, elementText, fontSize,
                        TextOrientation.Center, Vector2.Zero, new Vector2(UI.Font.MeasureString(fontSize, elementText).X + 15, buttonSize.Y), ButtonListSide.TopBottom, false, true, true));
                    break;
                case SupportedInfoBarElement.Label:
                    elements.Insert(index, new Label(ParentInterface, elementText, fontSize,
                        TextOrientation.Center, Vector2.Zero, new Vector2(UI.Font.MeasureString(fontSize, elementText).X + 15, buttonSize.Y), Color.Black, UI.HighlightColor, ButtonListSide.TopBottom, true, true));
                    break;
                case SupportedInfoBarElement.Slider:
                    elements.Insert(index, new Slider(ParentInterface, Vector2.Zero, new Vector2(100, buttonSize.Y), 1, 0.1f, -100, 100, 12, TextOrientation.Center, null, null, true, true, true));
                    break;
                case SupportedInfoBarElement.CheckBox:
                    elements.Insert(index, new CheckBox(ParentInterface, elementText, 12, Vector2.Zero, 25, false, true, true, true, true));
                    break;
                case SupportedInfoBarElement.TextBox:
                    elements.Insert(index, new TextBox(ParentInterface, elementText, 12, 20, Vector2.Zero, new Vector2(200, buttonSize.Y), null, null, null, null, ButtonListSide.TopBottom, true, true, true));
                    break;
                case SupportedInfoBarElement.Separator:
                    elements.Insert(index, new Separator(ParentInterface, Vector2.Zero, new Vector2(5, buttonSize.Y), null, null, true, ButtonListSide.TopBottom, true, true, true));
                    break;
            }
            UpdateOrigin();
        }

        public void RemoveAt(int index) {
            elements[index].Dispose();
            elements.RemoveAt(index);
        }

        public void Remove(Element element) {
            if (elements.Contains(element)) {
                elements[elements.IndexOf(element)].Dispose();
                elements.Remove(element);
            }
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (bgTex == null) {
                bgTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                bgTex.SetData(bgColor);
            }
            if (surfaceTex == null) {
                surfaceTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                surfaceTex.SetData(surfaceColor);
            }

            sb.Draw(bgTex, new Rectangle((Position - new Vector2(UI.BGOffset)).ToPoint(), (Size + new Vector2(UI.BGOffset * 2)).ToPoint()), Color.White);
            sb.Draw(surfaceTex, new Rectangle(Position.ToPoint(), Size.ToPoint()), Color.White);
        }
    }
}