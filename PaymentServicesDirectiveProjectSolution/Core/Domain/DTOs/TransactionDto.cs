using System;
using System.Collections.Generic;
using System.Text;
using Core.Domain.Enums;
using Domain.DTOs;
using Domain.Entities;

namespace Core.Domain.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public TransactionType TransactionType { get; set; }
        public int UserId { get; set; }
        public int? SourceAccount { get; set; }
        public int? DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public DateTime TransactionDate { get; set; }

        public TransactionDto() { }

        public TransactionDto(Transaction transaction)
        {
            Id = transaction.Id;
            TransactionType = transaction.TransactionType;
            UserId = transaction.UserId;
            SourceAccount = transaction.SourceAccount;
            DestinationAccount = transaction.DestinationAccount;
            Amount = transaction.Amount;
            Fee = transaction.Fee;
            TransactionDate = transaction.TransactionDate;
        }
    }
}
