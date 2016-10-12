﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringInterpolator.Tests.StringFormat
{
    [TestClass]
    public class SimpleStringFormatTests
    {
        [TestMethod]
        public void SimpleStringFormat()
        {
            string input = @"
public static class Source
{
    public static string GetGreeting(string firstName, string lastName)
    {
        return string.Format(""Hello {0} {1},"", firstName, lastName);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetGreeting(string firstName, string lastName)
    {
        return $""Hello {firstName} {lastName},"";
    }
}";

            Assert.AreEqual(expectedOutput, Program.Rewrite(input));
        }

        [TestMethod]
        public void SimpleStringFormatWithLeadingComments()
        {
            string input = @"
public static class Source
{
    public static string GetGreeting(string firstName, string lastName)
    {
        return string.Format(""Hello {0} {1},"", /*asdf*/ firstName, /*asdf*/ lastName);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetGreeting(string firstName, string lastName)
    {
        return $""Hello {/*asdf*/ firstName} {/*asdf*/ lastName},"";
    }
}";

            Assert.AreEqual(expectedOutput, Program.Rewrite(input));
        }

        [TestMethod]
        public void SimpleStringFormatWithTrailingComments()
        {
            string input = @"
public static class Source
{
    public static string GetGreeting(string firstName, string lastName)
    {
        return string.Format(""Hello {0} {1},"", firstName /*asdf*/, lastName /*asdf*/);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetGreeting(string firstName, string lastName)
    {
        return $""Hello {firstName /*asdf*/} {lastName /*asdf*/},"";
    }
}";

            Assert.AreEqual(expectedOutput, Program.Rewrite(input));
        }

        [TestMethod]
        public void SimpleStringFormatWithLeadingLeadingAndTrailingComments()
        {
            string input = @"
public static class Source
{
    public static string GetGreeting(string firstName, string lastName)
    {
        return string.Format(""Hello {0} {1},"", /*asdf*/ firstName /*asdf*/, /*asdf*/ lastName /*asdf*/);
    }
}";

            string expectedOutput = @"
public static class Source
{
    public static string GetGreeting(string firstName, string lastName)
    {
        return $""Hello {/*asdf*/ firstName /*asdf*/} {/*asdf*/ lastName /*asdf*/},"";
    }
}";

            Assert.AreEqual(expectedOutput, Program.Rewrite(input));
        }
    }
}