using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }


        public List<double> a = new List<double>();
        public List<double> b = new List<double>();


        public double compute(int n, int k, int N)
        {
            return (double)(-2 * k * n * Math.PI)*1.0f / N*1.0f;

        }

        public override void Run()
        {


            int k = 0;
            for (int i=0; i<InputTimeDomainSignal.Samples.Count; i++)
            {
                double sin = 0;
                double cos = 0;
                int n = 0;
                double angle;
                for (int j=0; j<InputTimeDomainSignal.Samples.Count; j++)
                {
                    angle = compute(n, k, InputTimeDomainSignal.Samples.Count);
                    
                    if (angle<0)
                    {
                        double s = Math.Sin(angle);
                        sin += (s*InputTimeDomainSignal.Samples[j]);
                    }
                    else
                    {
                        double s = Math.Sin(angle);
                        sin += (s * InputTimeDomainSignal.Samples[j]);
                    }
                    
                    double c = Math.Cos(angle);
                    cos += (c * InputTimeDomainSignal.Samples[j]);


                    n++;
                }
                a.Add(sin);
                b.Add(cos);

                k++;


            }

            double omega = (double)(2*Math.PI / (a.Count/InputSamplingFrequency));
            double Romega = omega;

            float A;
            float theta;
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
            OutputFreqDomainSignal.Frequencies = new List<float>();
            for (int i=0; i<a.Count; i++)
            {
                A = (float)Math.Sqrt(a[i] * a[i] + b[i] * b[i]);
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add(A);
                theta = (float)Math.Atan2(a[i] , b[i]);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add(theta);

                OutputFreqDomainSignal.Frequencies.Add((float)Romega);
                Romega += omega;


            }


        }
    }
}
