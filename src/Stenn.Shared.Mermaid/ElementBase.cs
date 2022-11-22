using Stenn.Shared.Text;

namespace Stenn.Shared.Mermaid
{
    public abstract class ElementBase
    {
        protected internal abstract bool Print(AdvStringBuilder builder);

        /// <inheritdoc />
        public override string? ToString()
        {
            var builder = new AdvStringBuilder();
            Print(builder);
            return builder.ToString();
        }
    }
}