using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();
    }
}
