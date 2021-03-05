using System.ComponentModel.DataAnnotations;

namespace ServicesLib.Domain.ViewModel
{
    public class LoginVm
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
