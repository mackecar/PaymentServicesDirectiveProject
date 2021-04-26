using System.ComponentModel.DataAnnotations;

namespace Applications.WebClient.Models.ViewModels.TransactionVMS
{
    public class CreateTransactionVM
    {
        [Display(Name = "JMBG")]
        public string PersonalNumber { get; set; }

        [Display(Name = "JMBG korisnika")]
        public string DestinationPersonalNumber { get; set; }

        [Display(Name = "Lozinka")]
        public string UserPass { get; set; }

        [Display(Name = "Iznos")]
        public decimal Amount { get; set; }
    }
}
