using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Identity.Models.AppUser;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Identity.Models
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public Country Country { get; set; }

        public int Age { get; set; }

        [Required]
        public string Salary { get; set; }


        public IFormFile ProfileImage { get; set; }

    }
}
