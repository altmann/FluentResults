namespace FluentResults.Extensions.FluentAssertions.Test
{
    internal class SomeReason : Error
    {
        public string Prop { get; set; }

        public SomeReason(string message) : base(message)
        {

        }
    }

    internal class AnotherReason : Error
    {
        public AnotherReason(string message) : base(message)
        {

        }
    }
}
