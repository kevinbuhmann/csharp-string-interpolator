using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringInterpolator.Tests.StringFormat
{
    [TestClass]
    public class EscapedBracesStringFormatTests
    {
        [TestMethod]
        public void EscapedBraceStringFormat()
        {
            string input = @"
public static class Source
{
    public static string GetCode(string variableName)
    {
        return string.Format(""string.Format(\""{0}.{{0}}\"", property);"", variableName);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetCode(string variableName)
    {
        return $""string.Format(\""{variableName}.{{0}}\"", property);"";
    }
}";

            Tester.ValidateExpectedOutput(input, expectedOutput);
        }
    }
}
