using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace iFinco.UI.Util
{
    public class EncryptionManager
    {
        public static string GenerateSecurityStamp()
        {
            //byte[] buf = new byte[16];

            byte[] buf = new byte[32];
            (new System.Security.Cryptography.RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }
        public static bool Validate(string HashedText, string secuirtyStamp, string PlainText)
        {
            bool isValid = false;
            if (!string.IsNullOrEmpty(secuirtyStamp))
            {
                string _hashedText = HashText(PlainText, secuirtyStamp);
                isValid = (_hashedText == HashedText);
            }
            return isValid;
        }
        public static string HashText(string PlanText, string secuirtyStamp)
        {
            byte[] bIn = System.Text.Encoding.Unicode.GetBytes(PlanText);
            byte[] bSalt = Convert.FromBase64String(secuirtyStamp);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;


            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);


            //HashAlgorithm s = HashAlgorithm.Create("SHA1");
            //bRet = s.ComputeHash(bAll);

            System.Security.Cryptography.SHA256Managed hashstring = new System.Security.Cryptography.SHA256Managed();
            bRet = hashstring.ComputeHash(bAll);

            return Convert.ToBase64String(bRet);
        }
    }
}