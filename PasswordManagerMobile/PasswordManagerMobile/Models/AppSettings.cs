using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManagerMobile.Models
{
    public class AppSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string PublicKey { get; set; }
        public string SecretEncryptionKey { get; set; }
        public int Expiration { get; set; }
        public string DefaultSecretKey { get; set; }
        public string IpLocationApiKey { get; set; }
    }
}
