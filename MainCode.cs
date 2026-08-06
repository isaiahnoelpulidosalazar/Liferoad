using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;

namespace Liferoad
{
    public class MainCode : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public MainCode()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            GlobalComponents.Initialize(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DirectoryInfo DirectoryInfo = new DirectoryInfo("Content\\Scenarios");

            foreach (FileInfo File in DirectoryInfo.GetFiles("*.cs"))
            {
                Debug.WriteLine(File.Name);
                GlobalComponents.GetGameScenarioManager().AddScenario((GameScenario)Activator.CreateInstance(Compiler.Run("Content\\Scenarios\\" + File.Name).GetType("Liferoad." + File.Name.Split('.')[0])));
            }

            GlobalComponents.GetGameScenarioManager().ChangeScenario("Test");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            Inputs.Update();

            GlobalComponents.GetGameScenarioManager().Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, Color.Black, 0, 0);

            _spriteBatch.Begin();

            GlobalComponents.GetGameScenarioManager().Draw(Content, _spriteBatch);

            if (LightingSystem.IsLightingSystemEnabled())
            {
                _spriteBatch.End();
                _spriteBatch.Begin();
                _spriteBatch.Draw(GlobalComponents.GetLightMask(), Vector2.Zero, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}