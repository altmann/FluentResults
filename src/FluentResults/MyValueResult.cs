namespace FluentResults
{
    public class MyValueResult : ValueResultBase<MyValueResult, int>
    {
        public int MyNumber { get { return Value; } set { Value = value; } }
    }
}