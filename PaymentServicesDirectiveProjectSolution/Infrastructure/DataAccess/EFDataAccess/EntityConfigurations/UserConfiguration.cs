using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EFDataAccess.EntityConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FirstName).HasMaxLength(200);
            builder.Property(p => p.LastName).HasMaxLength(200);
            builder.Property(p => p.PersonalNumber).HasMaxLength(13);
            builder.Property(p => p.BankName).HasMaxLength(50);
            builder.Property(p => p.BankAccountNumber).HasMaxLength(50);
            builder.Property(p => p.BankPinNumber).HasMaxLength(4);
            builder.Property(p => p.UserPass).HasMaxLength(6);
        }
    }
}
