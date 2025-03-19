using System.Linq;
using System.Text;

namespace RxClock.Clock
{
    public class TimeInputFormatter : ITimeInputFormatter
    {
        public string Format(string text)
        {
            string formatted = text.Length switch
            {
                (   0) => "",
                (<= 2) => text,
                (<= 4) => text.Insert(2, ":"),
                (   _) => text.Insert(2, ":").Insert(5, ":")
            };


            string[] parts = formatted.Split(":");
            
            // HH is uncapped
            if (parts.Length <= 1)
            {
                return formatted;
            }
            
            StringBuilder cappedTime = new (formatted.Take(3).ToString()); // HH:
            foreach (string part in parts.Skip(1))
            {
                cappedTime.AppendJoin(":", CapMinutesAndSeconds(part));
            }
            
            return cappedTime.ToString();
        }

        private string CapMinutesAndSeconds(string text)
        {
            return text.Length switch
            {
                1 when text[0] >= '6' => "5",
                2 when text[0] >= '6' => "59",
                _ => text
            };
        }
        
    }
}
