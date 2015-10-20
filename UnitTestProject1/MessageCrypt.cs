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
        public bool ValidateOutput(string message, int cryptKey)
        {
            if (message == "")
            {
                return false;
            }
            if (cryptKey <= 0)
            {
                return false;
            }
            return true;
         }
        public bool VerifyIfStringNeedsCleaning(string message, out int firstFoundIndex)
        {
            for (int i = 0; i < message.Length; i++)
            {
                if (!char.IsLetter(message[i]))
                {
                    firstFoundIndex = i;
                    return true;
                }
            }
            firstFoundIndex = -1;
            return false;
        }
        public string CleanStringFromIndex(string message, int firstFoundIndex)
        {
            message = message.Remove(firstFoundIndex, 1);
            for (int i = firstFoundIndex + 1; i < message.Length; i++)
            {
                if (!char.IsLetter(message[i]))
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
        public string TransposeString(string message, int cryptKey, int numberOfRows)
        {

            

            string result = "";
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < cryptKey; j++)
                {
                    int index = j * numberOfRows + i;
                    result = result + message[index];
                }
            }
            return result;

         }
        public string RotateCharacters(string message, int cryptKey, int numberOfRows)
        {
            string  resultString= "";
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < cryptKey; j++)
                {
                    int index = j * numberOfRows + i;
                    resultString = resultString+message[index];
                } 
            }

            return resultString;
        }
        /*public string[] TransposeStringIntoArray(string message, int cryptKey, int numberOfRows)
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
        }*/
        public void AddDecryptInfo(ref string encryptedMessage, int charsToAdd, int numberOfRows)
        {
            if (charsToAdd > 0)
            {
                encryptedMessage = encryptedMessage + (char)charsToAdd;
            }
        }
        public int GetDecryptInfo(ref string encryptedMessage)
        {
            char decryptInfo= encryptedMessage[encryptedMessage.Length-1];

            if (char.IsLetter(decryptInfo))
            {
                return 0;
            }
            encryptedMessage = encryptedMessage.Remove(encryptedMessage.Length - 1,1);

            return decryptInfo;
            
            
        }
        public string RotateCharactersBack(string encryptedMessage, int cryptKey)
        {
            string result = "";
            for (int i = 0; i < cryptKey; i++)
            {
                for (int j = 0; j < encryptedMessage.Length/cryptKey; j++)
                {
                    int index = j * cryptKey + i;
                    result = result + encryptedMessage[index];
                }
            }
            return result;
        }
        public string RemoveRandomCharsFromMessage(string message, int charsToRemove)
        {
           return message.Remove(message.Length - charsToRemove);
        }
        public string EncryptMessage(string message, int cryptKey)
        {
            if (ValidateInput(message, cryptKey))
            {
                int numberOfRows = 0;
                int charsToAdd = 0;
                int firstFoundIndex = -1;

                if (VerifyIfStringNeedsCleaning(message, out firstFoundIndex))
                {
                    message = CleanStringFromIndex(message,firstFoundIndex);
                }
                if (VerifyIfPaddingIsNeeded(message, cryptKey, out numberOfRows, out charsToAdd))
                {
                    message = AddRandomPaddingToMessage(message, charsToAdd);
                }

                message = RotateCharacters(message, cryptKey, numberOfRows);

                AddDecryptInfo(ref message, charsToAdd, numberOfRows);

                return message;
            }
                       
            return message; 
        }

        public string DecryptMessage(string encryptedMessage, int cryptKey)
        {

            if (ValidateOutput(encryptedMessage, cryptKey))
            {
                string result= RotateCharactersBack(encryptedMessage, cryptKey);
                int charsToRemove = GetDecryptInfo(ref encryptedMessage);
                if (charsToRemove > 0)
                {
                    result = RemoveRandomCharsFromMessage(result, charsToRemove);
                }
                return result;
              
           }
            
             return "Error"+ (char)24;
        }

        [TestMethod]
        public void SimpleEncryptTest()
        {
            string message = "superbowl starts tomorrow";
            int cryptKey = 3;

            string result = DecryptMessage(EncryptMessage(message, cryptKey),cryptKey);

            Assert.AreEqual("superbowlstartstomorrow",result);

        }
        [TestMethod]
        public void SimpleEncryptTestThreeColumnsShortMessage()
        {
            string message = "Ana are mere albe";
            int cryptKey = 3;

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual("Anaaremerealbe", result);

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

            Assert.AreEqual("OhMcDonaldshasafarmiaiaioandathisfarmhehasacowiaiaioandamumuhereandamumuthere", result);
        }
        [TestMethod]
        public void EncryptDecryptOneColumnLongStringWithSpaces()
        {
            string message = "Oh McDonalds has a farm ia ia io and at his farm he has a cow ia ia io and a mu-mu here and a mu-mu there";
            int cryptKey = 1;

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual("OhMcDonaldshasafarmiaiaioandathisfarmhehasacowiaiaioandamumuhereandamumuthere", result);
        }
        [TestMethod]
        public void EncryptDecryptManyColumnsLongStringWithSpaces()
        {
            string message = "Oh McDonalds has a farm ia ia io and at his farm he has a cow ia ia io and a mu-mu here and a mu-mu there";
            int cryptKey = 70;

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual("OhMcDonaldshasafarmiaiaioandathisfarmhehasacowiaiaioandamumuhereandamumuthere", result);
        }
        [TestMethod]
        public void EncryptDecryptNullKey()
        {
            string message = "Oh mamacita what a null key you have!";
            int cryptKey = 0;
            string expectedResult = "Error" + (char)(24);

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void EncryptDecryptNullInputValidKey()
        {
            string message = "";
            int cryptKey = 4;
            string expectedResult = "Error" + (char)(24);

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void EncryptDecryptNullInputInvalidKey()
        {
            string message = "";
            int cryptKey = 0;
            string expectedResult = "Error" + (char)(24);

            string result = DecryptMessage(EncryptMessage(message, cryptKey), cryptKey);

            Assert.AreEqual(expectedResult, result);
        }
        
    }
}