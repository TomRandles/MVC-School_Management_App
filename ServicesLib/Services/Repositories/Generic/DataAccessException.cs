using System;
using System.Runtime.Serialization;

namespace ServicesLib.Services.Repository.Generic
{
    [Serializable]
    public class DataAccessException : Exception
    {
        private object errorMessage;

        public DataAccessException()
        {
        }

        public DataAccessException(object errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        public DataAccessException(string message) : base(message)
        {
        }

        public DataAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}