using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData.Binding;

namespace DynamicData.Samplz.Domain
{
    public class RelationEditor: AbstractNotifyPropertyChanged
    {

        public string Name { get; }

        public ObservableCollection<SelectablePerson> Children { get; }

        public RelationEditor(ParentWithChildren parentWithChildren, Person[] people)
        {
            Name = parentWithChildren.Parent.Name;

            Children = new ObservableCollection<SelectablePerson>
            (
                people.Where(p => p.Name != parentWithChildren.Parent.Name)
                    .Select(person => new SelectablePerson(person, parentWithChildren.Children.Contains(person)))
             );
        }

        public class SelectablePerson: AbstractNotifyPropertyChanged, IEquatable<SelectablePerson>
        {
            private readonly Person _person;
            private  bool _isSelected;

            public SelectablePerson(Person person, bool isSelected)
            {
                _person = person;
                _isSelected = isSelected;
            }

            public bool IsSelected
            {
                get { return _isSelected; }
                set { SetAndRaise(ref _isSelected, value); }
            }

            #region Equality

            public bool Equals(SelectablePerson other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(_person, other._person);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((SelectablePerson) obj);
            }

            public override int GetHashCode()
            {
                return (_person != null ? _person.GetHashCode() : 0);
            }

            public static bool operator ==(SelectablePerson left, SelectablePerson right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(SelectablePerson left, SelectablePerson right)
            {
                return !Equals(left, right);
            }

            #endregion

            public static explicit operator Person(SelectablePerson source)
            {
                return source._person;
            }

            public override string ToString()
            {
                return _person.Name;
            }
        }
    }
}
