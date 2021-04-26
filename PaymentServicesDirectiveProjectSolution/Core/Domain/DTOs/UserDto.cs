using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Domain.DTOs;
using Domain.Entities;

namespace Domain.DTOs
{
    public class UserDto
    {
        public int Id { get;  set; }
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string PersonalNumber { get;  set; }
        public string BankName { get;  set; }
        public string BankAccountNumber { get;  set; }
        public string BankPinNumber { get;  set; }
        public string UserPass { get;  set; }
        public DateTime CreationDate { get;  set; }
        public decimal Amount { get;  set; }
        public List<TransactionDto> Transactions { get; set; }

        public UserDto() { }

        public UserDto(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PersonalNumber = user.PersonalNumber;
            BankName = user.BankName;
            BankAccountNumber = user.BankAccountNumber;
            BankPinNumber = user.BankPinNumber;
            UserPass = user.UserPass;
            CreationDate = user.CreationDate;
            Amount = user.Amount;
            Transactions = user.Transactions?.Select(t => new TransactionDto(t)).ToList();
        }
    }
}
