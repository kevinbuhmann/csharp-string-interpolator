using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringInterpolator.Tests
{
    public static class Tester
    {
        public static void ValidateExpectedOutput(string input, string expectedOutput)
        {
            string firstRunOutput = Program.Rewrite(input);
            string secondRunOutput = Program.Rewrite(firstRunOutput);

            Assert.AreEqual(expectedOutput, firstRunOutput);
            Assert.AreEqual(expectedOutput, secondRunOutput);
        }
    }
}
