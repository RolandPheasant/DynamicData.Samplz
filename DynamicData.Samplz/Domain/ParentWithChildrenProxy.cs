using System;
using System.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using DynamicData.Samplz.Infrastructure;

namespace DynamicData.Samplz.Domain
{
    public class ParentWithChildrenProxy: AbstractNotifyPropertyChanged
    {
        private readonly ParentWithChildren _personWithChildren;
        public Person Parent => _personWithChildren.Parent;
        public string ChildrenNames { get; }
        public int ChildrenCount => _personWithChildren.Children.Length;
        public ICommand EditCommand { get; }

        public ParentWithChildrenProxy(ParentWithChildren personWithChildren, Action<ParentWithChildren> editAction)
        {
            _personWithChildren = personWithChildren;
            var children = personWithChildren.Children;
            ChildrenNames = children.Length == 0 ? "<None>" : string.Join(", ", children.Select(p => p.Name).OrderBy(s=>s));
                        EditCommand = new Command(() => editAction(personWithChildren));
        }
    }
}