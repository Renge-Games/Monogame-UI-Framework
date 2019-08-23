using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using UIFramework;

namespace FrameworkTester {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int width;
        int height;
        Label tbInfo;
        TextBox tb;
        Button addButton;
        Button remButton;
        ListBox lb;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = width = 1280;
            graphics.PreferredBackBufferHeight = height = 720;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            UI.Init(new Vector2(width, height), new TextFont(Content, "Font/"));
            UI.AddInterface("Interface1");
            tbInfo = new Label(null, "Item Name", 14, TextOrientation.Left, new Vector2(100, 75), new Vector2(250, 25), Color.Black, Color.Transparent);
            tb = new TextBox(null, "", 14, 30, new Vector2(100, 100), new Vector2(250, 25), null, null, null, null);
            addButton = new Button(null, "Add Item", 14, TextOrientation.Left, new Vector2(100, 150), new Vector2(100, 25));
            remButton = new Button(null, "Remove Item", 14, TextOrientation.Left, new Vector2(225, 150), new Vector2(125, 25));
            lb = new ListBox(null, 14, TextOrientation.Left, new Vector2(100, 200), new Vector2(250, 500), null, null, null, null);
            Window window = new Window(null, null, "Stuff", TextOrientation.Left, Vector2.Zero, new Vector2(200), true, true, true, true);
            window.Open(new Vector2(100, 100));

            addButton.Disable();
            remButton.Disable();
            lb.SelectedItemCountChanged += (sender) => { if (lb.Count > 0 && lb.SelectedItems.Count > 0) remButton.Enable(); else remButton.Disable(); };
            tb.TextChanged += (sender) => { if (tb.Text != "") addButton.Enable(); else addButton.Disable(); };
            tb.EnterPressed += (sender) => { if (tb.Text != "") { lb.AddItem(tb.Text); tb.Clear(); } };

            lb.TransferToFront();

            addButton.Clicked += (sender) => { lb.AddItem(tb.Text); tb.Clear(); };
            remButton.Clicked += (sender) => { lb.RemoveSelectedItems(); };
        }

        protected override void UnloadContent() {
            Content.Unload();
        }

        float counter;
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            counter++;
            if (counter > 100000000)
                counter = 0;

            UI.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(200, 200, 255));

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null, null, null);
            UI.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}