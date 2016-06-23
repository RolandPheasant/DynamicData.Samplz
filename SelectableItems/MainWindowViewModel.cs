using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData.Binding;

namespace DynamicData.Samplz
{
    public class MainWindowViewModel: IDisposable
    {
        private readonly IDisposable _cleanUp;

        public ReadOnlyObservableCollection<SimpleItemViewModel> Selected { get; }
        public ReadOnlyObservableCollection<SimpleItemViewModel> NotSelected { get; }

        public MainWindowViewModel()
        {
            var sourceList = new SourceList<SimpleItem>();

            sourceList.AddRange(Enumerable.Range(1, 10).Select(i => new SimpleItem(i)));

            //create a shared list of view models
            var viewModels = sourceList.Connect()
                .Transform(simpleItem => new SimpleItemViewModel(simpleItem))
                .Publish();

            //filter on items which are selected and populate into an observable collection
            ReadOnlyObservableCollection<SimpleItemViewModel> selected;
            var selectedLoader = viewModels
                .FilterOnProperty(vm => vm.IsSeleted, vm => vm.IsSeleted)
                .Sort(SortExpressionComparer<SimpleItemViewModel>.Ascending(vm => vm.Number))
                .ObserveOnDispatcher()
                .Bind(out selected)
                .Subscribe();
            Selected = selected;

            //filter on items which are not selected and populate into an observable collection
            ReadOnlyObservableCollection<SimpleItemViewModel> notSselected;
            var notSelectedLoader = viewModels
                .FilterOnProperty(vm => vm.IsSeleted, vm => !vm.IsSeleted)
                .Sort(SortExpressionComparer<SimpleItemViewModel>.Ascending(vm => vm.Number))
                .ObserveOnDispatcher()
                .Bind(out notSselected)
                .Subscribe();
            NotSelected = notSselected;

            
            _cleanUp = new CompositeDisposable(sourceList,  selectedLoader, notSelectedLoader, viewModels.Connect());
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }

    public class SimpleItemViewModel: AbstractNotifyPropertyChanged
    {
        private bool _isSeleted;
        public SimpleItem Item { get;  }

        public int Number => Item.Id;

        public SimpleItemViewModel(SimpleItem item)
        {
            Item = item;
        }

        public bool IsSeleted
        {
            get { return _isSeleted; }
            set { SetAndRaise(ref _isSeleted, value); }
        }
    }

    public class SimpleItem
    {
        public int Id { get;  }

        public SimpleItem(int id)
        {
            Id = id;
        }
    }
}
