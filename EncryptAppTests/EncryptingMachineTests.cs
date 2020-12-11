using EncryptApp;
using System;
using Xunit;

namespace EncryptAppTests
{
    public class EncryptingMachineTests
    {
        private readonly EncryptingMachine machine;
        public EncryptingMachineTests()
        {
            machine = new EncryptingMachine();
        }

        [Theory]
        [InlineData("скорпион", null)]
        [InlineData("скорпион", "")]
        [InlineData("ключ", "КЛЮЧ")]
        [InlineData("ключик", "кЛюЧиК")]
        [InlineData("ключик", "ключик")]
        public void IsKeyValid(string expectedKey, string key)
        {
            machine.Key = key;
            Assert.Equal(expectedKey, machine.Key);
        }
        
        [Theory]
        [InlineData("Invalid Key: has to be all russian letters", "englishstr")]
        [InlineData("Invalid Key: has to be all letters", "123ключ")]
        [InlineData("Invalid Key: has to be all letters", "клю4ик")]
        [InlineData("Invalid Key: has to be all letters", "!клю4ик")]
        [InlineData("Invalid Key: has to be all letters", "клю ик")]
        public void KeyValidationException(string expectedMsg, string key)
        {
            string previousKey = machine.Key;
            try
            {
                machine.Key = key;
                Assert.Equal(previousKey, machine.Key);
            }
            catch (Exception ex)
            {
                Assert.Equal(expectedMsg, ex.Message);
            }
        }

        [Theory]
        [InlineData("Быэввчаавщщр", "Простострока")]
        [InlineData("Дпщвв Ъ ююалуьпхч", "Текст С пробелами")]
        [InlineData("!!@@#iiii гцэтю", "!!@@#iiii слово")]
        [InlineData("СКОРПИОН", "АААААААА")]
        [InlineData("", "")]
        public void EncryptTest(string expected, string passed)
        {
            machine.Key = "скорпион";
            Assert.Equal(expected, machine.Encrypt(passed.ToCharArray()));
        }

        [Theory]
        [InlineData("Быэввчаавщщр", "Простострока")]
        [InlineData("Дпщвв Ъ ююалуьпхч", "Текст С пробелами")]
        [InlineData("!!@@#iiii гцэтю", "!!@@#iiii слово")]
        [InlineData("СКОРПИОН", "АААААААА")]
        [InlineData("", "")]
        public void DecryptTest(string passed, string expected)
        {
            machine.Key = "скорпион";
            Assert.Equal(expected, machine.Decrypt(passed.ToCharArray()));
        }

        [Theory]
        [InlineData("Всякий текст")]
        [InlineData("Всякий different текст!")]
        public void MixedCryptingTest(string passed)
        {
            machine.Key = "скорпион";
            Assert.Equal(passed, machine.Decrypt(machine.Encrypt(machine.Decrypt(machine.Encrypt(passed.ToCharArray())))));
        }
        
    }
}
