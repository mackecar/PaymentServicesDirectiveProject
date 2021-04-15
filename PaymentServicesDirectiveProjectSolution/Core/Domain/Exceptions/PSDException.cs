using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Exceptions
{
    public class PSDException : Exception
    {
        public PSDException()
        {
        }

        public PSDException(string message) : base(message)
        {
        }
    }
}
