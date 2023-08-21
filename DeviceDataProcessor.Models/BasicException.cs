using System.Net;

namespace DeviceDataProcessor.Models;

public class BasicException : ApplicationException
{
    public HttpStatusCode HttpStatusCode { get; set; }

    public BasicException(string customMessage)
    {
    }
    
    public BasicException(string customMessage, HttpStatusCode statusCode, Exception innerException) : base(customMessage, innerException)
    {
        HttpStatusCode = statusCode;
    }
}