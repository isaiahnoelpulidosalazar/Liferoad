using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Liferoad
{
    public class GlobalComponents
    {
        public static int SCREEN_WIDTH, SCREEN_HEIGHT, TILE_SIZE = 48;
        public static GraphicsDevice MainGraphicsDevice;
        public static GameScenarioManager MainGameScenarioManager;
        public static GameMapManager MainGameMapManager;
        public static GameMap CurrentMap = null;
        public static Vector3 CameraPosition = new Vector3(0, 0, 0);

        public static void FocusToEntity(SpriteBatch _spriteBatch, Entity Entity)
        {
            CameraPosition = new Vector3((SCREEN_WIDTH / 2) - (TILE_SIZE / 2) - Entity.PositionX, (SCREEN_HEIGHT / 2) - (TILE_SIZE / 2) - Entity.PositionY, 0);

            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(CameraPosition));

            //RenderTimedShakes(_spriteBatch);
        }

        public static void FocusToCenter(SpriteBatch _spriteBatch)
        {
            CameraPosition = new Vector3((SCREEN_WIDTH / 2) - (TILE_SIZE / 2), (SCREEN_HEIGHT / 2) - (TILE_SIZE / 2), 0);

            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(CameraPosition));

            //RenderTimedShakes(_spriteBatch);
        }

        public class Inputs
        {
            public static bool Up, Down, Left, Right;
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
