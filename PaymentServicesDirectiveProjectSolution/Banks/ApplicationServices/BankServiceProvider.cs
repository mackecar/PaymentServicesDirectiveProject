using System;
using System.Collections.Generic;
using System.Text;
using Banks.ApplicationServiceInterfaces;

namespace Banks.ApplicationServices
{
    public class BankServiceProvider : IBankServiceProvider
    {
        public Dictionary<string, IBankService> ServiceProvider { get; set; }

        public BankServiceProvider()
        {
            ServiceProvider = new Dictionary<string, IBankService>();
        }

        public void Add(string bankName, IBankService bankProviderService)
        {
            ServiceProvider[bankName] = bankProviderService;
        }

        public IBankService Get(string bankName)
        {
            var bank = ServiceProvider.GetValueOrDefault(bankName);
            if(bank == null) throw new NullReferenceException($"{bankName} banka nije implementirana!");
            return bank;
        }
    }
}
