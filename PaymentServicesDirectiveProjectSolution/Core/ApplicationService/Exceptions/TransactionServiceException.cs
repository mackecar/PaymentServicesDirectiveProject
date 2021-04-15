using System;
using System.Collections.Generic;
using System.Text;
using Core.Domain.Exceptions;

namespace Core.ApplicationService.Exceptions
{
    public class TransactionServiceException : PSDException
    {
        public TransactionServiceException()
        {
        }

        public TransactionServiceException(string message) : base(message)
        {
        }
    }
}
