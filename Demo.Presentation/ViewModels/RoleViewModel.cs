using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Presentation.ViewModels
{
    public class RoleViewModel
    {
        public string Id {  get; set; }
        [Required(ErrorMessage ="Role Name is required")]
        public string RoleName { get; set; }


        /// nfs el kalam lw 7bet t3ml create user
        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
