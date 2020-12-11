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
        [InlineData("��������", null)]
        [InlineData("��������", "")]
        [InlineData("����", "����")]
        [InlineData("������", "������")]
        [InlineData("������", "������")]
        public void IsKeyValid(string expectedKey, string key)
        {
            machine.Key = key;
            Assert.Equal(expectedKey, machine.Key);
        }
        
        [Theory]
        [InlineData("Invalid Key: has to be all russian letters", "englishstr")]
        [InlineData("Invalid Key: has to be all letters", "123����")]
        [InlineData("Invalid Key: has to be all letters", "���4��")]
        [InlineData("Invalid Key: has to be all letters", "!���4��")]
        [InlineData("Invalid Key: has to be all letters", "��� ��")]
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
        [InlineData("������������", "������������")]
        [InlineData("����� � ���������", "����� � ���������")]
        [InlineData("!!@@#iiii �����", "!!@@#iiii �����")]
        [InlineData("��������", "��������")]
        [InlineData("", "")]
        public void EncryptTest(string expected, string passed)
        {
            machine.Key = "��������";
            Assert.Equal(expected, machine.Encrypt(passed.ToCharArray()));
        }

        [Theory]
        [InlineData("������������", "������������")]
        [InlineData("����� � ���������", "����� � ���������")]
        [InlineData("!!@@#iiii �����", "!!@@#iiii �����")]
        [InlineData("��������", "��������")]
        [InlineData("", "")]
        public void DecryptTest(string passed, string expected)
        {
            machine.Key = "��������";
            Assert.Equal(expected, machine.Decrypt(passed.ToCharArray()));
        }

        [Theory]
        [InlineData("������ �����")]
        [InlineData("������ different �����!")]
        public void MixedCryptingTest(string passed)
        {
            machine.Key = "��������";
            Assert.Equal(passed, machine.Decrypt(machine.Encrypt(machine.Decrypt(machine.Encrypt(passed.ToCharArray())))));
        }
        
    }
}
