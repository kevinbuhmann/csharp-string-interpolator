using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Vstack.Common.Extensions;

namespace StringInterpolator
{
    public class SeparatorAndArgument
    {
        public SeparatorAndArgument(SyntaxNodeOrToken separator, SyntaxNode argument)
        {
            argument.ValidateNotNull();

            this.Separator = separator;
            this.Argument = argument;
        }

        public SyntaxNodeOrToken Separator { get; }

        public SyntaxNode Argument { get; }

        public ExpressionSyntax ToExpression()
        {
            return SyntaxFactory.ParseExpression(this.ToString());
        }

        public override string ToString()
        {
            string argumentString = this.Argument.ToString();

            if (argumentString.Contains(":"))
            {
                argumentString = $"({this.Argument.ToString()})";
            }

            string leadingTrivia = this.Separator.GetTrailingTrivia().ToString().TrimStart();
            string trailingTrivia = this.Argument.GetTrailingTrivia().ToString().TrimEnd();

            return $"{leadingTrivia}{argumentString}{trailingTrivia}";
        }
    }
}
