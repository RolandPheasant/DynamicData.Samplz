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

                new SampleItem("Selectable Items", new SelectableItemsViewModel(),
                        "Filter on an object which implements INotifyPropertyChanged",
                        "SelectableItemsViewModel.cs",
                        "https://github.com/RolandPheasant/DynamicData.Samplz/blob/master/SelectableItems/Examples/SelectableItemsViewModel.cs"),

                new SampleItem("Aggregations", new AggregationViewModel(),
                        "Aggregate over a collection which is filtered on a property"
                        ,"AggregationViewModel.cs"
                        ,"https://github.com/RolandPheasant/DynamicData.Samplz/blob/master/SelectableItems/Examples/AggregationViewModel.cs"),

               new SampleItem("Filter An Observable", new  FilterObservableViewModel(), 
                        "Filter observable which is a property of an object",
                        "FilterObservableViewModel.cs",
                        "https://github.com/RolandPheasant/DynamicData.Samplz/blob/master/SelectableItems/Examples/FilterObservableViewModel.cs"),

            };
        }
    }
}
