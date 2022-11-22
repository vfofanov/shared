#nullable enable
using NUnit.Framework;
using Stenn.Shared.Mermaid.Flowchart;

namespace Stenn.Shared.Mermaid.Tests
{
    [TestFixture]
    public class FlowchartExtensionsTests
    {
        [Test]
        public void CreationTest()
        {
            var graph = new FlowchartGraph { Direction = FlowchartGraphDirection.TD };

            //--Counterparties
            var counterpartiesClass = graph.GetOrAddStyleClass("counterpartiesClass")
                .SetModifier("fill", "#ccffcc")
                .SetModifier("stroke-width", "2px")
                .SetModifier("stroke-dasharray", "2 2");

            graph.GetOrAdd("Domain_Counterparties", "Counterparties", 2, styleClassId: counterpartiesClass);
            
            graph.GetOrAdd("Country", parentItemId: "Domain_Counterparties", direction: FlowchartGraphDirection.LR);
            graph.GetOrAdd("Country_States","States", parentItemId: "Country", shape: FlowchartShape.Hexagon);
            
            graph.GetOrAdd("CountryState", parentItemId: "Domain_Counterparties", direction: FlowchartGraphDirection.LR);
            graph.GetOrAdd("CountryState_Country","Country", parentItemId: "CountryState", shape: FlowchartShape.BoxRoundEdges);

            //--Client
            var clientClass = graph.AddStyleClass(counterpartiesClass.Copy("clientClass")
                .SetModifier("fill", "#bbf"));

            graph.GetOrAdd("Domain_Client", "Client", 1, styleClassId: clientClass);
            
            graph.GetOrAdd("Person", parentItemId: "Domain_Client", direction: FlowchartGraphDirection.LR);
            graph.GetOrAdd("Person_Country", "Country", parentItemId: "Person", shape: FlowchartShape.BoxRoundEdges);

            graph.AddRelation("Country", "Person_Country", "fa:fa-1 CountryId fa:fa-N", FlowchartRelationLineEnding.None, FlowchartRelationLineStyle.BoldLine, 3);
            graph.AddRelation("Country_States", "CountryState_Country", "fa:fa-1 CountryId fa:fa-N", FlowchartRelationLineEnding.Arrow, FlowchartRelationLineStyle.Dots);
            
            var output = graph.ToString();
        }
    }
}