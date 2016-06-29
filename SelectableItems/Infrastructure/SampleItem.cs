namespace DynamicData.Samplz.Infrastructure
{
    public class SampleItem
    {
        public string Title { get;  }
        public string Description { get;  }
        public object Content { get;  }

        public SampleItem(string title, object content, string description)
        {
            Title = title;
            Description = description;
            Content = content;
        }
    }
}
