using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Liferoad
{
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

            var CoreAssembly = Assembly.GetExecutingAssembly().Location;
            if (!string.IsNullOrEmpty(CoreAssembly) && !References.Any(r => r.FilePath == CoreAssembly))
            {
                References.Add(MetadataReference.CreateFromFile(CoreAssembly));
            }

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
                        Debug.WriteLine(Diagnostic.GetMessage());
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
