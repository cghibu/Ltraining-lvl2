using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageCrypt
{
    [TestClass]
    public class MessageCrypt
    {
        public bool ValidateInput(string message, int cryptKey)
        {
            if (cryptKey <= 0)
            {
                return false;
            }
            if (message == "")
            {
                return false;
            }
            return true;
        }
        public bool ValidateOutput(string[] message, int cryptKey)
        {
            int cancel = 24;
            if (message[0][0] == (char)cancel)
            {
                return false;
            }
            if (cryptKey <= 0)
            {
                return false;
            }
            return true;
         }
        public bool VerifyIfStringNeedsTrimming(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                if ((int)message[i] == 32)
                {
                    return true;
                }
            }
            return false;
        }
        public string TrimSpacesAnywhere(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                if ((int)message[i] == 32)
                {
                    message = message.Remove(i, 1);
                }
            }
            return message;
        }
        public bool VerifyIfPaddingIsNeeded(string message, int cryptKey, out int numberOfRows, out int charsToAdd) 
        {
            numberOfRows = (int)(message.Length / cryptKey);
            int reminder = (int)(message.Length % cryptKey);
            if (reminder != 0)
            {
                numberOfRows = numberOfRows + 1;
                charsToAdd = numberOfRows * cryptKey - message.Length;
                return true;
            }
            charsToAdd = 0;
            return false;
        }
        public string AddRandomPaddingToMessage(string message, int charsToAdd)
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
        public string[] TransposeStringIntoArray(string message, int cryptKey, int numberOfRows)
        {
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
        public void AddDecryptInfo(ref string[] encryptedMessage, int charsToAdd, int numberOfRows)
        {
            if (charsToAdd >= 0)
            {
                Array.Resize<string>(ref encryptedMessage, numberOfRows + 1);
                encryptedMessage[encryptedMessage.Length - 1] = charsToAdd.ToString();
            }
        }
        public int GetDecryptInfo(ref string[] encryptedMessage)
        {
            int j = 0;
            if (int.TryParse(encryptedMessage[encryptedMessage.Length - 1], out j))
            {
                Array.Resize<string>(ref encryptedMessage, encryptedMessage.Length - 1);
                return j;
            }
            return 0;
        }
        public string TransposeArrayIntoString(string[] encryptedMessage, int cryptKey)
        {
            string result = "";
            for (int i = 0; i < cryptKey; i++)
            {
                for (int j = 0; j < encryptedMessage.Length; j++)
                {
                    result = result + encryptedMessage[j][i];
                }
            }
            return result;
        }
        public void RemoveRandomCharsFromMessage(ref string message, int charsToRemove)
        {
           message = message.Remove(message.Length - charsToRemove);
        }
        public string[] EncryptMessage(string message, int cryptKey)
        {
            if (ValidateInput(message, cryptKey))
            {
                int numberOfRows = 0;
                int charsToAdd = 0;

                if (VerifyIfStringNeedsTrimming(message))
                {
                    message = TrimSpacesAnywhere(message);
                }
                if (VerifyIfPaddingIsNeeded(message, cryptKey, out numberOfRows, out charsToAdd))
                {
                    message = AddRandomPaddingToMessage(message, charsToAdd);
                }

                string[] result = new string[numberOfRows];

                result = TransposeStringIntoArray(message, cryptKey, numberOfRows);

                AddDecryptInfo(ref result, charsToAdd, numberOfRows);

                return result;
            }
            string[] errorResult = new string[1];
            int cancel = 24;

            errorResult[0] = errorResult[0]+(char)cancel;

            return errorResult;
        }

        public string DecryptMessage(string[] encryptedMessage, int cryptKey)
        {

            if (ValidateOutput(encryptedMessage, cryptKey))
            {
                int charsToRemove = GetDecryptInfo(ref encryptedMessage);
                string result = TransposeArrayIntoString(encryptedMessage, cryptKey);

                if (charsToRemove > 0)
                {
                    RemoveRandomCharsFromMessage(ref result, charsToRemove);
                }

                return result;
             }
             string badData = "Invalid Data!";
             return badData + (char)(24);
        }

        [TestMethod]
        public void EncryptDecryptFourColumnsStringWithSpaces()
        {
            string message = "superbowl starts tomorrow";
            int cryptKey = 4;

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual("superbowlstartstomorrow",result);
         
        }

        [TestMethod]
        public void EncryptDecryptFiveColumnsStringWithoutSpaces()
        {
            string message = "superbowlstartstomorrowmorning";
            int cryptKey = 5;

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual("superbowlstartstomorrowmorning", result);
        }
        [TestMethod]
        public void EncryptDecryptTwoColumnsLongStringWithSpaces()
        {
            string message = "Oh McDonalds has a farm ia ia io and at his farm he has a cow ia ia io and a mu-mu here and a mu-mu there";
            int cryptKey = 2;

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual("OhMcDonaldshasafarmiaiaioandathisfarmhehasacowiaiaioandamu-muhereandamu-muthere", result);
        }
        [TestMethod]
        public void EncryptDecryptOneColumnLongStringWithSpaces()
        {
            string message = "Oh McDonalds has a farm ia ia io and at his farm he has a cow ia ia io and a mu-mu here and a mu-mu there";
            int cryptKey = 1;

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual("OhMcDonaldshasafarmiaiaioandathisfarmhehasacowiaiaioandamu-muhereandamu-muthere", result);
        }
        [TestMethod]
        public void EncryptDecryptManyColumnsLongStringWithSpaces()
        {
            string message = "Oh McDonalds has a farm ia ia io and at his farm he has a cow ia ia io and a mu-mu here and a mu-mu there";
            int cryptKey = 70;

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual("OhMcDonaldshasafarmiaiaioandathisfarmhehasacowiaiaioandamu-muhereandamu-muthere", result);
        }
        [TestMethod]
        public void EncryptDecryptNullKey()
        {
            string message = "Oh mamacita what a null key you have!";
            int cryptKey = 0;
            string expectedResult = "Invalid Data!" + (char)(24);

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void EncryptDecryptNullInputValidKey()
        {
            string message = "";
            int cryptKey = 4;
            string expectedResult = "Invalid Data!" + (char)(24);

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void EncryptDecryptNullInputInvalidKey()
        {
            string message = "";
            int cryptKey = 0;
            string expectedResult = "Invalid Data!" + (char)(24);

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void SimpleDecryptTest()
        {
            string[] message= new string[7] { "soro", "uwtr", "plsr", "esto", "rtow", "bamz","1"};
            int cryptKey = 4;
            string expectedResult = "superbowlstartstomorrow";
            
            string result = DecryptMessage(message, cryptKey);

            Assert.AreEqual(expectedResult, result);
            
        }

    }
}