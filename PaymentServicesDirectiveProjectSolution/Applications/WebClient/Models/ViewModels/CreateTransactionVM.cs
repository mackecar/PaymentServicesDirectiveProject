using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Applications.WebClient.Models.ViewModels
{
    public class CreateTransactionVM
    {
        [Display(Name = "JMBG")]
        public string PersonalNumber { get; set; }

        [Display(Name = "Lozinka")]
        public string UserPass { get; set; }

        [Display(Name = "Iznos")]
        public decimal Amount { get; set; }
    }
}
