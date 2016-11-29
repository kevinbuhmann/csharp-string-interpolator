using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Vstack.Common;

namespace StringInterpolator
{
    public class StringInterpolatorRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.IsLiteralStringFormat())
            {
                return this.RewriteLiteralStringFormat(node);
            }
            else if (node.IsLiteralFormatCall())
            {
                return this.RewriterOtherLiteralFormatCall(node);
            }

            return base.VisitInvocationExpression(node);
        }

        private static SyntaxNode ReplaceInterpolation(InterpolationSyntax interpolation, ExpressionSyntax[] parameters)
        {
            int index = int.Parse(interpolation.Expression.ToString());
            return SyntaxFactory.Interpolation(parameters[index], interpolation.AlignmentClause, interpolation.FormatClause);
        }

        private SyntaxNode RewriteLiteralStringFormat(InvocationExpressionSyntax node)
        {
            string format = node.ArgumentList.Arguments
                .First()
                .ToString();

            ExpressionSyntax[] parameters = node.ArgumentList.Arguments
                .GetWithSeparators()
                .Skip(1)
                .Batch(2)
                .Select(i => new SeparatorAndArgument(i.First(), i.Last().AsNode()).ToExpression())
                .ToArray();

            ExpressionSyntax formatExpression = SyntaxFactory.ParseExpression($"${format}");

            IEnumerable<InterpolationSyntax> interpolations = formatExpression.DescendantNodes()
                .OfType<InterpolationSyntax>();

            ExpressionSyntax newInterpolation = formatExpression
                .ReplaceNodes(interpolations, (original, rewritten) => ReplaceInterpolation(original, parameters))
                .WithLeadingTrivia(node.GetLeadingTrivia())
                .WithTrailingTrivia(node.GetTrailingTrivia());

            return this.Visit(newInterpolation);
        }

        private SyntaxNode RewriterOtherLiteralFormatCall(InvocationExpressionSyntax node)
        {
            SyntaxNode stringInteroplationSyntax = this.RewriteLiteralStringFormat(node);
            string stringInteroplation = stringInteroplationSyntax.ToString();
            ArgumentListSyntax newArgumentList = SyntaxFactory.ParseArgumentList($"({stringInteroplation})");
            InvocationExpressionSyntax newInvocationSyntax = node.WithArgumentList(newArgumentList);

            return newInvocationSyntax;
        }
    }
}
