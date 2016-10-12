using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Vstack.Extensions;

namespace StringInterpolator
{
    public static class Extensions
    {
        public static bool IsLiteralStringFormat(this InvocationExpressionSyntax node)
        {
            node.ValidateNotNullParameter(nameof(node));

            string[] expressions = new string[]
            {
                "string.Format",
                "String.Format"
            };

            return expressions.Contains(node.Expression.ToString()) &&
                node.ArgumentList.Arguments.FirstOrDefault()?.Expression is LiteralExpressionSyntax;
        }
    }
}
