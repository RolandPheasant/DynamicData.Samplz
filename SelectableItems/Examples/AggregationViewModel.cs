using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData.Binding;
using DynamicData.Aggregation;

namespace DynamicData.Samplz.Examples
{
    public class AggregationViewModel : AbstractNotifyPropertyChanged, IDisposable
    {
        private readonly IDisposable _cleanUp;

        public ReadOnlyObservableCollection<AggregationItem> Items { get; }

        private int _count;
        private int _max;
        private double _stdDev;
        private double _avg;
        private int _min;
        private double _sumOfOddNumbers;
        private int _sum;
        private int _countIncluded;

        public AggregationViewModel()
        {
            var sourceList = new SourceList<AggregationItem>();

            sourceList.AddRange(Enumerable.Range(1, 15).Select(i => new AggregationItem(i)));

            //Load items to display to user and allow them to include items or not
            ReadOnlyObservableCollection<AggregationItem> items;
            var listLoader = sourceList.Connect()
                .Sort(SortExpressionComparer<AggregationItem>.Ascending(vm => vm.Number))
                .ObserveOnDispatcher()
                .Bind(out items)
                .Subscribe();
            Items = items;

            // share the connection because we are doing multiple aggregations
            var aggregatable = sourceList.Connect()
                .FilterOnProperty(vm => vm.IncludeInTotal, vm => vm.IncludeInTotal)
                .Publish();

            //Do a custom aggregation
            var sumOfOddNumbers = aggregatable.ToCollection()
                .Select(collection => collection.Where(i => i.Number%2 == 1).Select(ai => ai.Number).Sum())
                .Subscribe(sum => SumOfOddNumbers = sum);
            
            _cleanUp = new CompositeDisposable(sourceList, 
                listLoader,
                aggregatable.Count().Subscribe(count => Count = count),
                aggregatable.Sum(ai => ai.Number).Subscribe(sum => Sum = sum),
                aggregatable.Avg(ai => ai.Number).Subscribe(average => Avg = Math.Round(average,2)),
                aggregatable.Minimum(ai => ai.Number).Subscribe(max => Max = max),
                aggregatable.Maximum(ai => ai.Number).Subscribe(min => Min = min),
                aggregatable.StdDev(ai => ai.Number).Subscribe(std => StdDev = Math.Round(std, 2)),
                sumOfOddNumbers,
                aggregatable.Connect());
        }
        

        public int Count
        {
            get { return _count; }
            set { SetAndRaise(ref _count, value); }
        }

        public int CountIncluded
        {
            get { return _countIncluded; }
            set { SetAndRaise(ref _countIncluded, value); }
        }

        public int Sum
        {
            get { return _sum; }
            set { SetAndRaise(ref _sum, value); }
        }

        public int Min
        {
            get { return _min; }
            set { SetAndRaise(ref _min, value); }
        }

        public int Max
        {
            get { return _max; }
            set { SetAndRaise(ref _max, value); }
        }

        public double StdDev
        {
            get { return _stdDev; }
            set { SetAndRaise(ref _stdDev, value); }
        }
        
        public double Avg
        {
            get { return _avg; }
            set { SetAndRaise(ref _avg, value); }
        }

        public double SumOfOddNumbers
        {
            get { return _sumOfOddNumbers; }
            set { SetAndRaise(ref _sumOfOddNumbers, value); }
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }

    public class AggregationItem : AbstractNotifyPropertyChanged
    {
        public int Number { get; }

        private bool _includeInTotal = true;
        
        public AggregationItem(int number)
        {
            Number = number;
        }

        public bool IncludeInTotal
        {
            get { return _includeInTotal; }
            set { SetAndRaise(ref _includeInTotal, value); }
        }
    }
}
