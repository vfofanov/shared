using System;
using System.Collections.Generic;
using System.Linq;
using Stenn.Shared.Text;

namespace Stenn.Shared.Mermaid.Flowchart
{
    public sealed class FlowchartGraph: ElementBase
    {
        private readonly Dictionary<string, FlowchartGraphItem> _items;
        private readonly List<FlowchartRelation> _relations;

        public FlowchartGraph()
        {
            Root = new FlowchartGraphItem("###flowchart###", this, null);
            _items = new Dictionary<string, FlowchartGraphItem>();
            _relations = new List<FlowchartRelation>();
        }

        internal FlowchartGraphItem Root { get; }

        public FlowchartGraphDirection Direction
        {
            get => Root.Direction;
            set => Root.Direction = value;
        }

        private Dictionary<string, FlowchartStyleClass> StyleClasses { get; } = new();

        public FlowchartGraphItem? FindItem(string id)
        {
            _items.TryGetValue(id, out var item);
            return item;
        }

        public FlowchartGraphItem GetOrAdd(string id, string? name = null, int countChildren = 0, string? parentItemId = null,
            FlowchartGraphDirection? direction = default,
            string? styleClassId = default,
            FlowchartShape? shape = default)
        {
            var item = FindItem(id);
            if (item is null)
            {
                var parent = Root;
                if (parentItemId is { })
                {
                    parent = FindItem(parentItemId);
                    if (parent is null)
                    {
                        throw new ArgumentException($"Can't find graph item '{parentItemId}'", nameof(parentItemId));
                    }
                }
                item = parent.Add(id, name, countChildren);
            }
            
            if (name is { })
            {
                item.Caption = name;
            }
            if (direction is { } d)
            {
                item.Direction = d;
            }
            if (shape is { } s)
            {
                item.Shape = s;
            }
            if (styleClassId is not null)
            {
                item.StyleClassId = styleClassId;
            }
            return item;
        }

        public bool RemoveItem(string id)
        {
            if (!_items.TryGetValue(id, out var item))
            {
                return false;
            }

            SetStyleClassToItem(null, item);
            item.Parent!._children.Remove(item);
            return _items.Remove(id);
        }

        internal void AddItem(FlowchartGraphItem item)
        {
            _items.Add(item.Id, item);
            item.Parent!._children.Add(item);
        }

        public FlowchartStyleClass? FindStyleClass(string? id)
        {
            if (id is null)
            {
                return null;
            }
            StyleClasses.TryGetValue(id, out var ret);
            return ret;
        }

        public FlowchartStyleClass GetOrAddStyleClass(string? id = null)
        {
            string GetAutoId()
            {
                var index = StyleClasses.Count;
                do
                {
                    id = $"class{++index}";
                } while (StyleClasses.ContainsKey(id));
                return id;
            }

            return FindStyleClass(id)
                   ?? AddStyleClass(new FlowchartStyleClass(id ?? GetAutoId(), this));
        }

        public FlowchartStyleClass AddStyleClass(FlowchartStyleClass styleClass)
        {
            if (!ReferenceEquals(styleClass.Graph, this))
            {
                styleClass = styleClass.Copy(styleClass.Id, this);
            }
            StyleClasses.Add(styleClass.Id, styleClass);
            return styleClass;
        }

        public bool RemoveStyleClass(string id)
        {
            if (!StyleClasses.TryGetValue(id, out var styleClass))
            {
                return false;
            }

            foreach (var item in styleClass.Items)
            {
                item.Value._styleClassId = null;
            }
            return StyleClasses.Remove(id);
        }

        internal void SetStyleClassToItem(string? id, FlowchartGraphItem item)
        {
            if (item._styleClassId == id)
            {
                return;
            }

            if (item._styleClassId is { } prevClassId)
            {
                var styleClass = FindStyleClass(prevClassId);
                styleClass?.Items.Remove(item.Id);
            }
            if (id is { } classId)
            {
                var styleClass = FindStyleClass(classId);
                if (styleClass is null)
                {
                    throw new ArgumentException($"Can't find style class '{id}'", nameof(id));
                }
                styleClass.Items.Add(item.Id, item);
            }
            item._styleClassId = id;
        }

        public FlowchartRelation AddRelation(string leftItemId, string rightItemId, string? caption = null,
            FlowchartRelationLineEnding leftItemEnding = FlowchartRelationLineEnding.None,
            FlowchartRelationLineStyle lineStyle = FlowchartRelationLineStyle.Line,
            int lineLength = 0,
            FlowchartRelationLineEnding rightItemEnding = FlowchartRelationLineEnding.Arrow)
        {
            var leftItem = GetOrAdd(leftItemId);
            var rightItem = GetOrAdd(rightItemId);

            var relation = new FlowchartRelation(leftItem, rightItem, caption, leftItemEnding, lineStyle, lineLength, rightItemEnding);
            _relations.Add(relation);

            return relation;
        }

        /// <inheritdoc />
        protected internal override bool Print(AdvStringBuilder builder)
        {
            Root.Print(builder);
            builder.AppendLine();
            builder.AppendLine();

            builder.AppendLine("%%% Relations");
            
            foreach (var relation in _relations)
            {
                if(relation.Print(builder))
                {
                    builder.AppendLine();
                }
            }
            
            builder.AppendLine();

            builder.AppendLine("%%% Style classes");

            foreach (var styleClass in StyleClasses.Values.Where(s => s.Items.Count > 0 && s.Modifiers.Count > 0))
            {
                if (styleClass.Print(builder))
                {
                    builder.AppendLine();
                    builder.AppendLine();    
                }
            }

            return true;
        }
    }
}