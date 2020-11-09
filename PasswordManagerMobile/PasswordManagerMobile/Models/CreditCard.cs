using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerMobile.Models
{
    
    public class CreditCard: UserRelationshipModel
    {
        
        public int Id { get; set; }
        
        
        public string Name { get; set; }
        
        public string CardHolderName { get; set; }
       
        public string CardNumber { get; set; }
       
        public string SecurityCode { get; set; }
        
        public string ExpirationDate { get; set; }

        

    }
}
