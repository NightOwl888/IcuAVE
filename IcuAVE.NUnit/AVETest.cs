using NUnit.Framework;

namespace IcuAVE.NUnit
{
    public class AVETest
    {
        [Test]
        public void TestAccessViolationException()
        {
            new IcuAVE.Test().RunTest();
        }
    }
}
