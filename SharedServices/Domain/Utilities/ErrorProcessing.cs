using ServicesLib.Services.Repository.Generic;
using System;

namespace ServicesLib.Domain.Utilities
{
    public static class ErrorProcessing
    {
        public static string ProcessException(string errorHeaderMessage, Exception e, ErrorType type = ErrorType.Error)
        {
            string message = errorHeaderMessage;
            if ((e != null) && (!string.IsNullOrEmpty(e.Message)))
                message += e.Message;
            if ((e.InnerException != null) && (!string.IsNullOrEmpty(e.InnerException.Message)))
                message += e.InnerException.Message;
            return message;
        }
    }
}
