using FluentAssertions;
using NUnit.Framework;

namespace RxClock.Tests.Editor.Clock
{
    public partial class TimerInputFormatterShould
    {
        [TestCase("", "")]
        [TestCase("1", "1")]
        [TestCase("12", "12")]
        [TestCase("123", "12:3")]
        [TestCase("1234", "12:34")]
        [TestCase("12345", "12:34:5")]
        [TestCase("123456", "12:34:56")]
        // Capping string to 8 characters is done via the TMP_InputField
        [TestCase("1234567", "12:34:567")]
        [TestCase("99999999", "99:99:9999")] 
        public void FormatUserInputAsStandardHHmmss(string userInput, string expected)
        {
            (string format, int _) = WhenEditFormatting(userInput);

            ThenTextShouldBeFormatted(format, expected);
        }
        
        [TestCase("12as34", "12:34")]
        [TestCase("alkfhlask", "")]
        [TestCase("12:34:56", "12:34:56")]
        [TestCase("1:2:3:4:", "12:34")]
        [TestCase("12:34:56:78:89", "12:34:567889")]
        public void IgnoreAllCharactersThatAreNotDigits(string userInput, string expected)
        {
            (string format, int _) = WhenEditFormatting(userInput);

            ThenTextShouldBeFormatted(format, expected);
        }
        
        [TestCase("", 0)]
        [TestCase("1", 0)]
        [TestCase("12", 0)]
        [TestCase("123", 1)]
        [TestCase("1234", 1)]
        [TestCase("12345", 2)]
        [TestCase("123456", 2)]
        [TestCase("1234567", 2)]
        [TestCase("1234asd567", 2)]
        [TestCase("99999999", 2)]
        [TestCase("12:34:56", 2)]
        [TestCase("12:34:56:78:89", 2)]
        public void CalculateCaretOffsetByAmountOfColonsInFormattedResult(string userInput, int expected)
        {
            (string _, int caretOffset) = WhenEditFormatting(userInput);

            ThenCaretOffsetShouldBe(caretOffset, expected);
        }
        
        private (string format, int) WhenEditFormatting(string userInput) => formatter.EditFormat(userInput);

        private void ThenTextShouldBeFormatted(string format, string expected)
        {
            format.Should().Be(expected);
        }
        
        private void ThenCaretOffsetShouldBe(int offset, int expected)
        {
            offset.Should().Be(expected);
        }
    }
}