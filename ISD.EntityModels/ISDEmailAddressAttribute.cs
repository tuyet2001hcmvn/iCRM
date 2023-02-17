using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ISD.EntityModels
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public sealed class ISDEmailAddressAttribute : DataTypeAttribute
    {
        private const string DefaultErrorMessage = "{0} contains invalid email addresses.";
         private const string RegexPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*" +
                                        @"@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        public ISDEmailAddressAttribute()
            : base(DefaultErrorMessage)
        {
            // Set DefaultErrorMessage not ErrrorMessage, allowing user to set
            // ErrorMessageResourceType and ErrorMessageResourceName to use localized messages.
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is string valueAsString))
            {
                return false;
            }

            // only return true if there is only 1 '@' character
            // and it is neither the first nor the last character
            return Regex.IsMatch(valueAsString, RegexPattern, RegexOptions.IgnoreCase);
        }
    }
    
}
