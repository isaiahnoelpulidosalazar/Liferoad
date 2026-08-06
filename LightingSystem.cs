using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Liferoad
{
    public class LightingSystem
    {
        static bool IsEnabled = false;

        public static void Enable(ContentManager Content, SpriteBatch _spriteBatch)
        {
            IsEnabled = true;
            _spriteBatch.End();

            GlobalComponents.GetGraphicsDevice().SetRenderTarget(GlobalComponents.GetLightMask());
            GlobalComponents.GetGraphicsDevice().Clear(new Color(0, 0, 0, 200));

            _spriteBatch.Begin(transformMatrix: Camera.GetCameraMatrix(), blendState: GlobalComponents.GetLightCutoutBlend());

            foreach (GameObject Object in GlobalComponents.GetGameMapManager().GetCurrentGameMap().GetObjects())
            {
                if (Object.IsPlayerNear)
                {
                    Vector2 FontRectangle = Content.Load<SpriteFont>("DefaultFont").MeasureString("[E] to interact");
                    Rectangle Bounds = new Rectangle(
                        (int)(Object.SolidBody.X + (GlobalComponents.TILE_SIZE / 2) - (FontRectangle.X / 2)),
                        Object.SolidBody.Y - (GlobalComponents.TILE_SIZE / 2),
                        (int)FontRectangle.X + 3,
                        (int)FontRectangle.Y
                    );
                    _spriteBatch.Draw(GlobalComponents.GetWhiteTexture(), Bounds, Color.White);
                }

                if (Object.GetLightLevel() > 0)
                {
                    int LightRadius = Object.GetLightLevel() * GlobalComponents.TILE_SIZE * 2;
                    Rectangle LightRect = new Rectangle(
                        (int)Object.PositionX - (LightRadius / 2) + (GlobalComponents.TILE_SIZE / 2),
                        (int)Object.PositionY - (LightRadius / 2) + (GlobalComponents.TILE_SIZE / 2),
                        LightRadius,
                        LightRadius
                    );
                    _spriteBatch.Draw(GlobalComponents.GetWhiteTexture(), LightRect, Color.White);
                }
            }

            _spriteBatch.End();
            GlobalComponents.GetGraphicsDevice().SetRenderTarget(null);
        }

        public static void Disable()
        {
            IsEnabled = false;
        }

        public static bool IsLightingSystemEnabled()
        {
            return IsEnabled;
        }
    }
}
