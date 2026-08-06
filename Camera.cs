using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Liferoad
{
    public class Camera
    {
        static int ShakeCounter = 0;
        static List<TimedShake> TimedShakes = new List<TimedShake>();
        static Vector2 ShakeOffset = Vector2.Zero;
        static Vector3 CameraPosition = new Vector3(0, 0, 0);
        static Matrix CameraMatrix => Matrix.CreateTranslation(CameraPosition + new Vector3(ShakeOffset, 0));

        class TimedShake
        {
            public bool IsActive { get; set; }
            int TimedShakeCounter = 0, Duration;

            public TimedShake(int Duration)
            {
                this.Duration = Duration;
                IsActive = true;
            }

            public void Run(SpriteBatch _spriteBatch)
            {
                if (TimedShakeCounter < Duration)
                {
                    ShakeLogic(_spriteBatch);
                    TimedShakeCounter++;
                }
                else
                {
                    IsActive = false;
                }
            }
        }

        public static Matrix GetCameraMatrix()
        {
            return CameraMatrix;
        }

        static void ShakeLogic(SpriteBatch _spriteBatch)
        {
            ShakeOffset = ShakeCounter switch
            {
                0 => new Vector2(2, -2),
                1 => new Vector2(2, 2),
                2 => new Vector2(-2, 2),
                3 => new Vector2(2, -2),
                _ => new Vector2(-2, -2)
            };
            ShakeCounter = (ShakeCounter + 1) % 5;
        }

        static void RenderShake(SpriteBatch _spriteBatch)
        {
            if (TimedShakes.Count > 0)
            {
                if (TimedShakes[0].IsActive)
                {
                    TimedShakes[0].Run(_spriteBatch);
                }
                else
                {
                    TimedShakes.Remove(TimedShakes[0]);
                }
            }
        }

        public static void Shake(int Duration)
        {
            TimedShake TimedShake = new TimedShake(Duration);
            TimedShakes.Add(TimedShake);
        }

        public static void FocusToEntity(SpriteBatch _spriteBatch, Entity Entity)
        {
            CameraPosition = new Vector3((GlobalComponents.GetScreenWidth() / 2) - (GlobalComponents.TILE_SIZE / 2) - Entity.PositionX, (GlobalComponents.GetScreenHeight() / 2) - (GlobalComponents.TILE_SIZE / 2) - Entity.PositionY, 0);
            ShakeOffset = Vector2.Zero;

            RenderShake(_spriteBatch);

            if (!LightingSystem.IsLightingSystemEnabled())
            {
                _spriteBatch.End();
            }
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, CameraMatrix);
        }

        public static void FocusToCenter(SpriteBatch _spriteBatch)
        {
            CameraPosition = new Vector3((GlobalComponents.GetScreenWidth() / 2) - (GlobalComponents.TILE_SIZE / 2), (GlobalComponents.GetScreenHeight() / 2) - (GlobalComponents.TILE_SIZE / 2), 0);
            ShakeOffset = Vector2.Zero;

            RenderShake(_spriteBatch);

            if (!LightingSystem.IsLightingSystemEnabled())
            {
                _spriteBatch.End();
            }
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, CameraMatrix);
        }
    }
}
