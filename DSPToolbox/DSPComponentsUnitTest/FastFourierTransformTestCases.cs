﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSPAlgorithms.Algorithms;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace DSPComponentsUnitTest
{
    [TestClass]
    public class FastFourierTransformTestCases
    {
        //input : Signal2_FFT
        //output : Signal2_FFT_Result
        [TestMethod]
        public void FFT_TestMethod1()
        {
            FastFourierTransform FFT = new FastFourierTransform();
            // test case 1 ..
            var sig1 = UnitTestUtitlities.LoadSignal("TestingSignals/Signal2_FFT.ds");
            var expectedOutput = UnitTestUtitlities.LoadSignal("TestingSignals/Signal2_FFT_Results.ds");

            FFT.InputTimeDomainSignal = sig1;
            FFT.InputSamplingFrequency = 360;


            Stopwatch sw = new Stopwatch();
            sw.Start();

            FFT.Run();

            sw.Stop();
            TimeSpan time = sw.Elapsed;

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.FrequenciesAmplitudes, FFT.OutputFreqDomainSignal.FrequenciesAmplitudes) 
                && UnitTestUtitlities.SignalsPhaseShiftsAreEqual(expectedOutput.FrequenciesPhaseShifts, FFT.OutputFreqDomainSignal.FrequenciesPhaseShifts));
        }

        //input : Signal1_FFT
        //output : Signal1_FFT_Result

        [TestMethod]
        public void FFT_TestMethod2()
        {
            FastFourierTransform FFT = new FastFourierTransform();
            // test case 1 ..
            var sig1 = UnitTestUtitlities.LoadSignal("TestingSignals/Signal1_FFT.ds");
            var expectedOutput = UnitTestUtitlities.LoadSignal("TestingSignals/Signal1_FFT_Results.ds");

            FFT.InputTimeDomainSignal = sig1;
            FFT.InputSamplingFrequency = 360;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            FFT.Run();

            sw.Stop();
            TimeSpan time = sw.Elapsed;



            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(expectedOutput.FrequenciesAmplitudes, FFT.OutputFreqDomainSignal.FrequenciesAmplitudes)
               && UnitTestUtitlities.SignalsPhaseShiftsAreEqual(expectedOutput.FrequenciesPhaseShifts, FFT.OutputFreqDomainSignal.FrequenciesPhaseShifts));
        }
        
        [TestMethod]
        public void FFT_TestMethod3()
        {
            FastFourierTransform FFT = new FastFourierTransform();
            // test case 2
            List<float> Samples = new List<float> { 1, 3, 5, 7, 9, 11, 13, 15 };
            FFT.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(Samples, false);
            FFT.InputSamplingFrequency = 4;

            var FrequenciesAmplitudes = new List<float> { 64, 20.9050074380220f, 11.3137084989848f, 8.65913760233915f, 8, 8.65913760233915f, 11.3137084989848f, 20.9050074380220f };
            var FrequenciesPhaseShifts = new List<float> { 0, 1.96349540849362f, 2.35619449019235f, 2.74889357189107f, -3.14159265358979f, -2.74889357189107f, -2.35619449019235f, -1.96349540849362f };
            var Frequencies = new List<float> { 0, 1, 2, 3, 4, 5, 6, 7 };

            FFT.Run();

            Assert.IsTrue(UnitTestUtitlities.SignalsSamplesAreEqual(FrequenciesAmplitudes, FFT.OutputFreqDomainSignal.FrequenciesAmplitudes)
                && UnitTestUtitlities.SignalsPhaseShiftsAreEqual(FrequenciesPhaseShifts, FFT.OutputFreqDomainSignal.FrequenciesPhaseShifts));
        }
    }
}
