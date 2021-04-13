using System;
using System.Collections.Generic;
using System.Text;

namespace Banks.ApplicationServiceInterfaces
{
    public interface IBankServiceProvider
    {
        void Add(string bankName, IBankService bankProviderService);
        IBankService Get(string bankName);
    }
}
