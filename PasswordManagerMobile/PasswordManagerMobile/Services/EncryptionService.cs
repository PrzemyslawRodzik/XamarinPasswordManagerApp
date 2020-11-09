using PasswordManagerApp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerApp.Services
{

    public class EncryptionService
    {
        

       
        public string Encrypt(string key, string data) 
        {
            using (Aes myAes = Aes.Create())
            {
                myAes.Key = dataToSHA256(key);
                myAes.Mode = CipherMode.CBC;
                myAes.Padding = PaddingMode.PKCS7;
                
                return AESHelper.EncryptAES(data, myAes.Key);
            }   
        }
        
        public string Decrypt(string key, string encData)
        {
            using (Aes myAes = Aes.Create())
            {
                myAes.Key = dataToSHA256(key);
                myAes.Mode = CipherMode.CBC;
                myAes.Padding = PaddingMode.PKCS7;

                return AESHelper.DecryptAES(encData, myAes.Key);
            }   
        }
        




        private  byte[] dataToSHA256(string data)
        {
            SHA256 mysha256 = SHA256.Create();
            return mysha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
        


    }




    

    


    
}

