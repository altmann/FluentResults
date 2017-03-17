namespace FluentResults
{
    public class Success : Reason
    {
        public Success(string message)
        {
            Message = message;
        }

        public Success WithTag(string tag)
        {
            Tags.Add(tag);
            return this;
        }

        public Success WithTags(params string[] tags)
        {
            Tags.AddRange(tags);
            return this;
        }
    }
}