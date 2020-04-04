using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }


        public double compute(int n, int k, int N)
        {
            return (2 * k * n * Math.PI) * 1.0f / N * 1.0f;

        }


        public override void Run()
        {
            OutputTimeDomainSignal = new Signal(new List<float>(), false);
            List<double> a = new List<double>();
            List<double> b = new List<double>();
            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                a.Add(InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                b.Add(InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[i]));

            }


            int n = 0;
            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                double output = 0;
                int k = 0;
                double angle;
                for (int j = 0; j < InputFreqDomainSignal.FrequenciesAmplitudes.Count; j++)
                {
                    angle = compute(n, k, InputFreqDomainSignal.FrequenciesAmplitudes.Count);

                    double s = (float)Math.Sin(angle);
                    double c = (float)Math.Cos(angle);
                    output += (a[j] * c) + (b[j] * s * -1);
                    k++;
                }
                OutputTimeDomainSignal.Samples.Add((float)Math.Round(output * (1.0f / InputFreqDomainSignal.FrequenciesAmplitudes.Count*1.0f), 3));
                OutputTimeDomainSignal.SamplesIndices.Add(i);
                n++;


            }


        }
    }
}
