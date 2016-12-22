namespace DynamicData.Samplz.Domain
{
    public class ParentWithChildren
    {
        public Person Parent { get;  }
        public Person[] Children { get;  }
        
        public ParentWithChildren(Person parent, Person[] children)
        {
            Parent = parent;
            Children = children;
        }
    }
}