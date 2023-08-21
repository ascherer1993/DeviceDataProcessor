using System.Net;

namespace DeviceDataProcessor.Models;

public class BasicException : ApplicationException
{
    public HttpStatusCode HttpStatusCode { get; set; }

    public BasicException(string customMessage)
    {
    }
    
    public BasicException(string customMessage, HttpStatusCode statusCode, Exception innerException = null) : base(customMessage, innerException)
    {
        HttpStatusCode = statusCode;
    }
}