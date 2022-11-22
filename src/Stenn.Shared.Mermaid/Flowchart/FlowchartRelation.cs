using System;
using Stenn.Shared.Text;

namespace Stenn.Shared.Mermaid.Flowchart
{
    public sealed class FlowchartRelation:ElementBase
    {
        private int _lineLength;

        public FlowchartRelation(
            FlowchartGraphItem leftItem,
            FlowchartGraphItem rightItem,
            string? caption = null,
            FlowchartRelationLineEnding leftItemEnding = FlowchartRelationLineEnding.None,
            FlowchartRelationLineStyle lineStyle = FlowchartRelationLineStyle.Line,
            int lineLength = 0,
            FlowchartRelationLineEnding rightItemEnding = FlowchartRelationLineEnding.Arrow)
        {
            LeftItem = leftItem;
            RightItem = rightItem;
            Caption = caption;
            LeftItemEnding = leftItemEnding;
            LineStyle = lineStyle;
            LineLength = lineLength;
            RightItemEnding = rightItemEnding;
        }

        public FlowchartGraphItem LeftItem { get; set; }
        public FlowchartGraphItem RightItem { get; set; }
        public string? Caption { get; set; }
        public FlowchartRelationLineEnding LeftItemEnding { get; set; }
        public FlowchartRelationLineStyle LineStyle { get; set; }

        public int LineLength
        {
            get => _lineLength;
            set
            {
                if (_lineLength < 0)
                {
                    throw new ArgumentException("Can't be less than 0", nameof(value));
                }
                _lineLength = value;
            }
        }

        public FlowchartRelationLineEnding RightItemEnding { get; set; }

        /// <inheritdoc />
        protected internal override bool Print(AdvStringBuilder builder)
        {
            void AddLineLength(char lineChar, int modifier = 0)
            {
                var lineLength = _lineLength + modifier;
                switch (lineLength)
                {
                    case 0:
                        return;
                    case 1:
                        builder.Append(lineChar);
                        return;
                    default:
                        builder.Append(new string(lineChar, _lineLength));
                        break;
                }
            }

            builder.Append(MermaidHelper.EscapeString(LeftItem.Id));
            switch(LeftItemEnding)
            {
                case FlowchartRelationLineEnding.None:
                    break;
                case FlowchartRelationLineEnding.Arrow:
                    builder.Append('<');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (LineStyle)
            {
                case FlowchartRelationLineStyle.Line:
                    builder.Append('-');
                    AddLineLength('-');
                    builder.Append('-');
                    break;
                case FlowchartRelationLineStyle.BoldLine:
                    builder.Append('=');
                    AddLineLength('=');
                    builder.Append('=');
                    break;
                case FlowchartRelationLineStyle.Dots:
                    builder.Append('-');
                    AddLineLength('.', 1);
                    builder.Append('-');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            switch(RightItemEnding)
            {
                case FlowchartRelationLineEnding.None:
                    break;
                case FlowchartRelationLineEnding.Arrow:
                    builder.Append('>');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (!string.IsNullOrWhiteSpace(Caption))
            {
                builder.Append('|');
                builder.Append(MermaidHelper.EscapeString(Caption));
                builder.Append('|');
            }
            builder.Append(MermaidHelper.EscapeString(RightItem.Id));

            return true;
        }
    }
}