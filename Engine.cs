using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Liferoad
{
    public class Engine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Engine()
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
            Core.Initialize(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DirectoryInfo DirectoryInfo = new DirectoryInfo("Content\\Scenarios");

            foreach (FileInfo File in DirectoryInfo.GetFiles("*.cs"))
            {
                Assembly CompiledAssembly = Compiler.Run("Content\\Scenarios\\" + File.Name);
                Type ScenarioType = CompiledAssembly.GetTypes()
                    .FirstOrDefault(t => typeof(GameScenario).IsAssignableFrom(t)
                      && !t.IsAbstract
                      && t.IsClass);

                if (ScenarioType != null)
                {
                    Core.GetGameScenarioManager().AddScenario((GameScenario)Activator.CreateInstance(ScenarioType));
                }
            }

            Core.GetGameScenarioManager().ChangeScenario("MainMenu");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            Inputs.Update();

            Core.GetGameScenarioManager().Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, Color.Black, 0, 0);

            _spriteBatch.Begin();

            Core.GetGameScenarioManager().Draw(Content, _spriteBatch);

            if (LightingSystem.IsLightingSystemEnabled())
            {
                _spriteBatch.End();
                _spriteBatch.Begin();
                _spriteBatch.Draw(Core.GetLightMask(), Vector2.Zero, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}