using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageCrypt
{
    [TestClass]
    public class MessageCrypt
    {
        public string AddRandomCharsToStringArray(string message, int charsToAdd)
        {

            for (int i = 0; i < charsToAdd; i++)
            {
                Random random = new Random();
                int asciiCode = random.Next(97, 122);
                message= message + asciiCode.ToString();
                
            }
            return message;
        }
        public string[] EncryptMessage(string message, int cryptKey) {

            int numberOfRows = (int)(message.Length / cryptKey);
            int reminder = (int)(message.Length % cryptKey);
           

            if (reminder != 0)
            {
                AddRandomCharsToStringArray(message, reminder);
                numberOfRows = numberOfRows + 1;
            }
            string[] result = new string[numberOfRows] ;
            for (int i = 0; i< numberOfRows; i++)
            {
                
                for (int j = 0; j < cryptKey; j++)
                {
                    if (j == 0)
                    {
                        result[i] = result[i] + message[i];
                    }
                    else
                    {
                        int index = j * numberOfRows + 1;
                        result[i] = result[i] + message[index];
                    }
                    
                    
                }              
            }
            return result;
        }


        [TestMethod]
        public void EncryptTest1()
        {
            string message = "Superbowl starts tomorrow";
            int cryptKey = 4;
            string[] expectedResult = new string[7] { "swtr", "ulsr", "p  o", "estw", "rtox", "bamy", "oroz" };

            string[] result = EncryptMessage(message, cryptKey);

            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
