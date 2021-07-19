using System;

namespace ParkBee.Domain.Exceptions
{
    [Serializable]
    public class ParkBeeBaseException : ApplicationException
    {
        public ParkBeeBaseException(string message, Exception exception)
            : base(message, exception)
        {
            
        }

        public ParkBeeBaseException(string message)
            : base(message)
        {

        }
    }
}