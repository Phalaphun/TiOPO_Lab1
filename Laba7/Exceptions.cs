using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba7
{
    internal class CustomExceptions
    {
        class InvalidSurnameException : Exception
        {
            public InvalidSurnameException(string message):base(message) { }
        }

        class InvalidNameException : Exception
        {
            public InvalidNameException(string message) : base(message) { }
        }

        class InvalidPatronimicException : Exception
        {
            public InvalidPatronimicException(string message) : base(message) { }
        }
    }
}
