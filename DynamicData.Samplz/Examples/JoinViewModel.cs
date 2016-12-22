using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData.Binding;
using DynamicData.Samplz.Domain;
using MaterialDesignThemes.Wpf;

namespace DynamicData.Samplz.Examples
{
    public class JoinManyViewModel: AbstractNotifyPropertyChanged, IDisposable
    {
        private readonly IDisposable _cleanUp;
        private readonly SourceCache<Person, string> _people = new SourceCache<Person, string>(p=>p.Name);
        private readonly SourceCache<Relation, string> _relations = new SourceCache<Relation, string>(p => p.Key);

        public ReadOnlyObservableCollection<ParentWithChildrenProxy> Data { get; }

        public JoinManyViewModel()
        {
            ReadOnlyObservableCollection<ParentWithChildrenProxy> data;
            
            var parentsWithChildren = _people.Connect()
                .LeftJoinMany(_relations.Connect(), r => r.Parent.Name, (person, children) =>
                {
                    return new ParentWithChildren(person, children.Items.Select(c=>c.Child).ToArray());
                })
                //from this point, the operators are required only for presentation purposes 
                .Transform(pwc=> new ParentWithChildrenProxy(pwc, async x => await EditPerson(x)))
                .Sort(SortExpressionComparer<ParentWithChildrenProxy>.Ascending(p=>p.Parent.Age))
                .ObserveOnDispatcher()
                .Bind(out data)
                .Subscribe();

            Data = data;

            LoadInitialData();

            _cleanUp = new CompositeDisposable(parentsWithChildren, _people, _relations);
        }

        private async Task EditPerson(ParentWithChildren parentWithChildren)
        {
            //in a real world app you would pass _people in and allow the selectable items source to dynamically change
            var editor = new RelationEditor(parentWithChildren, _people.Items.ToArray());

            //TODO: Investigate why selectable state is being held onto ny the Dialog
            await DialogHost.Show(editor, (object sender, DialogClosingEventArgs eventArgs) =>
            {
                //use the .Edit method as it is more efficient when apply multiple changes
                _relations.Edit(innerCache =>
                {
                    //extract the new relations
                    var newRelations = editor.Children
                                .Where(c => c.IsSelected).Select(selectable => new Relation(parentWithChildren.Parent, (Person)selectable))
                                .ToArray();

                    //remove old ones
                    var toRemove = innerCache
                        .Items.Where(r => r.Parent == parentWithChildren.Parent && !newRelations.Contains(r))
                        .ToArray();

                    innerCache.Remove(toRemove);
                    innerCache.AddOrUpdate(newRelations);
                });
            });
        }

        private void LoadInitialData()
        {
            var people = Enumerable.Range(1, 15).Select(i => new Person("Person " + i, i + 10)).ToArray();
            var relations = new[]
            {
                new Relation(people[1], people[2]),
                new Relation(people[1], people[8]),
                new Relation(people[2], people[2]),
                new Relation(people[5], people[6]),
                new Relation(people[6], people[10]),
                new Relation(people[10], people[0]),
                new Relation(people[10], people[1]),
            };

            _relations.AddOrUpdate(relations);
            _people.AddOrUpdate(Enumerable.Range(1, 15).Select(i => new Person("Person " + i, i + 10)));
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}
