using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{

    public class InverseFastFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public List<double> a = new List<double>();
        public List<double> b = new List<double>();

        public Complex[] IFFT(List<Complex> complices)
        {
            int N = complices.Count;

            Complex[] ret = new Complex[N];

            if (N == 2)
            {

                
                ret[0] = complices[0] + complices[1];
                ret[1] = complices[0] - complices[1];

                return ret;
            }

           

            List<Complex> even = new List<Complex>();
            List<Complex> odd = new List<Complex>();
            for (int i = 0; i < N; i += 2) { even.Add(complices[i]); }
            for (int i = 1; i < N; i += 2) { odd.Add(complices[i]); }

            Complex[] fft1 = IFFT(even);
            Complex[] fft2 = IFFT(odd);



            for (int k = 0; k < (N / 2); k++)
            {
                double angle = (2 * Math.PI * k) * 1.0 / N * 1.0;
                double real = Math.Cos(angle);
                double imag = Math.Sin(angle);
                Complex an = new Complex(real, imag);

                Complex t = an * fft2[k];
                ret[k] = fft1[k] + t;
                ret[k + N / 2] = fft1[k] - t;



            }

            return ret;
        }

        public override void Run()
        {
            OutputTimeDomainSignal = new Signal(new List<float>(), false);
            List<Complex> complices = new List<Complex>();
            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                a.Add(InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                b.Add(InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                complices.Add(new Complex(a[i], b[i]));
            }

            Complex[] samples = IFFT(complices);
            for (int i=0; i<samples.Length; i++)
            {
                samples[i].real = samples[i].real/InputFreqDomainSignal.FrequenciesAmplitudes.Count;
                OutputTimeDomainSignal.Samples.Add((float)Math.Round(samples[i].real, 3));
                OutputTimeDomainSignal.SamplesIndices.Add(i);
            }



        }
    }
}
