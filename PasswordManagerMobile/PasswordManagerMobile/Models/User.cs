
using System.Collections.Generic;


namespace PasswordManagerMobile.Models
{   
    public class User
    {
        
        public int Id { get; set; }
        
        public string Email { get; set; }
        

        public string Password { get; set; }
        
        public string PasswordSalt { get; set; }
        
        public int TwoFactorAuthorization { get; set; }
        
        public int Admin { get; set; }
       
        public int PasswordNotifications { get; set; }
        
        public int AuthenticationTime { get; set; }
        


    }
}
