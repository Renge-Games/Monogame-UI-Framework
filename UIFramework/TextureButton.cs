using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public class TextureButton : Element {
        Color originColor;
        Color hoverColor;
        Color pressedColor;
        Color overlayColor;
        Color[] bgTexArr;
        Texture2D btnBGTex;
        bool hasBorder;

        public Texture2D Texture { get; set; }
        public Vector2 BGOffset { get; set; }
        public bool OverlayTexture { get; set; }

        public TextureButton(Interface parent, Texture2D texture, Vector2 position, Vector2 size, Vector2 bgOffset, Color? origin, Color? hover, Color? pressed, bool overlayTexture, ButtonListSide side = ButtonListSide.LeftRight, bool hasBorder = true, bool pollEvents = true, bool addToUI = true, bool isFrontElement = false)
            : base(parent, side, pollEvents, addToUI, isFrontElement) {
            BGOffset = bgOffset;
            Position = position;
            Size = size;
            this.hasBorder = hasBorder;
            OverlayTexture = overlayTexture;
            Init(texture, origin.HasValue ? origin.Value : UI.BGColor, hover.HasValue ? hover.Value : UI.BGHoverColor, pressed.HasValue ? pressed.Value : UI.BGPressedColor);
        }

        private void Init(Texture2D texture, Color originColor, Color hoverColor, Color pressedColor) {
            Texture = texture;
            this.originColor = originColor;
            this.hoverColor = hoverColor;
            this.pressedColor = pressedColor;
            overlayColor = originColor;
            bgTexArr = new Color[] { UI.BGColor };

            Hover += (sender) => { overlayColor = this.hoverColor; };
            HoverLeft += (sender) => { overlayColor = this.originColor; };
            Pressed += (sender) => { overlayColor = this.pressedColor; };
            Clicked += (sender) => { overlayColor = this.hoverColor; };
            ResetState += (sender) => { overlayColor = this.originColor; };
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (btnBGTex == null) {
                btnBGTex = new Texture2D(sb.GraphicsDevice, 1, 1);
                btnBGTex.SetData(bgTexArr);
            }

            if (disabled_)
                overlayColor = UI.DisabledColor;

            if (hasBorder)
                sb.Draw(btnBGTex, new Rectangle((Position - BGOffset).ToPoint(), (Size + BGOffset * 2).ToPoint()), overlayColor);
            sb.Draw(Texture, new Rectangle(Position.ToPoint(), Size.ToPoint()), (EventsEnabled && OverlayTexture) || disabled_ ? overlayColor : Color.White);
        }
    }
}