using System;
using System.Collections.Generic;
using System.Text;
using Core.Domain.Exceptions;

namespace Core.ApplicationService.Exceptions
{
    public class UserServiceException : PSDException
    {
        public UserServiceException()
        {
        }

        public UserServiceException(string message) : base(message)
        {
        }
    }
}
