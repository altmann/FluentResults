namespace FluentResults
{
    public class Reason
    {
        public string Message { get; protected set; }

        protected virtual ReasonStringBuilder GetReasonStringBuilder()
        {
            return new ReasonStringBuilder()
                .WithReasonType(GetType());
        }

        public override string ToString()
        {
            return GetReasonStringBuilder()
                .Build();
        }
    }
}