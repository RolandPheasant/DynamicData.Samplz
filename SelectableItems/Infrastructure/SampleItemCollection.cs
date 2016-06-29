using System.Collections.Generic;
using DynamicData.Samplz.Examples;

namespace DynamicData.Samplz.Infrastructure
{
    public class SelectableItemCollection
    {
        public List<SampleItem> Items { get; }

        public SelectableItemCollection()
        {
            Items = new List<SampleItem>
            {
                new SampleItem("Selectable Items", new SelectableItemsViewModel(),"How to filter on an object which implements INotifyPropertyChanged"),
                new SampleItem("Aggregations", new AggregationViewModel(),"Aggregate over a collection filtered on a property")
            };
        }
    }
}
