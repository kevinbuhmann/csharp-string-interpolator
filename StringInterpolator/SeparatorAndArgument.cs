using Microsoft.CodeAnalysis;
using Vstack.Extensions;

namespace StringInterpolator
{
    public class SeparatorAndArgument
    {
        public SeparatorAndArgument(SyntaxNodeOrToken separator, SyntaxNode argument)
        {
            separator.ValidateNotNullParameter(nameof(separator));
            argument.ValidateNotNullParameter(nameof(argument));

            this.Separator = separator;
            this.Argument = argument;
        }

        public SyntaxNodeOrToken Separator { get; }

        public SyntaxNode Argument { get; }

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
