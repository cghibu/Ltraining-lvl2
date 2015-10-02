using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageCrypt
{
    [TestClass]
    public class MessageCrypt
    {
        public string AddRandomCharsToMessage(string message, int charsToAdd)
        {

            for (int i = 0; i < charsToAdd; i++)
            {
                Random random = new Random();
                int asciiCode = random.Next(97, 122);
                message = message + (char)(asciiCode);
                asciiCode = 0;
            }
            return message;
        }
        public string RemoveRandomCharsFromMessage(string message, int charsToRemove)

        {


        }
        public string[] EncryptMessage(string message, int cryptKey)
        {

            int numberOfRows = (int)(message.Length / cryptKey);
            int reminder = (int)(message.Length % cryptKey);
            if (reminder != 0)
            {
                numberOfRows = numberOfRows + 1;
                int charsToAdd = numberOfRows * cryptKey - message.Length;
                message = AddRandomCharsToMessage(message, charsToAdd);

            }
            

            string[] result = new string[numberOfRows];
            for (int i = 0; i < numberOfRows; i++)
            {

                for (int j = 0; j < cryptKey; j++)
                {
                    int index = j * numberOfRows + i;
                    result[i] = result[i] + message[index];
                 }
            }
            return result;
        }

        public string DecryptMessage(string[] message, int cryptKey)
        {

            string result = "";
            for (int i = 0; i < cryptKey; i++)
            {

                for (int j = 0; j < message.Length; j++)
                {

                    result = result + message[j][i];

                }
            }

            return result;

        }

        [TestMethod]
        public void EncryptTest1()
        {
            string message = "superbowl starts tomorrow";
            int cryptKey = 4;
            string[] expectedResult = new string[7] { "swtr", "ulsr", "p  o", "estw", "rtoo", "bamo", "oroo" };

            string[] result = EncryptMessage(message, cryptKey);

            for (int k = 0; k < 7; k++)
            {
                if (k < 4)
                {
                    Assert.AreEqual(expectedResult[k], result[k], true);
                }
                else
                {
                    for (int u = 0; u < 3; u++)
                    {
                        Assert.AreEqual(expectedResult[k][u].ToString(), result[k][u].ToString(), true);

                    }
                }
            }
        }

        [TestMethod]
        public void EncryptTest2()
        {
            string message = "superbowl starts tomorrow morning";
            int cryptKey = 5;
            string[] expectedResult = new string[7] { "swtrr", "ulsrn", "p  oi", "estwn", "rto g", "bammx", "orooy" };

            string[] result = EncryptMessage(message, cryptKey);

            for (int k = 0; k < 7; k++)
            {
                if (k < 5)
                {
                    Assert.AreEqual(expectedResult[k], result[k], true);
                }
                else
                {
                    for (int u = 0; u < 2; u++)
                    {
                        Assert.AreEqual(expectedResult[k][u].ToString(), result[k][u].ToString(), true);

                    }
                }
            }
        }
        [TestMethod]
        public void DecryptTest1()
        {
            string[] message= new string[7] { "swtrr", "ulsrn", "p  oi", "estwn", "rto g", "bammx", "orooy" };
            string expectedResult = "superbowl starts tomorrow morning";
            int cryptKey = 5;

            string result = DecryptMessage(message, cryptKey);
            Assert.AreEqual(expectedResult, result);
            
        }

    }
}