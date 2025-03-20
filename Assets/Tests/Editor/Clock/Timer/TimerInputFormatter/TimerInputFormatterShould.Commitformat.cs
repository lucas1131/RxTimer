using NUnit.Framework;

namespace RxClock.Tests.Clock
{
    public partial class TimerInputFormatterShould
    {

        [TestCase("1", "10:00:00")]
        [TestCase("12", "12:00:00")]
        [TestCase("12:3", "12:30:00")]
        [TestCase("12:34", "12:34:00")]
        [TestCase("12:34:5", "12:34:50")]
        [TestCase("12:34:56", "12:34:56")]
        public void AddAppropriatePaddingForHHmmnnFormatFromMostToLeastSignificantDigit(string userInput, string expected)
        {
            string format = WhenCommitFormatting(userInput);

            ThenTextShouldBeFormatted(format, expected);
        }

        // Corner case, input field can handle this by itself using the placeholder text so the formatter simply does nothing
        [Test] 
        public void DoNothingWhenInputIsEmpty()
        {
            string format = WhenCommitFormatting(string.Empty);

            ThenTextShouldBeFormatted(format, string.Empty);
        }
        
        [TestCase("12:34:56", "12:34:56")]
        [TestCase("24:59:59", "24:59:59")]
        [TestCase("00:00:88", "00:00:59")]
        [TestCase("00:77:00", "00:59:00")]
        [TestCase("99:00:00", "99:00:00")]
        [TestCase("99:99:99", "99:59:59")]
        public void CapMinutesAndSecondsValue(string userInput, string expected)
        {
            string format = WhenCommitFormatting(userInput);

            ThenTextShouldBeFormatted(format, expected);
        }
        
        private string WhenCommitFormatting(string userInput) => formatter.CommitFormat(userInput);
        
    }
}