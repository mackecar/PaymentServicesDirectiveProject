using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Applications.WebClient.Models.ViewModels.UserVMS
{
    public class ChangeUserPassVM
    {
        [Display(Name = "JMBG")]
        public string PersonalNumber { get; set; }

        [Display(Name = "Stara Lozinka")]
        public string OldUserPass { get; set; }

        [Display(Name = "Nova Lozinka")]
        public string NewUserPass { get; set; }
    }
}
