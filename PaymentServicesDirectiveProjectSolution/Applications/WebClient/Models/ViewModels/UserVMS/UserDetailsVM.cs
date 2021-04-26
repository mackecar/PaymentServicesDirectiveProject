using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Applications.WebClient.Models.ViewModels.UserVMS
{
    public class UserDetailsVM
    {
        [Display(Name = "JMBG")]
        public string PersonalNumber { get; set; }

        [Display(Name = "Lozinka")]
        public string UserPass { get; set; }
        public UserVM User { get; set; }
    }
}
