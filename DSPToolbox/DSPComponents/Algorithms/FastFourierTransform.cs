using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Complex
    {
        public double real;
        public double imag;

        public Complex() { }

        public Complex(double r, double i)
        {
            real = r;
            imag = i;
        }


        public static Complex operator -(Complex a, Complex b)
        {
            Complex data = new Complex((a.real - b.real), (a.imag - b.imag));
            return data;
        }

        public static Complex operator +(Complex a, Complex b)
        {
            Complex data = new Complex((a.real + b.real), (a.imag + b.imag));
            return data;
        }

        public static Complex operator *(Complex a, Complex b)
        {
            Complex data = new Complex((a.real * b.real) - (a.imag * b.imag), (a.real * b.imag) + (a.imag * b.real));
            return data;
        }
    }

    public class FastFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public int InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public Complex[] FFTOutput = new Complex[0];

        public Complex[] FFT(List<float> samples)
        {
            int N = samples.Count;

            Complex[] ret = new Complex[N];

            if (N == 2)
            {
                
                ret[0] = new Complex(samples[0] + samples[1], 0); //even
                ret[1] = new Complex(samples[0] - samples[1], 0); //odd

                return ret;
            }



            List<float> even = new List<float>();
            List<float> odd = new List<float>();
            for (int i=0; i<N; i+=2) { even.Add(samples[i]); }
            for (int i = 1; i < N; i += 2) { odd.Add(samples[i]); }

            Complex[] fft1 = FFT(even);
            Complex[] fft2 = FFT(odd);

            

            for (int k=0; k<(N/2); k++)
            {
                double angle = (-2 * Math.PI * k)*1.0 / N*1.0;
                double real = Math.Cos(angle);
                double imag = Math.Sin(angle);
                Complex a = new Complex(real, imag);

                Complex t = a * fft2[k];
                ret[k] = fft1[k] + t;
                ret[k + N / 2] = fft1[k] - t;



            }

            return ret;

            




        }

        public override void Run()
        {
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
            OutputFreqDomainSignal.Frequencies = new List<float>();

            FFTOutput = FFT(InputTimeDomainSignal.Samples);

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
