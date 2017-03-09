namespace FluentResults
{
    public class MyError : Error
    {
        public MyError()
        {
            Message = "My message";
            ErrorCode = "1.1";
        }
    }
}