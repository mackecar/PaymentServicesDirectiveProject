using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain.DTOs;
using Core.Domain.Enums;

namespace Applications.WebClient.Models.ViewModels.TransactionVMS
{
    public class TransactionVM
    {
        public int Id { get; set; }

        [Display(Name = "Tip transackije")]
        public TransactionType TransactionType { get; set; }
        public int UserId { get; set; }
        public int? SourceAccount { get; set; }
        public int? DestinationAccount { get; set; }

        [Display(Name = "Iznos")]
        public decimal Amount { get; set; }

        [Display(Name = "Provizija")]
        public decimal Fee { get; set; }

        [Display(Name = "Datum transkacije")]
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
