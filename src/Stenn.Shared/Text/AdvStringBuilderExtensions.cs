using System.Text;

namespace Stenn.Shared.Text
{
    public static class AdvStringBuilderExtensions
    {
        public static AdvStringBuilder ToAdvanced(this StringBuilder stringBuilder, string identChunk = " ")
        {
            return new AdvStringBuilder(stringBuilder, identChunk);
        }
    }
}