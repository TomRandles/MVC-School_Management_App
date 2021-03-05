using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace SchoolManagement.Utilities
{
    public static class ModelStateErrors
    {
        public static string ProcessModelStateErrors(IEnumerable<ModelStateEntry> errors)
        {
            string errorMsg = default;
            // foreach (var modelState in ViewData.ModelState.Values)
            foreach (var modelState in errors)
            {
                foreach (var error in modelState.Errors)
                {
                    errorMsg += error.ErrorMessage;
                    if (error.Exception != null)
                        errorMsg += error.Exception.Message;
                }
            }
            return errorMsg;
        }
    }
}
