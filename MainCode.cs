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
            _graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
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

            MessageBG = new Texture2D(GraphicsDevice, 1, 1);
            Darkness = new Texture2D(GraphicsDevice, 1, 1);
            LightMask = new RenderTarget2D(GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);
            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            WhiteTexture.SetData(new[] { Color.White });
            MessageBG.SetData([new Color(0, 0, 0)]);
            Darkness.SetData([new Color(0, 0, 0, 150)]);

            DepthStencilStateForRead = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                ReferenceStencil = 0,
                DepthBufferEnable = false
            };
            DepthStencilStateForWrite = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false
            };
            DepthStencilBlendState = new BlendState
            {
                ColorWriteChannels = ColorWriteChannels.None
            };

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

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Inputs.MouseDown = true;
                Inputs.MousePress = true;
            }
            else
            {
                Inputs.MouseDown = false;
            }
            if (Inputs.MousePress && !Inputs.MouseDown)
            {
                Inputs.MouseUp = true;
                Inputs.MousePress = false;
            }
            else
            {
                Inputs.MouseUp = false;
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
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Inputs.InteractReset = true;
            }
            if (Inputs.InteractReset && !Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Inputs.Interact = true;
                Inputs.InteractReset = false;
            }
            else
            {
                Inputs.Interact = false;
            }

            MainGameScenarioManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, Color.Black, 0, 0);

            _spriteBatch.Begin();

            MainGameScenarioManager.Draw(Content, _spriteBatch);

            if (IsLightingSystemEnabled)
            {
                _spriteBatch.End();
                _spriteBatch.Begin();
                _spriteBatch.Draw(LightMask, Vector2.Zero, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}