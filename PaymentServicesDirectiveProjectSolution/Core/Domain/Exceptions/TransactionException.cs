using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Exceptions
{
    public class TransactionException : PSDException
    {
        public TransactionException()
        {
        }

        public TransactionException(string message) : base(message)
        {
        }
    }
}
