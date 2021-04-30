using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Applications.WebClient.Models.ViewModels.UserVMS
{
    public class BlockUserVM
    {
        [Display(Name = "JMBG")]
        public string PersonalNumber { get; set; }

        [Display(Name = "Administratorka lozinka")]
        public string AdminPass { get; set; }
    }
}
