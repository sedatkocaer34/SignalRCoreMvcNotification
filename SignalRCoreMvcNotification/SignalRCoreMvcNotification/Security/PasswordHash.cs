using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
namespace SignalRCoreMvcNotification.Security
{
    public class PasswordHash : IPasswordHash
    {
        public string Hash(string hashText)
        {
            if (hashText != null)
            {
                return BC.HashPassword(hashText);
            }

            throw new Exception("Hash Text Not Be Empty");
        }

        public bool VerifHash(string text, string hashText)
        {
            if (text != null && hashText != null)
            {
                return BC.Verify(text, hashText);
            }
            return false;
        }
    }
}
