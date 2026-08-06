using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Liferoad
{
    public class GlobalComponents
    {
        static int SCREEN_WIDTH, SCREEN_HEIGHT;
        public const int TILE_SIZE = 48, VERY_SHORT = 5, SHORT = 15, MEDIUM = 30, LONG = 75;
        
        static GraphicsDevice GraphicsDevice;
        static GameScenarioManager GameScenarioManager;
        static GameMapManager GameMapManager;

        static Texture2D MessageBackground;
        static Texture2D Darkness;
        static Texture2D WhiteTexture;
        static RenderTarget2D LightMask;

        static BlendState LightCutoutBlend = new BlendState
        {
            ColorSourceBlend = Blend.Zero,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            AlphaSourceBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };

        public static void Initialize(GraphicsDevice _graphicsDevice)
        {
            SCREEN_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            SCREEN_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            GraphicsDevice = _graphicsDevice;
            GameScenarioManager = new GameScenarioManager();
            GameMapManager = new GameMapManager();

            MessageBackground = new Texture2D(GraphicsDevice, 1, 1);
            Darkness = new Texture2D(GraphicsDevice, 1, 1);
            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            MessageBackground.SetData(new[] { Color.Black });
            Darkness.SetData([new Color(0, 0, 0, 150)]);
            WhiteTexture.SetData(new[] { Color.White });

            LightMask = new RenderTarget2D(GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);
        }

        public static int GetScreenWidth()
        {
            return SCREEN_WIDTH;
        }

        public static int GetScreenHeight()
        {
            return SCREEN_HEIGHT;
        }

        public static GraphicsDevice GetGraphicsDevice()
        {
            return GraphicsDevice;
        }

        public static GameScenarioManager GetGameScenarioManager()
        {
            return GameScenarioManager;
        }

        public static GameMapManager GetGameMapManager()
        {
            return GameMapManager;
        }

        public static Texture2D GetMessageBackground()
        {
            return MessageBackground;
        }

        public static Texture2D GetWhiteTexture()
        {
            return WhiteTexture;
        }

        public static RenderTarget2D GetLightMask()
        {
            return LightMask;
        }

        public static BlendState GetLightCutoutBlend()
        {
            return LightCutoutBlend;
        }

        public static void ChangeScenario(string Name)
        {
            GameScenarioManager.ChangeScenario(Name);
        }

        public static void ChangeMap(string Name)
        {
            GameMapManager.ChangeMap(Name);
        }

        public static void DrawMap(ContentManager Content, SpriteBatch _spriteBatch)
        {
            GameMapManager.Draw(Content, _spriteBatch);
        }

        public static GameObject GetGameObject(int Index)
        {
            return GameMapManager.GetCurrentGameMap().GetObjects()[Index];
        }

        public static void SetAllGameObjectsLightLevel(int LightLevel)
        {
            foreach (GameObject Object in GameMapManager.GetCurrentGameMap().GetObjects())
            {
                Object.SetLightLevel(LightLevel);
            }
        }

        public static void SetAllGameObjectsEvent(Action Event)
        {
            foreach (GameObject Object in GameMapManager.GetCurrentGameMap().GetObjects())
            {
                Object.SetEvent(Event);
            }
        }

        public static void SetGameObjectLightLevel(int Index, int LightLevel)
        {
            GameMapManager.GetCurrentGameMap().GetObjects()[Index].SetLightLevel(LightLevel);
        }

        public static void SetGameObjectEvent(int Index, Action Event)
        {
            GameMapManager.GetCurrentGameMap().GetObjects()[Index].SetEvent(Event);
        }
    }
}
