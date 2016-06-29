namespace DynamicData.Samplz.Infrastructure
{
    public class SampleItem
    {
        public string Title { get;  }
        public string Description { get;  }
        public object Content { get;  }

        public string CodeFileDisplay { get; }

        public string CodeFileUrl { get; }

        public SampleItem(string title, object content, string description, string codeFileDisplay, string codeFileUrl)
        {
            Title = title;
            Description = description;
            CodeFileDisplay = codeFileDisplay;
            CodeFileUrl = codeFileUrl;
            Content = content;
        }
    }
}
