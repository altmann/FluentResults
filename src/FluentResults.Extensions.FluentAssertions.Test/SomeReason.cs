namespace FluentResults.Extensions.FluentAssertions.Test
{
    internal class SomeReason : Error
    {
        public string Prop { get; set; }

        public SomeReason(string message) : base(message)
        {

        }
    }
}
