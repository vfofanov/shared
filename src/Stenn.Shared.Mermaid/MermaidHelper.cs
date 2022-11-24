using System;
using System.Collections.Generic;
using System.Linq;

namespace Stenn.Shared.Mermaid
{
    public static class MermaidHelper
    {
        public static string EscapeString(string value, MermaidPrintConfig config)
        {
            Func<char, string?> escape = config switch
            {
                MermaidPrintConfig.Normal => EscapeNormal,
                MermaidPrintConfig.ForHtml => EscapeToHtml,
                _ => throw new ArgumentOutOfRangeException(nameof(config))
            };
            return new string(EscapeStringInternal(value, escape).ToArray());
        }

        private static IEnumerable<char> EscapeStringInternal(string value, Func<char, string?> escape)
        {
            foreach (var symbol in value)
            {
                var escaped = escape(symbol);
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

        private static string? EscapeNormal(char symbol)
        {
            return symbol switch
            {
                '"' => "&quot",
                '&' => "&amp",
                '%' => "&#37",
                '\'' => "&#39",
                '(' => "&#40",
                ')' => "&#41",
                ';' => "&#59",
                '<' => "&lt",
                '=' => "&#61",
                '>' => "&gt",
                '[' => "&#91",
                ']' => "&#93",
                '{' => "&#123",
                '|' => "&#124",
                '}' => "&#125",
                '~' => "&#126",
                _ => null
            };
        }

        private static string? EscapeToHtml(char symbol)
        {
            return symbol switch
            {
                '"' => "&ampquot",
                '%' => "&amp#37",
                '(' => "&amp#40",
                ')' => "&amp#41",
                '{' => "&amp#123",
                '}' => "&amp#125",
                '[' => "&amp#91",
                ']' => "&amp#93",
                '~' => "&amp#126",
                _ => EscapeNormal(symbol)
            };
        }
    }
}