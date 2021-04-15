using System;
using Core.Domain.Enums;
using Core.Domain.Exceptions;

namespace Domain.Entities
{
    public class Transaction
    {
        public int Id { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public int UserId { get; private set; }
        public User User { get; set; }
        public int? SourceAccount { get; private set; }
        public int? DestinationAccount { get; private set; }
        public decimal Amount { get; private set; }
        public decimal Fee { get; private set; }
        public DateTime TransactionDate { get; private set; }

        public Transaction(TransactionType transactionType,
            int userId,
            int? sourceAccount,
            int? destinationAccount,
            decimal amount,
            decimal fee)
        {
            TransactionType = transactionType;
            UserId = userId;

            if (transactionType == TransactionType.PayIn && sourceAccount == null) throw new TransactionException("Izvor uplate mora biti nunesen!");
            if (transactionType == TransactionType.PayOut && destinationAccount == null) throw new TransactionException("Destinacija isplate mora biti nunesen!");

            SourceAccount = sourceAccount;
            DestinationAccount = destinationAccount;

            if(amount <= 0) throw new TransactionException("Iznos mora biti veci od nule!");
            Amount = amount;

            if (amount < 0) throw new TransactionException("Provizija mora biti veci ili jednaka nuli!");
            Fee = fee;

            TransactionDate = DateTime.Now;
        }
    }
}
