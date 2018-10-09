using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLauncher.Scripts {
    public class AuthenticationService : IAuthenticationService {
        
        public User AuthenticateUser(string pUsername, string pClearTextPassword) {
            Task<string> task = Task.Run(async () => {
                return await WebServer.RetrieveAuthenticationResultJson(pUsername, pClearTextPassword);
            });
            string phpResultJson = task.Result;

            AuthenticationResult result = JsonConvert.DeserializeObject<AuthenticationResult>(phpResultJson);

            if (result.Status.ToLower() == "success") {
                return new User(pUsername, "", result.Roles); // E-mail empty for now.. TODO: Add later maybe..
            } else {
                throw new UnauthorizedAccessException(result.Status);
            }
        }

        //private string CalculateHash(string clearTextPassword, string salt) {
        //    // Convert the salted password to a byte array
        //    byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
        //    // Use the hash algorithm to calculate the hash
        //    HashAlgorithm algorithm = new SHA256Managed();
        //    byte[] hash = algorithm.ComputeHash(saltedHashBytes);
        //    // Return the hash as a base64 encoded string to be compared to the stored password
        //    return Convert.ToBase64String(hash);
        //}
    }
}

