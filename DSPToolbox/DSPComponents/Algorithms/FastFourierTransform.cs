using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{


    public class FastFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public int InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public Complex[] FFTOutput = new Complex[0];

        public void FFT(ref List<Complex> samples, int N)
        {
            

            

            if (N==1)
            {
                return;
            }


            List<Complex> even = new List<Complex>();
            List<Complex> odd = new List<Complex>();
            for (int i=0; i<N; i+=2) { even.Add(samples[i]); }
            for (int i = 1; i < N; i += 2) { odd.Add(samples[i]); }

            FFT(ref even, N/2);
            FFT(ref odd, N/2);

            

            for (int k=0; k<(N/2); k++)
            {
                double angle = (-2 * Math.PI * k)*1.0 / N*1.0;
                double real = Math.Cos(angle);
                double imag = Math.Sin(angle);
                Complex a = new Complex(real, imag);

                Complex t = a * odd[k];
                samples[k] = even[k] + t;
                samples[k + N / 2] = even[k] - t;



            }

            return;

            




        }

        public override void Run()
        {
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
            OutputFreqDomainSignal.Frequencies = new List<float>();

            List<Complex> samples = new List<Complex>();
            for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
                samples.Add(new Complex(InputTimeDomainSignal.Samples[i], 0));

            FFT(ref samples, samples.Count);

            FFTOutput = samples.ToArray();

            float A;
            float theta;

            double omega = (double)(2 * Math.PI / (FFTOutput.Length / InputSamplingFrequency));
            double Romega = omega;


            for (int i = 0; i < FFTOutput.Length; i++)
            {
                A = (float)Math.Sqrt(FFTOutput[i].real * FFTOutput[i].real + FFTOutput[i].imag * FFTOutput[i].imag);
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add(A);
                theta = (float)Math.Atan2(FFTOutput[i].imag, FFTOutput[i].real);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add(theta);

                OutputFreqDomainSignal.Frequencies.Add((float)Romega);
                Romega += omega;

            }
        }
    }
}
