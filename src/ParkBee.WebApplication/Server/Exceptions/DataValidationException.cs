using System;
using ParkBee.Domain.Exceptions;

namespace ParkBee.WebApplication.Server.Exceptions
{
    [Serializable]
    public class DataValidationException : ParkBeeBaseException
    {
        public DataValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}