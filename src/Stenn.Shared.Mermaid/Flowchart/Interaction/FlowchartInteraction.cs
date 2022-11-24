using System;
using Stenn.Shared.Text;

namespace Stenn.Shared.Mermaid.Flowchart.Interaction
{
    public abstract class FlowchartInteraction : ElementBase
    {
        private readonly Func<MermaidPrintConfig, string> _reference;
        private readonly string _action;

        /// <inheritdoc />
        protected FlowchartInteraction(Func<MermaidPrintConfig, string> reference, string? toolTip = null, string action = "click")
        {
            ToolTip = toolTip;
            _reference = reference ?? throw new ArgumentNullException(nameof(reference));
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        internal FlowchartGraphItem Item { get; set;} = default!;
        public string? ToolTip { get; set; }

        /// <inheritdoc />
        protected internal override bool Print(AdvStringBuilder builder, MermaidPrintConfig config)
        {
            builder.Append(_action);
            builder.Append(' ');
            builder.Append(Item.Id);
            builder.Append(' ');
            var reference = _reference(config);
            builder.Append(reference);
            if (!string.IsNullOrWhiteSpace(ToolTip))
            {
                builder.Append(' ');
                builder.Append('"');
                builder.Append(MermaidHelper.EscapeString(ToolTip, config));
                builder.Append('"');
            }
            return true;
        }

        
    }
}