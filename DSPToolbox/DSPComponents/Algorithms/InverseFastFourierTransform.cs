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

        public void IFFT(ref List<Complex> complices)
        {
            int N = complices.Count;

            

            if (N == 1)
            {
                return;
            }

           

            List<Complex> even = new List<Complex>();
            List<Complex> odd = new List<Complex>();
            for (int i = 0; i < N; i += 2) { even.Add(complices[i]); }
            for (int i = 1; i < N; i += 2) { odd.Add(complices[i]); }

            IFFT(ref even);
            IFFT(ref odd);



            for (int k = 0; k < (N / 2); k++)
            {
                double angle = (2 * Math.PI * k) * 1.0 / N * 1.0;
                double real = Math.Cos(angle);
                double imag = Math.Sin(angle);
                Complex an = new Complex(real, imag);

                Complex t = an * odd[k];
                complices[k] = even[k] + t;
                complices[k + N / 2] = even[k] - t;



            }

            return;
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

            IFFT(ref complices);
            for (int i=0; i< complices.Count; i++)
            {
                complices[i].real = complices[i].real/InputFreqDomainSignal.FrequenciesAmplitudes.Count;
                OutputTimeDomainSignal.Samples.Add((float)Math.Round(complices[i].real, 3));
                OutputTimeDomainSignal.SamplesIndices.Add(i);
            }



        }
    }
}
