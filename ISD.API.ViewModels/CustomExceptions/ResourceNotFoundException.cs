using System;
using System.Globalization;

namespace ISD.API.ViewModels.CustomExceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base() { }

        public ResourceNotFoundException(string message) : base(message) { }

        public ResourceNotFoundException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
