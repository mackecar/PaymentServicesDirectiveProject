using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Banks.ApplicationServiceInterfaces.DTOs;

namespace Banks.ApplicationServiceInterfaces
{
    public interface IBankService
    {
        Task<CheckStatusDto> CheckStatusAsync(string personalNumber, string bankPinNumber);
        Task<DepositDto> DepositAsync(string personalNumber, string bankPinNumber, decimal amount);
        Task<WithdrawDto> WithdrawAsync(string personalNumber, string bankPinNumber, decimal amount);
    }
}
