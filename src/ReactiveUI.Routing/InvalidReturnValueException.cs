using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    /// <summary>
    /// Defines a class that represents an exception for an unexpected value returned from a method.
    /// </summary>
    public class InvalidReturnValueException : Exception
    {
        public InvalidReturnValueException()
        {
        }

        public InvalidReturnValueException(string message) : base(message)
        {
        }

        public InvalidReturnValueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
