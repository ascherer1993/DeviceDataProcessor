using System.Net;

namespace DeviceDataProcessor.Models;

public class BasicException : ApplicationException
{
    public bool Success { get; set; }
    
    public string CustomMessage { get; set; }
    
    public HttpStatusCode HttpStatusCode { get; set; }

    public BasicException()
    {
    }
    
    public BasicException(string customMessage, HttpStatusCode statusCode, Exception innerException = null) : base(customMessage, innerException)
    {
        HttpStatusCode = statusCode;
    }
}