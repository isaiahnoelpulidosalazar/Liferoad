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
        static int ShakeCounter = 0;
        public static List<TimedShake> TimedShakes = new List<TimedShake>();
        public static GraphicsDevice MainGraphicsDevice;
        public static Texture2D MessageBG;
        public static Texture2D Darkness;
        public static DepthStencilState DepthStencilStateForRead;
        public static DepthStencilState DepthStencilStateForWrite;
        public static BlendState DepthStencilBlendState;
        public static GameScenarioManager MainGameScenarioManager;
        public static GameMapManager MainGameMapManager;
        public static GameMap CurrentMap = null;
        public static Vector3 CameraPosition = new Vector3(0, 0, 0);

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

        public static void AddTimedShake(int Duration)
        {
            TimedShake TimedShake = new TimedShake(Duration);
            TimedShakes.Add(TimedShake);
        }

        public static void EnableLightSystem(ContentManager Content, SpriteBatch _spriteBatch)
        {
            foreach (GameObject GameObject in CurrentMap.GetObjects())
            {
                if (GameObject.IsPlayerNear)
                {
                    _spriteBatch.End();
                    _spriteBatch.Begin(blendState: DepthStencilBlendState, depthStencilState: DepthStencilStateForWrite);
                    _spriteBatch.Draw(Darkness, new Rectangle((int)Math.Round(GameObject.SolidBody.X + (TILE_SIZE / 2) - (Content.Load<SpriteFont>("DefaultFont").MeasureString("[E] to interact").X / 2)) + (int)Math.Round(CameraPosition.X), GameObject.SolidBody.Y - (TILE_SIZE / 2) + (int)Math.Round(CameraPosition.Y), (int)Math.Round(Content.Load<SpriteFont>("DefaultFont").MeasureString("[E] to interact").X) + 3, (int)Math.Round(Content.Load<SpriteFont>("DefaultFont").MeasureString("[E] to interact").Y)), Color.White);
                }
            }
            foreach (GameObject GameObject in CurrentMap.GetObjects())
            {
                if (GameObject.GetLightLevel() > 0)
                {
                    _spriteBatch.End();
                    _spriteBatch.Begin(blendState: DepthStencilBlendState, depthStencilState: DepthStencilStateForWrite);
                    _spriteBatch.Draw(Darkness, new Rectangle((int)Math.Round(GameObject.PositionX) + (int)Math.Round(CameraPosition.X) - (TILE_SIZE * (GameObject.GetLightLevel() - 1)) - (TILE_SIZE / 4), (int)Math.Round(GameObject.PositionY) + (int)Math.Round(CameraPosition.Y) - (TILE_SIZE * (GameObject.GetLightLevel() - 1)) - (TILE_SIZE / 4), TILE_SIZE + ((TILE_SIZE / 4) * 2) + (TILE_SIZE * (GameObject.GetLightLevel() - 1) * 2), TILE_SIZE + ((TILE_SIZE / 4) * 2) + (TILE_SIZE * (GameObject.GetLightLevel() - 1) * 2)), Color.White);
                }
            }
            _spriteBatch.End();
            _spriteBatch.Begin(depthStencilState: DepthStencilStateForRead);
            _spriteBatch.Draw(Darkness, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
            foreach (GameObject GameObject in CurrentMap.GetObjects())
            {
                if (GameObject.GetLightLevel() > 0)
                {
                    _spriteBatch.End();
                    _spriteBatch.Begin(blendState: DepthStencilBlendState, depthStencilState: DepthStencilStateForWrite);
                    _spriteBatch.Draw(Darkness, new Rectangle((int)Math.Round(GameObject.PositionX) + (int)Math.Round(CameraPosition.X) - (TILE_SIZE * GameObject.GetLightLevel()) - (TILE_SIZE / 4), (int)Math.Round(GameObject.PositionY) + (int)Math.Round(CameraPosition.Y) - (TILE_SIZE * GameObject.GetLightLevel()) - (TILE_SIZE / 4), TILE_SIZE + ((TILE_SIZE / 4) * 2) + (TILE_SIZE * GameObject.GetLightLevel() * 2), TILE_SIZE + ((TILE_SIZE / 4) * 2) + (TILE_SIZE * GameObject.GetLightLevel() * 2)), Color.White);
                }
            }
            _spriteBatch.End();
            _spriteBatch.Begin(depthStencilState: DepthStencilStateForRead);
            _spriteBatch.Draw(Darkness, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
        }

        public static void Shake(SpriteBatch _spriteBatch)
        {
            switch (ShakeCounter)
            {
                case 0:
                    _spriteBatch.End();
                    _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(new Vector3(CameraPosition.X + 2, CameraPosition.Y - 2, 0)));
                    break;
                case 1:
                    _spriteBatch.End();
                    _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(new Vector3(CameraPosition.X + 2, CameraPosition.Y + 2, 0)));
                    break;
                case 2:
                    _spriteBatch.End();
                    _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(new Vector3(CameraPosition.X - 2, CameraPosition.Y + 2, 0)));
                    break;
                case 3:
                    _spriteBatch.End();
                    _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(new Vector3(CameraPosition.X + 2, CameraPosition.Y - 2, 0)));
                    break;
                default:
                    _spriteBatch.End();
                    _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(new Vector3(CameraPosition.X - 2, CameraPosition.Y - 2, 0)));
                    ShakeCounter = 0;
                    break;
            }
            ShakeCounter++;
        }

        public static void FocusToEntity(SpriteBatch _spriteBatch, Entity Entity)
        {
            CameraPosition = new Vector3((SCREEN_WIDTH / 2) - (TILE_SIZE / 2) - Entity.PositionX, (SCREEN_HEIGHT / 2) - (TILE_SIZE / 2) - Entity.PositionY, 0);

            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(CameraPosition));

            RenderTimedShakes(_spriteBatch);
        }

        public static void FocusToCenter(SpriteBatch _spriteBatch)
        {
            CameraPosition = new Vector3((SCREEN_WIDTH / 2) - (TILE_SIZE / 2), (SCREEN_HEIGHT / 2) - (TILE_SIZE / 2), 0);

            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(CameraPosition));

            RenderTimedShakes(_spriteBatch);
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
