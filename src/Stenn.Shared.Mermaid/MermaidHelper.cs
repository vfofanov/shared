using System.Collections.Generic;
using System.Linq;

namespace Stenn.Shared.Mermaid
{
    public static class MermaidHelper
    {
        public static string EscapeString(string value)
        {
            return new string(EscapeStringInternal(value).ToArray());
        }

        private static IEnumerable<char> EscapeStringInternal(string value)
        {
            foreach (var symbol in value)
            {
                var escaped = Escape(symbol);
                if (escaped is null)
                {
                    yield return symbol;
                }
                else
                {
                    foreach (var s in escaped)
                    {
                        yield return s;
                    }
                }
            }
        }

        private static string? Escape(char symbol)
        {
            switch (symbol)
            {
                case '"':
                    return "&quot";
                case '&':
                    return "&amp";
                case '%':
                    return "&#37";
                case '\'':
                    return "&#39";
                case '(':
                    return "&#40";
                case ')':
                    return "&#41";
                case ';':
                    return "&#59";
                case '<':
                    return "&lt";
                case '=':
                    return "&#61";
                case '>':
                    return "&gt";
                case '[':
                    return "&#91";
                case ']':
                    return "&#93";
                case '{':
                    return "&#123";
                case '|':
                    return "&#124";
                case '}':
                    return "&#125";
                case '~':
                    return "&#126";
                // case '-':
                //     return "&#45";
                // case ':':
                //     return "&#58";
                default:
                    return null;
            }
        }
    }
}