namespace ExchangePrediction.Handlers
{
    using Constants;
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class AutoCompleteHandler : IAutoCompleteHandler
    {
        // characters to start completion from
        public char[] Separators { get; set; } = new char[] { ' ' };

        // text - The current text entered in the console
        // index - The index of the terminal cursor within {text}
        public string[] GetSuggestions(string text, int index)
        {
            if (Regex.IsMatch(text, $"^{Commands.ExchangePredict}[ ]+{Commands.Parameters.From}[ ]*=[ ]*[a-zA-Z]+[ ]+{GetGroupPattern(Commands.Parameters.To, 0, Commands.Parameters.To.Length, false, false, string.Empty)}$", RegexOptions.IgnoreCase))
            {
                return new string[] { $"{Commands.Parameters.To}=" };
            }

            if (Regex.IsMatch(text,  $"^{Commands.ExchangePredict}[ ]+{GetGroupPattern(Commands.Parameters.From, 0, Commands.Parameters.From.Length, false, false, string.Empty)}$", RegexOptions.IgnoreCase))
            {
                return new string[] { $"{Commands.Parameters.From}=" };
            }

            if (Regex.IsMatch(text, GetGroupPattern(Commands.ExchangePredict, 2, Commands.ExchangePredict.Length -3), RegexOptions.IgnoreCase))
            {
                return new string[] { Commands.ExchangePredict };
            }

            if (Regex.IsMatch(text, GetGroupPattern(Commands.Exit, 2, Commands.Exit.Length - 3), RegexOptions.IgnoreCase))
            {
                return new string[] { Commands.Exit };
            }

            if (Regex.IsMatch(text, GetGroupPattern(Commands.Clear, 0, Commands.Clear.Length - 1), RegexOptions.IgnoreCase))
            {
                return new string[] { Commands.Clear };
            }


            if (Regex.IsMatch(text, "(^e$|^ex$)", RegexOptions.IgnoreCase))
            {
                return new string[] { Commands.ExchangePredict, Commands.Exit };
            }

            return null;
        }

        private string GetGroupPattern(string text, int start, int end, bool startWith = true, bool endWith = true, string additionalPattern = null)
        {
            if (additionalPattern == null)
                return $"({string.Join("|", Enumerable.Range(start, end).Select(i => $"{(startWith ? "^" : "")}{text.Substring(0, i + 1)}{(endWith ? "$" : "")}"))})";

            return $"({string.Join("|", Enumerable.Range(start, end).Select(i => $"{(startWith ? "^" : "")}{text.Substring(0, i + 1)}{(endWith ? "$" : "")}"))}|{additionalPattern})";
        }
    }

}
