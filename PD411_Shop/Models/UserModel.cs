using Microsoft.AspNetCore.Identity;

namespace PD411_Shop.Models
{
    public class UserModel: IdentityUser
    {
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? Image {  get; set; }
    }
}


//Identity doesn't know about this model we need to include it to the migration,