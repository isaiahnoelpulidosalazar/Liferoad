using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Liferoad
{
    public class GlobalComponents
    {
        public static int SCREEN_WIDTH, SCREEN_HEIGHT, TILE_SIZE = 48, VERY_SHORT = 5, SHORT = 15, MEDIUM = 30, LONG = 75;
        public static bool IsLightingSystemEnabled = false;
        static int ShakeCounter = 0;
        public static List<TimedShake> TimedShakes = new List<TimedShake>();
        public static GraphicsDevice MainGraphicsDevice;
        public static Texture2D MessageBG;
        public static Texture2D Darkness;
        public static Texture2D WhiteTexture;
        public static RenderTarget2D LightMask;
        public static Vector2 ShakeOffset = Vector2.Zero;
        public static DepthStencilState DepthStencilStateForRead;
        public static DepthStencilState DepthStencilStateForWrite;
        public static BlendState DepthStencilBlendState;
        public static GameScenarioManager MainGameScenarioManager;
        public static GameMapManager MainGameMapManager;
        public static GameScenario CurrentScenario = null;
        public static GameMap CurrentMap = null;
        public static Vector3 CameraPosition = new Vector3(0, 0, 0);
        public static Matrix CameraMatrix => Matrix.CreateTranslation(CameraPosition + new Vector3(ShakeOffset, 0));
        public static BlendState LightCutoutBlend = new BlendState
        {
            ColorSourceBlend = Blend.Zero,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            AlphaSourceBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };

        public class TimedShake
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
                    Shake(_spriteBatch);
                    TimedShakeCounter++;
                }
                else
                {
                    IsActive = false;
                }
            }
        }

        public static void RenderTimedShakes(SpriteBatch _spriteBatch)
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

        public static void ChangeMap(string Name)
        {
            MainGameMapManager.ChangeMap(Name);
        }

        public static void ChangeScenario(string Name)
        {
            MainGameScenarioManager.ChangeScenario(Name);
        }

        public static void AddTimedShake(int Duration)
        {
            TimedShake TimedShake = new TimedShake(Duration);
            TimedShakes.Add(TimedShake);
        }

        public static void EnableLightingSystem(ContentManager Content, SpriteBatch _spriteBatch)
        {
            IsLightingSystemEnabled = true;
            _spriteBatch.End();

            MainGraphicsDevice.SetRenderTarget(LightMask);
            MainGraphicsDevice.Clear(new Color(0, 0, 0, 200));

            _spriteBatch.Begin(transformMatrix: CameraMatrix, blendState: LightCutoutBlend);

            foreach (GameObject Object in CurrentMap.GetObjects())
            {
                if (Object.IsPlayerNear)
                {
                    Vector2 FontRectangle = Content.Load<SpriteFont>("DefaultFont").MeasureString("[E] to interact");
                    Rectangle Bounds = new Rectangle(
                        (int)(Object.SolidBody.X + (TILE_SIZE / 2) - (FontRectangle.X / 2)),
                        Object.SolidBody.Y - (TILE_SIZE / 2),
                        (int)FontRectangle.X + 3,
                        (int)FontRectangle.Y
                    );
                    _spriteBatch.Draw(WhiteTexture, Bounds, Color.White);
                }

                if (Object.GetLightLevel() > 0)
                {
                    int LightRadius = Object.GetLightLevel() * TILE_SIZE * 2;
                    Rectangle LightRect = new Rectangle(
                        (int)Object.PositionX - (LightRadius / 2) + (TILE_SIZE / 2),
                        (int)Object.PositionY - (LightRadius / 2) + (TILE_SIZE / 2),
                        LightRadius,
                        LightRadius
                    );
                    _spriteBatch.Draw(WhiteTexture, LightRect, Color.White);
                }
            }

            _spriteBatch.End();
            MainGraphicsDevice.SetRenderTarget(null);
        }

        public static void DisableLightingSystem()
        {
            IsLightingSystemEnabled = false;
        }

        public static void Shake(SpriteBatch _spriteBatch)
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

        public static void FocusToEntity(SpriteBatch _spriteBatch, Entity Entity)
        {
            CameraPosition = new Vector3((SCREEN_WIDTH / 2) - (TILE_SIZE / 2) - Entity.PositionX, (SCREEN_HEIGHT / 2) - (TILE_SIZE / 2) - Entity.PositionY, 0);
            ShakeOffset = Vector2.Zero;

            RenderTimedShakes(_spriteBatch);

            if (!IsLightingSystemEnabled)
            {
                _spriteBatch.End();
            }
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, CameraMatrix);
        }

        public static void FocusToCenter(SpriteBatch _spriteBatch)
        {
            CameraPosition = new Vector3((SCREEN_WIDTH / 2) - (TILE_SIZE / 2), (SCREEN_HEIGHT / 2) - (TILE_SIZE / 2), 0);
            ShakeOffset = Vector2.Zero;

            RenderTimedShakes(_spriteBatch);

            if (!IsLightingSystemEnabled)
            {
                _spriteBatch.End();
            }
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, CameraMatrix);
        }

        public class Inputs
        {
            public static bool Up, Down, Left, Right, MouseDown, MousePress, MouseUp, Interact, InteractReset;
            public static int MouseX, MouseY;
            public static float Theta, AngleInDegrees;
        }

        public class Compiler
        {
            public static Assembly Run(string FilePath)
            {
                string Code = File.ReadAllText(FilePath);
                string[] Split = FilePath.Split("\\");

                var CompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

                var References = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(Assembly => !Assembly.IsDynamic && !string.IsNullOrEmpty(Assembly.Location))
                    .Select(Assembly => MetadataReference.CreateFromFile(Assembly.Location))
                    .ToList();

                var SyntaxTree = SyntaxFactory.ParseSyntaxTree(Code);
                var Compilation = CSharpCompilation.Create(Split[Split.Length - 1])
                    .WithOptions(CompilationOptions)
                    .AddReferences(References)
                    .AddSyntaxTrees(SyntaxTree);

                using (MemoryStream MemoryStream = new MemoryStream())
                {
                    var EmitResult = Compilation.Emit(MemoryStream);

                    if (!EmitResult.Success)
                    {
                        foreach (var Diagnostic in EmitResult.Diagnostics)
                        {
                            Console.WriteLine(Diagnostic.GetMessage());
                        }
                    }
                    else
                    {
                        MemoryStream.Seek(0, SeekOrigin.Begin);

                        return Assembly.Load(MemoryStream.ToArray());
                    }
                }

                return null;
            }
        }
    }
}
