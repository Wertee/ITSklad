using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkladIdentity.ViewModels
{
    public class LoginViewModel
    {
        [Required] 
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}