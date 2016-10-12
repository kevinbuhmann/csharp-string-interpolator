using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace StringInterpolator
{
    public class StringInterpolatorRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.IsLiteralStringFormat())
            {
                return RewriteLiteralStringFormat(node);
            }

            return base.VisitInvocationExpression(node);
        }

        private static SyntaxNode RewriteLiteralStringFormat(InvocationExpressionSyntax node)
        {
            string format = node.ArgumentList.Arguments
                .First()
                .ToString();

            string[] parameters = node.ArgumentList.Arguments
                .Skip(1)
                .Select(n => n.ToString())
                .Select(p => p.Contains(":") ? $"({p})" : p)
                .ToArray();

            if (parameters.Any())
            {
                format = $"${format}";

                for (int i = 0; i < parameters.Length; i++)
                {
                    format = format.Replace($"{{{i}}}", $"{{{parameters[i]}}}");
                }
            }

            return SyntaxFactory.ParseExpression(format);
        }
    }
}
