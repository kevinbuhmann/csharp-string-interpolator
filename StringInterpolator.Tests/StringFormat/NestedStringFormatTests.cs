using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringInterpolator.Tests.StringFormat
{
    [TestClass]
    public class NestedStringFormatTests
    {
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

            Tester.ValidateExpectedOutput(input, expectedOutput);
        }

        [TestMethod]
        public void NestedStringFormatWithLeadingComments()
        {
            string input = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return string.Format(""Hello{0}, "", /*:asdf:*/ string.IsNullOrEmpty(name) ? /*:asdf:*/ string.Format("" {0}"", name) : string.Empty);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return $""Hello{/*:asdf:*/ (string.IsNullOrEmpty(name) ? /*:asdf:*/ $"" {name}"" : string.Empty)}, "";
    }
}";

            Tester.ValidateExpectedOutput(input, expectedOutput);
        }

        [TestMethod]
        public void NestedStringFormatWithTrailingComments()
        {
            string input = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return string.Format(""Hello{0}, "", string.IsNullOrEmpty(name) ? string.Format("" {0}"", name) /*:asdf:*/ : string.Empty /*:asdf:*/);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return $""Hello{(string.IsNullOrEmpty(name) ? $"" {name}"" /*:asdf:*/ : string.Empty) /*:asdf:*/}, "";
    }
}";

            Tester.ValidateExpectedOutput(input, expectedOutput);
        }

        [TestMethod]
        public void NestedStringFormatWithLeadingAndTrailingComments()
        {
            string input = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return string.Format(""Hello{0}, "", /*:asdf:*/ string.IsNullOrEmpty(name) ? /*:asdf:*/ string.Format("" {0}"", name) /*:asdf:*/ : string.Empty /*:asdf:*/);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetGreeting(string name)
    {
        return $""Hello{/*:asdf:*/ (string.IsNullOrEmpty(name) ? /*:asdf:*/ $"" {name}"" /*:asdf:*/ : string.Empty) /*:asdf:*/}, "";
    }
}";

            Tester.ValidateExpectedOutput(input, expectedOutput);
        }
    }
}