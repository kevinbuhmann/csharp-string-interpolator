using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringInterpolator.Tests
{
    [TestClass]
    public class StringFormatTests
    {
        [TestMethod]
        public void SimpleStringFormat()
        {
            string input = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return string.Format(""Hello, {0}"", name);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return $""Hello, {name}"";
    }
}";

            Assert.AreEqual(expectedOutput, Program.Rewrite(input));
        }
    }
}