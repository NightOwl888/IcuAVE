﻿using Xunit;

namespace IcuAVE.xUnit
{
    public class AVETest
    {
        [Fact]
        public void TestAccessViolationException()
        {
            new IcuAVE.Test().RunTest();
        }
    }
}
