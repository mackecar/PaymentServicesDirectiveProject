using System;
using System.Collections.Generic;
using Core.Domain.Exceptions;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PersonalNumber { get; private set; }
        public string BankName { get; private set; }
        public string BankAccountNumber { get; private set; }
        public string BankPinNumber { get; private set; }
        public string UserPass { get; private set; }
        public DateTime CreationDate { get; private set; }
        public decimal Amount { get; private set; }
        public bool IsBlocked { get; private set; }
        public DateTime? BlockDate { get; private set; }

        public List<Transaction> Transactions { get; set; }

        public User() { }

        public User(string firstName, 
            string lastName, 
            string personalNumber, 
            string bankName,
            string bankAccountNumber,
            string bankPinNumber)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetPersonalNumber(personalNumber);
            SetBankName(bankName);
            SetBankAccountNumber(bankAccountNumber);
            SetBankPinNumber(bankPinNumber);
            CreationDate = DateTime.Now;
            Amount = 0;
            IsBlocked = false;
            BlockDate = null;
        }

        private void SetFirstName(string firstName)
        {
            if(string.IsNullOrWhiteSpace(firstName)) throw new UserException("Ime mora biti popunjeno!");
            FirstName = firstName;
        }

        private void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName)) throw new UserException( "Prezime mora biti popunjeno!");
            LastName = lastName;
        }

        private void SetPersonalNumber(string personalNumber)
        {
            if (string.IsNullOrWhiteSpace(personalNumber)) throw new ArgumentNullException(personalNumber, "JMBG mora biti popunjeno!");
            if(personalNumber.Length != 13) throw new UserException("JMBG mora da ima 13 cifara!");
            if(!IsPersonAdult(personalNumber)) throw new UserException("Korisnik nije punoletan!");
            PersonalNumber = personalNumber;
        }

        private void SetBankName(string bankName)
        {
            if (string.IsNullOrWhiteSpace(bankName)) throw new UserException( "Ime banke mora biti popunjeno!");
            BankName = bankName;
        }

        private void SetBankAccountNumber(string bankAccountNumber)
        {
            if (string.IsNullOrWhiteSpace(bankAccountNumber)) throw new UserException( "Broj bankovnog racuna mora biti popunjeno!");
            BankAccountNumber = bankAccountNumber;
        }

        private void SetBankPinNumber(string bankPinNumber)
        {
            if (string.IsNullOrWhiteSpace(bankPinNumber)) throw new UserException( "PIN mora biti popunjen!");
            if(bankPinNumber.Length != 4)throw new UserException("PIN mora da ima 4 cifre!");
            BankPinNumber = bankPinNumber;
        }

        private bool IsPersonAdult(string personalNumber)
        {
            string year = personalNumber.Substring(4, 3);
            if (!year.StartsWith("9"))
            {
                year = $"2{year}";
                int age = Convert.ToInt32(year);
                int ageLimit = DateTime.Now.Year - 18;
                return age < ageLimit;
            }

            return true;
        }

        public void SetUserPass(string userPass)
        {
            UserPass = userPass;
        }

        public void Deposit(decimal amount)
        {
            Amount += amount;
        }

        public void Withdraw(decimal amount)
        {
            Amount -= amount;
        }

        public void BlockUser()
        {
            IsBlocked = true;
            BlockDate = DateTime.Now;
        }

        public void UnblockUser()
        {
            IsBlocked = false;
            BlockDate = null;
        }
    }
}
