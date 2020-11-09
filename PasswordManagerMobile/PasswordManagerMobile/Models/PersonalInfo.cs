using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace PasswordManagerMobile.Models
{
    
    public class PersonalInfo:UserRelationshipModel
    {
        
        public int Id { get; set; }

       
        public string Name { get; set; }

        
        public string SecondName { get; set; }

       
        public string LastName { get; set; }

       
        public DateTime DateOfBirth { get; set; }


        


    }
}
