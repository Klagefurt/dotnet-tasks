namespace dotnet_tasks.Exceptions
{
    public class WorksheetEmptyException : Exception
    {
        public WorksheetEmptyException(string? message) : base(message)
        {
        }
    }
}
