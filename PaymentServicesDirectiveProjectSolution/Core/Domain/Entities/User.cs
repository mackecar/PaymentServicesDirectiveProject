using System;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PersonalNumber { get; private set; }
        public string BankAccountNumber { get; private set; }
        public string BankPinNumber { get; private set; }
        public string UserPass { get; private set; }
        public DateTime CreationDate { get; private set; }

        public User() { }

        public User(string firstName, string lastName, string personalNumber, string bankAccountNumber,
            string bankPinNumber)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetPersonalNumber(personalNumber);
            SetBankAccountNumber(bankAccountNumber);
            SetBankPinNumber(bankPinNumber);
            CreationDate = DateTime.Now;
        }

        private void SetFirstName(string firstName)
        {
            if(string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException(firstName,"First name is empty!");
            FirstName = firstName;
        }

        private void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException(lastName, "Last name is empty!");
            LastName = lastName;
        }

        private void SetPersonalNumber(string personalNumber)
        {
            if (string.IsNullOrWhiteSpace(personalNumber)) throw new ArgumentNullException(personalNumber, "Personal number is empty!");
            if(personalNumber.Length != 13) throw new ArgumentException("Personal number must be string of 13 character length!", personalNumber);
            if(!IsPersonAdult(personalNumber)) throw new ArgumentException("Person is not Adult!",personalNumber);
            PersonalNumber = personalNumber;
        }

        private void SetBankAccountNumber(string bankAccountNumber)
        {
            if (string.IsNullOrWhiteSpace(bankAccountNumber)) throw new ArgumentNullException(bankAccountNumber, "Bank account number is empty!");
            BankAccountNumber = bankAccountNumber;
        }

        private void SetBankPinNumber(string bankPinNumber)
        {
            if (string.IsNullOrWhiteSpace(bankPinNumber)) throw new ArgumentNullException(bankPinNumber, "Bank pin number is empty!");
            if(bankPinNumber.Length != 4)throw new ArgumentException("Bank pin number must be 4 character length!", bankPinNumber);
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
                return age > ageLimit;
            }

            return true;
        }

        public void SetUserPass(string userPass)
        {
            UserPass = userPass;
        }
    }
}
