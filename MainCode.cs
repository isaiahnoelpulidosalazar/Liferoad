using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using static Liferoad.GlobalComponents;

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
            SCREEN_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            SCREEN_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            MainGraphicsDevice = GraphicsDevice;
            MainGameScenarioManager = new GameScenarioManager();
            MainGameMapManager = new GameMapManager();

            Debug.WriteLine(SCREEN_WIDTH);
            Debug.WriteLine(SCREEN_HEIGHT);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DirectoryInfo DirectoryInfo = new DirectoryInfo("Content\\Maps");

            foreach (FileInfo File in DirectoryInfo.GetFiles("*.cs"))
            {
                Debug.WriteLine(File.Name);
                MainGameScenarioManager.AddScenario((GameScenario)Activator.CreateInstance(Compiler.Run("Content\\Maps\\" + File.Name).GetType("Liferoad." + File.Name.Split('.')[0])));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Inputs.Up = true;
            }
            else
            {
                Inputs.Up = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Inputs.Down = true;
            }
            else
            {
                Inputs.Down = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Inputs.Left = true;
            }
            else
            {
                Inputs.Left = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Inputs.Right = true;
            }
            else
            {
                Inputs.Right = false;
            }

            MainGameScenarioManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            MainGameScenarioManager.Draw(Content, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
