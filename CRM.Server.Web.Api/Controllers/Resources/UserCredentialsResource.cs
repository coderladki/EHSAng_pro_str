using System.ComponentModel.DataAnnotations;

namespace CRM.Server.Web.Api.Controllers.Resources
{
    public class UserCredentialsResource
    {
        //[Required]
        //[DataType(DataType.EmailAddress)]
        //[StringLength(255)]
        //public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        //[RegularExpression(@"(^(?i:([a-z])(?!\1{2,}))*$)|(^[A-Ya-y1-8]*$)", ErrorMessage = "Please Enter One Special Character One Capital Letter One Numeric Number and One alphabat")]
        [StringLength(32)]
        public string Password { get; set; }

        public int Id { get; set; }
    }
}