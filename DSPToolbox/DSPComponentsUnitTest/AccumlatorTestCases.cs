using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSPAlgorithms.Algorithms;
using DSPAlgorithms.DataStructures;
using DSPAlgorithms;
namespace DSPComponentsUnitTest
{
    /// <summary>
    /// Summary description for ConvolutionTestCases
    /// </summary>
    [TestClass]
    public class AccumlatorTestCases
    {
        [TestMethod]
        public void AccumlatorTestMethod1()
        {
            Accumulator a = new Accumulator();

            // test case 1 ..
            var expectedOutput = new Signal(new List<float>() { 1,3,6,10,15 }, false);

            a.InputSignal = new Signal(new List<float>() { 1, 2,3,4,5 }, false);

            a.Run();

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.Samples, a.OutputSignal.Samples));

        }
        [TestMethod]
        public void AccumlatorTestMethod2()
        {
            Accumulator a = new Accumulator();
            // test case 1 ..
            var expectedOutput = new Signal(new List<float>() { 5, 11, 18 ,26 ,35 }, false);

            a.InputSignal = new Signal(new List<float>() { 5,6,7,8,9 }, false);

            a.Run();

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.Samples, a.OutputSignal.Samples));

        }


    }
}
