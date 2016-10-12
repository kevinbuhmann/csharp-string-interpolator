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

        [TestMethod]
        public void NestedStringFormat()
        {
            string input = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return string.Format(""Hello{0}, "", string.IsNullOrEmpty(name) ? string.Format("" {0}"", name) : string.Empty);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return $""Hello{(string.IsNullOrEmpty(name) ? $"" {name}"" : string.Empty)}, "";
    }
}";

            Assert.AreEqual(expectedOutput, Program.Rewrite(input));
        }
    }
}