using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public class Separator : Element {
        Texture2D surfaceTex;
        Texture2D lineTex;
        Color[] lineColor;
        Color[] surfaceColor;
        bool down;

        public Separator(Interface parent, Vector2 position, Vector2 size, Color? surfaceColor, Color? lineColor, bool down = false, ButtonListSide side = ButtonListSide.LeftRight, bool pollEvents = true, bool addToUI = true, bool isFrontElement = true)
            : base(parent, position, size, side, pollEvents, addToUI, isFrontElement) {
            this.down = down;
            this.surfaceColor = new Color[] { surfaceColor.HasValue ? surfaceColor.Value : UI.SurfaceColor };
            this.lineColor = new Color[] { lineColor.HasValue ? lineColor.Value : UI.SurfacePressedColor };
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (surfaceTex == null || lineTex == null) {
                surfaceTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                lineTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                surfaceTex.SetData(surfaceColor);
                lineTex.SetData(lineColor);
            }

            Color overlay = Color.White;
            if (disabled_)
                overlay = UI.DisabledColor;

            sb.Draw(surfaceTex, new Rectangle(Position.ToPoint(), Size.ToPoint()), overlay);
            if (down)
                sb.Draw(lineTex, new Rectangle((new Vector2(Position.X + Size.X / 2.0f, Position.Y + 1)).ToPoint(), (new Vector2(1, Size.Y - 2)).ToPoint()), overlay);
            else
                sb.Draw(lineTex, new Rectangle((new Vector2(Position.X + 1, Position.Y + Size.Y / 2.0f)).ToPoint(), (new Vector2(Size.X - 2, 1)).ToPoint()), overlay);
        }
    }
}