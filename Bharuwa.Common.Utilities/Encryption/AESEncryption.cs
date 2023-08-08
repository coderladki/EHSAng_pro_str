using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bharuwa.Common.Utilities.Encryption
{
    public class AESEncryption
    {
        public static String Encrypt(String key,String text)
        {

            byte[] value = UTF8Encoding.UTF8.GetBytes(text);
            byte[] crypt;
            byte[] iv;
            using (Aes myAes = Aes.Create())
            {

                myAes.KeySize = 256;
                myAes.Mode = CipherMode.CBC;
                myAes.Key = HexToBin(key);
                myAes.GenerateIV();
                myAes.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, myAes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(value, 0, value.Length);
                        cs.FlushFinalBlock();
                        crypt = ms.ToArray();
                    }
                }
                iv = myAes.IV;
                myAes.Clear();
            }
            return ByteArrayToString(crypt) + ":" + ByteArrayToString(iv);

        }

        public static string Decrypt(String key, String cipher)
        {
            String outputString = "";
            byte[] ivBytes = HexToBin(getIV(cipher));
            byte[] valBytes = HexToBin(getSSN(cipher));

            using (Aes myAes = Aes.Create())
            {
                int size = valBytes.Count();

                myAes.KeySize = 256;
                myAes.Mode = CipherMode.CBC;
                myAes.Key = HexToBin(key);
                myAes.IV = ivBytes;
                myAes.Padding = PaddingMode.PKCS7;

                char[] output = new char[256];

                ICryptoTransform myDecrypter = myAes.CreateDecryptor(myAes.Key, myAes.IV);

                using (MemoryStream memory = new MemoryStream(valBytes))
                {
                    using (CryptoStream cryptStream = new CryptoStream(memory, myDecrypter, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptStream))
                        {

                            outputString = reader.ReadToEnd();
                        }

                        return outputString;
                    }
                }

            }
        }

        private static byte[] HexToBin(String hexString)
        {


            int charCount = hexString.Length;
            byte[] output = new byte[charCount / 2];
            for (int i = 0; i < charCount; i += 2)
            {
                output[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }


            return output;
        }

        private static String getSSN(String cipher)
        {
            int delimiterIndex = cipher.IndexOf(":");
            String SSN = cipher.Substring(0, delimiterIndex);
            return SSN;
        }


        private static String getIV(String cipher)
        {
            int delimiterIndex = cipher.IndexOf(":");
            String IV = cipher.Substring(delimiterIndex + 1);
            return IV;
        } 
         
        private static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
    }
}
