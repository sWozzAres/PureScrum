namespace ScrumApp.Exceptions;

internal class ScrumAppException : Exception
{
    public ScrumAppException()
    {
    }

    public ScrumAppException(string? message) : base(message)
    {
    }

    public ScrumAppException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
