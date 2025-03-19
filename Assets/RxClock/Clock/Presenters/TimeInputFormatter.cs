using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RxClock.Clock
{
    public class TimeInputFormatter : ITimeInputFormatter
    {
        public string EditFormat(string text)
        {
            string digits = string.Concat(text.Where(char.IsDigit)); 
            
            string formatted = digits.Length switch
            {
                (   0) => "",
                (<= 2) => digits,
                (<= 4) => digits.Insert(2, ":"),
                (   _) => digits.Insert(2, ":").Insert(5, ":")
            };

            return formatted;
        }

        public string CommitFormat(string text)
        {
            string[] parts = text.Split(":");

            // Normalize string format
            if (parts.Length == 1)
            {
                text = $"{text}:00:00";
                return text; // HH is uncapped
            } 
            
            else if (parts.Length == 2)
            {
                text = $"{text}:00";
            }
            
            // Redo split with proper format
            parts = text.Split(":");

            StringBuilder cappedTime = new (string.Concat(text.Take(3))); // HH:
            List<string> timeParts = new(2);
            foreach (string part in parts.Skip(1))
            {
                timeParts.Add(CapMinutesAndSeconds(part));
            }
            
            cappedTime.AppendJoin(":", timeParts);
            return cappedTime.ToString();
        }

        private static string CapMinutesAndSeconds(string text)
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
