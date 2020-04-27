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

    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputSignal1">Signal to be transformed</param>
        /// <param name="conjuagte">mark as true if you want to output the comjugate</param>
        /// <returns>Signal Transformed to Fourier Series as complex numbers</returns>
        public List<Complex> toFourier(Signal InputSignal1, bool conjuagte)
        {
            List<Complex> Signal1 = new List<Complex>();

            FastFourierTransform FFT = new FastFourierTransform();
            FFT.InputSamplingFrequency = 1;

            FFT.InputTimeDomainSignal = InputSignal1;
            FFT.Run();

            for (int i = 0; i < FFT.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex temp = new Complex();
                temp.real = FFT.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Cos(FFT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                temp.imag = FFT.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Sin(FFT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                if (conjuagte) temp.imag *= -1;

                Signal1.Add(temp);
            }
            return Signal1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Signal1">First Complex List</param>
        /// <param name="Signal2">Second Complex List</param>
        /// <returns>Multiplication of two Complex Lists</returns>
        public List<Complex> Multipy(List<Complex> Signal1, List<Complex> Signal2)
        {
            List<Complex> output = new List<Complex>();
            int end = Math.Max(Signal1.Count, Signal2.Count);
            
            for(int i=0; i<end; i++)
            {
                Complex a = (i >= Signal1.Count) ? new Complex(0,0) : Signal1[i];
                Complex b = (i >= Signal2.Count) ? new Complex(0,0) : Signal2[i];

                output.Add(a * b);
            }
            return output;
        }

        public Signal Inverse(List<Complex> Signal, bool Periodic)
        {
            Signal FreqDomain = new Signal(Periodic, new List<float>(), new List<float>(),new List<float>());

            //get phase and amplitude and construct freq domain signal
            for (int i = 0; i < Signal.Count; i++)
            {
                double a = Signal[i].real;
                double b = Signal[i].imag;

                float Amplitude = (float)Math.Sqrt(a*a + b*b);
                FreqDomain.FrequenciesAmplitudes.Add(Amplitude);

                float theta = (float)Math.Atan2(b,a);
                FreqDomain.FrequenciesPhaseShifts.Add(theta);

            }

            //get IFFT of signal
            InverseFastFourierTransform IFFT = new InverseFastFourierTransform();
            IFFT.InputFreqDomainSignal = FreqDomain;
            IFFT.Run();

            return IFFT.OutputTimeDomainSignal;
        }

        public Signal divideByN(Signal Inv)
        {
            float N = 1 / (float)Inv.Samples.Count;
            MultiplySignalByConstant M = new MultiplySignalByConstant();
            M.InputSignal = Inv;
            M.InputConstant = N;
            M.Run();

            return M.OutputMultipliedSignal;
        }


        public Signal Correlation(Signal InputSignal1, Signal InputSignal2)
        {
            FastFourierTransform FFT = new FastFourierTransform();
            FFT.InputSamplingFrequency = 1;

            List<Complex> Signal1 = new List<Complex>();
            List<Complex> Signal2 = new List<Complex>();

            bool Periodic = InputSignal1.Periodic;

            //FFT for Signal 1 Conjugate
            Signal1 = toFourier(InputSignal1, true);

            //FFT for Signal 2 
            Signal2 = toFourier(InputSignal2, false);

            //Multiply two signals
            List<Complex> Mult = Multipy(Signal1, Signal2);

            //Get Inverse
            Signal Inv = Inverse(Mult, Periodic);

            //Divide all Samples by N
            Signal S = divideByN(Inv);

            return S;
        }
       

        public List<float> Normalize(Signal InputSignal1,Signal InputSignal2, Signal S )
        {
            float sum1 = 0, sum2 = 0;
            for(int i=0;i<S.Samples.Count;i++)
            {
                float a = (i >= InputSignal1.Samples.Count) ? 0: InputSignal1.Samples[i];
                float b = (i >= InputSignal2.Samples.Count) ? 0: InputSignal2.Samples[i];
                sum1 += a * a;
                sum2 += b * b;

            }
            float N = S.Samples.Count;

            float norm = (1 / N) * (float) Math.Sqrt(sum1 * sum2);

            List<float> NormSignal = new List<float>();
            for(int i = 0; i < S.Samples.Count; i++)
            {
                NormSignal.Add(S.Samples[i] / norm);
            }

            return NormSignal;
        }
        public override void Run()
        {

            /*// we may need to add zeros enough to make both lengthes n1+n2+1
            if (InputSignal1.Samples.Count != InputSignal2.Samples.Count && InputSignal2 != null)
            {
                for (int i = 0; i < InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1; i++)
                {
                    if (i >= InputSignal1.Samples.Count) InputSignal1.Samples.Add(0);
                    if (i >= InputSignal2.Samples.Count) InputSignal2.Samples.Add(0);

                }
            }*/

            //Cross Correlation
            if (InputSignal2 != null)
            {

                Signal S = Correlation(InputSignal1, InputSignal2);

                OutputNonNormalizedCorrelation = S.Samples;

                OutputNormalizedCorrelation = Normalize(InputSignal1, InputSignal2, S);
            }
            else if (InputSignal2 == null)
            {
                Signal S = Correlation(InputSignal1, InputSignal1);

                OutputNonNormalizedCorrelation = S.Samples;

                OutputNormalizedCorrelation = Normalize(InputSignal1, InputSignal1, S);

            }
        }
    }
}