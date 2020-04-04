using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSPAlgorithms.Algorithms;
using System.Collections.Generic;
using System.Diagnostics;
namespace DSPComponentsUnitTest
{
    [TestClass]
    public class DFTTestCases
    {
        //input : Signal2_FFT
        //output : Signal2_FFT_Result
        [TestMethod]
        public void FFT_TestMethod1()
        {
            DiscreteFourierTransform DFT = new DiscreteFourierTransform();
            // test case 1 ..
            var sig1 = UnitTestUtitlities.LoadSignal("TestingSignals/Signal2_FFT.ds");
            var expectedOutput = UnitTestUtitlities.LoadSignal("TestingSignals/Signal2_FFT_Results.ds");

            DFT.InputTimeDomainSignal = sig1;
            DFT.InputSamplingFrequency = 360;


            Stopwatch sw = new Stopwatch();
            sw.Start();
            DFT.Run();
            sw.Stop();

            TimeSpan time = sw.Elapsed;
            bool x = UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.FrequenciesAmplitudes, DFT.OutputFreqDomainSignal.FrequenciesAmplitudes);
            bool y = UnitTestUtitlities.SignalsPhaseShiftsAreEqual(expectedOutput.FrequenciesPhaseShifts, DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts);


            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.FrequenciesAmplitudes, DFT.OutputFreqDomainSignal.FrequenciesAmplitudes)
                && UnitTestUtitlities.SignalsPhaseShiftsAreEqual(expectedOutput.FrequenciesPhaseShifts, DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts));
        }

        //input : Signal1_FFT
        //output : Signal1_FFT_Result

        [TestMethod]
        public void FFT_TestMethod2()
        {
            DiscreteFourierTransform DFT = new DiscreteFourierTransform();
            // test case 1 ..
            var sig1 = UnitTestUtitlities.LoadSignal("TestingSignals/Signal1_FFT.ds");
            var expectedOutput = UnitTestUtitlities.LoadSignal("TestingSignals/Signal1_FFT_Results.ds");

            DFT.InputTimeDomainSignal = sig1;
            DFT.InputSamplingFrequency = 360;


            Stopwatch sw = new Stopwatch();
            sw.Start();

            DFT.Run();

            sw.Stop();
            TimeSpan time = sw.Elapsed;


            bool x = UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.FrequenciesAmplitudes, DFT.OutputFreqDomainSignal.FrequenciesAmplitudes);
            bool y = UnitTestUtitlities.SignalsPhaseShiftsAreEqual(expectedOutput.FrequenciesPhaseShifts, DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts);

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.FrequenciesAmplitudes, DFT.OutputFreqDomainSignal.FrequenciesAmplitudes)
               && UnitTestUtitlities.SignalsPhaseShiftsAreEqual(expectedOutput.FrequenciesPhaseShifts, DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts));
        }
        [TestMethod]
        public void DFTTestMethod3()
        {
            DiscreteFourierTransform DFT = new DiscreteFourierTransform();
            // test case 2
            List<float> Samples = new List<float> { 1, 3, 5, 7, 9, 11, 13, 15 };
            DFT.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(Samples, false);
            DFT.InputSamplingFrequency = 4;

            var FrequenciesAmplitudes = new List<float> { 64, 20.9050074380220f, 11.3137084989848f, 8.65913760233915f, 8, 8.65913760233915f, 11.3137084989848f, 20.9050074380220f };
            var FrequenciesPhaseShifts = new List<float> { 0, 1.96349540849362f, 2.35619449019235f, 2.74889357189107f, -3.14159265358979f, -2.74889357189107f, -2.35619449019235f, -1.96349540849362f };
            var Frequencies = new List<float> { 0, 1, 2, 3, 4, 5, 6, 7 };

            DFT.Run();

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(FrequenciesAmplitudes, DFT.OutputFreqDomainSignal.FrequenciesAmplitudes)
                && UnitTestUtitlities.SignalsPhaseShiftsAreEqual(FrequenciesPhaseShifts, DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts));

        }
    }
}
