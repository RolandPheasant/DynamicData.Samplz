namespace DynamicData.Samplz.Domain
{
    public class Relation
    {
        public string Key { get; }

        public Person Parent { get;  }
        public Person Child { get;  }

        public Relation(Person parent, Person child)
        {
            Parent = parent;
            Child = child;
            Key = parent.Name + "." + child.Name;
        }
    }
}