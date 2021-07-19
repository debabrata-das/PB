namespace ParkBee.WebApplication.Server.CustomExceptionMiddleware.ProblemDetails
{
    public class ParkBeeProblemDetail : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public ParkBeeProblemDetail(string title, string message)
        {
            Title = title;
            Status = 400;
            Type = typeof(ParkBeeProblemDetail).ToString();
            Detail = message;
        }
    }
}
