using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Banks.ApplicationServiceInterfaces;
using Banks.ApplicationServiceInterfaces.DTOs;

namespace Banks.ApplicationServices
{
    public class DummyBankService : IBankService
    {
        public async Task<CheckStatusDto> CheckStatusAsync(string personalNumber, string bankPinNumber)
        {
            if (personalNumber == "0312984710065")
            {
                return new CheckStatusDto()
                {
                    Message = "Korisnik ne postoji u banci Dummy!",
                    Status = false
                };
            }

            return new CheckStatusDto()
            {
                Message = "Ok",
                Status = true
            };
        }

        public async Task<DepositDto> DepositAsync(string personalNumber, string bankPinNumber, decimal amount)
        {
            if (personalNumber == "0312984710065")
            {
                return new DepositDto()
                {
                    Message = "Korisnik ne postoji u banci Dummy!",
                    Status = false
                };
            }

            return new DepositDto()
            {
                Message = "Ok",
                Status = true,
                Amount = amount
            };
        }

        public async Task<WithdrawDto> WithdrawAsync(string personalNumber, string bankPinNumber, decimal amount)
        {
            if (personalNumber == "0312984710065")
            {
                return new WithdrawDto()
                {
                    Message = "Korisnik ne postoji u banci Dummy!",
                    Status = false
                };
            }

            return new WithdrawDto()
            {
                Message = "Ok",
                Status = true,
                Amount = amount
            };
        }
    }
}
