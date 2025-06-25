namespace WebHost;

public record ApiError(string Message, params string[] Errors)
{
    public static ApiError Empty => new ("An unexpected error occurred.");
}