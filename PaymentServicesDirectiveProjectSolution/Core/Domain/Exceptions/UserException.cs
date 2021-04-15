using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Exceptions
{
    public class UserException : PSDException
    {
        public UserException()
        {
        }

        public UserException(string message) : base(message)
        {
        }
    }
}
