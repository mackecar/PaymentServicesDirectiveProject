using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Domain.DTOs;
using Domain.Entities;

namespace Core.ApplicationService.Extensions
{
    public static class TransactionDtoExtension
    {
        public static TransactionDto ToTransactionDto(this Transaction transaction)
        {
            return new TransactionDto(transaction);
        }

        public static ICollection<TransactionDto> ToTransactionDtos(this ICollection<Transaction> transactions)
        {
            return transactions.Select(t => new TransactionDto(t)).ToList();
        }
    }
}
