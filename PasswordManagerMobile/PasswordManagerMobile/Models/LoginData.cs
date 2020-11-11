using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace PasswordManagerMobile.Models
{
    
    public class LoginData: UserRelationshipModel
    {

       [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
        [JsonProperty("compromised")]
        public int Compromised { get; set; }
        [JsonProperty("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        public new int? UserId { get; set; }















    }
}
