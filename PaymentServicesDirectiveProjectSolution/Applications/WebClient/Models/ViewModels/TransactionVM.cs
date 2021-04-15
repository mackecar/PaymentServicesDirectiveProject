using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.DTOs;
using Core.Domain.Enums;

namespace Applications.WebClient.Models.ViewModels
{
    public class TransactionVM
    {
        public int Id { get; set; }
        public TransactionType TransactionType { get; set; }
        public int UserId { get; set; }
        public int? SourceAccount { get; set; }
        public int? DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public DateTime TransactionDate { get; set; }

        public TransactionVM() { }

        public TransactionVM(TransactionDto transaction)
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
