using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using System.Data;


namespace PasswordManagerMobile.Models
{
   
    public class Note: UserRelationshipModel
    {
       
        public int Id { get; set; }

        
        public string Name { get; set; }

        

        public string Details { get; set; }

       

    }
}
