using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Text;
using Vstack.Common.Extensions;

namespace StringInterpolator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            args.ValidateNotNull();

            foreach (string path in args)
            {
                if (Directory.Exists(path))
                {
                    Console.Write($"Processing directory {path}... ");
                    string[] files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
                    Console.WriteLine($"{files.Length} files");

                    foreach (string file in files)
                    {
                        Console.Write($"Processing file {file}... ");
                        RewriteFile(file);
                        Console.WriteLine($"done");
                    }

                    Console.WriteLine();
                }
                else if (File.Exists(path))
                {
                    Console.Write($"Processing file {path}... ");
                    RewriteFile(path);
                    Console.WriteLine($"done");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"Error: {path} not found.");
                    Console.WriteLine();
                }
            }
        }

        public static string Rewrite(string source)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            SyntaxNode root = tree.GetRoot();

            StringInterpolatorRewriter rewriter = new StringInterpolatorRewriter();
            SyntaxNode rewrittenRoot = rewriter.Visit(root);

            return rewrittenRoot.ToFullString();
        }

        private static void RewriteFile(string file)
        {
            File.WriteAllText(file, Rewrite(File.ReadAllText(file)), Encoding.UTF8);
        }
    }
}
