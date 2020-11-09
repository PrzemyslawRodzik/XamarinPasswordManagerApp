
using System;




namespace PasswordManagerMobile.Models
{   
    public class PaypallAcount : UserRelationshipModel
    {   
        public int Id { get; set; }

       
        public string Email { get; set; }

        
        public string Password { get; set; }

       
        public int Compromised { get; set; }

        
        public DateTime ModifiedDate { get; set; }

        
    }
}
