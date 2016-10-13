using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
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

            return expressions.Contains(node.Expression.ToString()) && node.IsLiteralFormatCall();
        }

        public static bool IsLiteralFormatCall(this InvocationExpressionSyntax node)
        {
            node.ValidateNotNullParameter(nameof(node));

            bool result = false;

            ArgumentSyntax firstArgument = node.ArgumentList.Arguments.FirstOrDefault();
            ExpressionSyntax firstArgumentExpression = firstArgument?.Expression;
            bool hasLiteralFirstArgument = firstArgumentExpression is LiteralExpressionSyntax;

            if (node.ArgumentList.Arguments.Count > 1 && hasLiteralFirstArgument)
            {
                string format = firstArgumentExpression.ToString();
                ExpressionSyntax formatExpression = SyntaxFactory.ParseExpression($"${format}");

                IEnumerable<InterpolationSyntax> interpolations = formatExpression.DescendantNodes()
                    .OfType<InterpolationSyntax>();

                int maxInterpolationIndex = interpolations.Any() ?
                    interpolations.Max(interpolation => int.Parse(interpolation.Expression.ToString())) : -1;

                int interpolationCount = maxInterpolationIndex + 1;
                int expectedArgumentCount = interpolationCount + 1;

                result = node.ArgumentList.Arguments.Count == expectedArgumentCount;
            }

            return result;
        }
    }
}
