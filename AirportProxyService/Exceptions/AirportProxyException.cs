using System;

namespace AirportProxyService.Exceptions
{
    public class AirportProxyException : Exception
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}