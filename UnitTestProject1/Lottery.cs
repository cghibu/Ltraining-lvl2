using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lottery
{
    [TestClass]
    public class Lottery
    {
        public ulong CalculateFactorial(uint number)
        {
            if (number > 20)
            {
                return 0;
            }
            if (number == 0)
            {
                return 1;
            }
            ulong result = 1;
            for (int i= (int)number; i> 0;i--)
            {
                result = result * (ulong)i;
            }
            return result;
        }
        public ulong MultiplySequence(uint sequenceStart, uint sequenceLength)
        {
            ulong result = sequenceStart;
            for (int i = 1; i < sequenceLength; i++) {

                result = result*(sequenceStart+(ulong)i);
            }
            return result;
        }

        public float CalculateOdds(uint set, uint variant)
        {

            uint sequenceStart = set - variant+1;

            ulong result = 1/(MultiplySequence(sequenceStart, variant)/CalculateFactorial(variant));
            
            return result;
        }

        [TestMethod]
        public void SixFourtyNine()
        {
            uint variant = 6;
            uint set = 49;
            ulong expectedResult = 1/13983816;

            float actualResult = CalculateOdds(set, variant);

            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void FiveFourtyNine()
        {
            uint variant = 5;
            uint set = 49;
            ulong expectedResult = 1 / 1906884;

            float actualResult = CalculateOdds(set, variant);

            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void FourFourtyNine()
        {
            uint variant = 4;
            uint set = 49;
            ulong expectedResult = 1 / 1906884;

            float actualResult = CalculateOdds(set, variant);

            Assert.AreEqual(expectedResult, actualResult);

        }
    }
}
