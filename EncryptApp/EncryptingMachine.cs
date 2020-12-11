using System;
using System.Linq;

namespace EncryptApp
{
    public class EncryptingMachine
    {
        private const string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private string key;
        public string Key
        {
            get
            {
                if (key == null || key == "") return "скорпион";
                return key;
            }
            set
            {
                if (value == null || value == "")
                {
                    key = "скорпион";
                }
                else if (value.All(char.IsLetter))
                {
                    if (value.ToLower().All(alphabet.Contains))
                    {
                        key = value.ToLower();
                    }
                    else
                    {
                        throw new Exception("Invalid Key: has to be all russian letters");
                    }
                }
                else
                {
                    throw new Exception("Invalid Key: has to be all letters");
                }
            }
        }

        //Methods
        public char[] Encrypt(char[] s)
        {
            return Vigenere(true, s);
        }
        public char[] Decrypt(char[] s)
        {
            return Vigenere(false, s);
        }

        //Actual encrypting/decrypting encapsulated
        private char[] Vigenere(bool encrypt, char[] chars)
        {
            char[] key = Key.ToCharArray();
            int keyIteration = 0;
            int charIteration = 0;
            while (charIteration < chars.Length)
            {
                if (char.IsLetter(chars[charIteration]) && alphabet.Contains(char.ToLower(chars[charIteration])))
                {
                    if (encrypt)
                    {
                        int index = (alphabet.IndexOf(char.ToLower(chars[charIteration])) + alphabet.IndexOf(key[keyIteration])) % alphabet.Length;
                        if (char.IsUpper(chars[charIteration]))
                        {
                            chars[charIteration] = char.ToUpper(alphabet[index]);
                        }
                        else
                        {
                            chars[charIteration] = alphabet[index];

                        }
                    }
                    else
                    {
                        int index = (alphabet.IndexOf(char.ToLower(chars[charIteration])) - alphabet.IndexOf(key[keyIteration]));
                        if (index < 0) index += alphabet.Length;
                        if (char.IsUpper(chars[charIteration]))
                        {
                            chars[charIteration] = char.ToUpper(alphabet[index]);
                        }
                        else
                        {
                            chars[charIteration] = alphabet[index];
                        }
                    }
                    keyIteration++;
                    if (keyIteration == key.Length)
                    {
                        keyIteration = 0;
                    }
                }
                charIteration++;
            }
            return chars;
        }
    }
    public class EncryptingException : Exception
    {
        public EncryptingException(string msg) : base(msg)
        {

        }
    }
}
