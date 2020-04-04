using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class RemoveDCComponent : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //DFT
            DiscreteFourierTransform DFT = new DiscreteFourierTransform();
            DFT.InputTimeDomainSignal = InputSignal;
            DFT.Run();

            List<double> real = DFT.b;
            List<double> imag = DFT.a;

            for (int i=0; i<real.Count; i++)
            {
                real[i] = real[i] - real[0];
                imag[i] = imag[i] - imag[0];
            }

            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            float A;
            float theta;
            IDFT.InputFreqDomainSignal = new Signal(new List<float>(), false);
            IDFT.InputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            IDFT.InputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
            IDFT.InputFreqDomainSignal.Frequencies = new List<float>();
            
            for (int i = 0; i < real.Count; i++)
            {
                A = (float)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
                IDFT.InputFreqDomainSignal.FrequenciesAmplitudes.Add(A);
                theta = (float)Math.Atan2(imag[i], real[i]);
                IDFT.InputFreqDomainSignal.FrequenciesPhaseShifts.Add(theta);

            }

            IDFT.Run();

            OutputSignal = IDFT.OutputTimeDomainSignal;
            

            //FFT
            /*
            FastFourierTransform FFT = new FastFourierTransform();
            FFT.InputTimeDomainSignal = InputSignal;
            FFT.Run();

            Complex[] complices = FFT.FFTOutput;

            List<double> real = new List<double>();
            List<double> imag = new List<double>();

            for (int i = 0; i < complices.Length; i++)
            {
                real.Add(complices[i].real);
                imag.Add(complices[i].imag);

                real[i] = real[i] - real[0];
                imag[i] = imag[i] - imag[0];
            }

            InverseFastFourierTransform IFFT = new InverseFastFourierTransform();
            float A;
            float theta;
            IFFT.InputFreqDomainSignal = new Signal(new List<float>(), false);
            IFFT.InputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            IFFT.InputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
            IFFT.InputFreqDomainSignal.Frequencies = new List<float>();

            for (int i = 0; i < real.Count; i++)
            {
                A = (float)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
                IFFT.InputFreqDomainSignal.FrequenciesAmplitudes.Add(A);
                theta = (float)Math.Atan2(imag[i], real[i]);
                IFFT.InputFreqDomainSignal.FrequenciesPhaseShifts.Add(theta);

            }

            IFFT.Run();

            OutputSignal = IFFT.OutputTimeDomainSignal;

            */


            /*
            //get mean;
            double mean = 0;
            for (int i=0; i<InputSignal.Samples.Count; i++)
            {
                mean += InputSignal.Samples[i];
            }
            mean = mean / InputSignal.Samples.Count;


            OutputSignal = new Signal(new List<float>(), false);
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                OutputSignal.Samples.Add((float)Math.Round(InputSignal.Samples[i]-= (float)mean, 3));
            }
            */



        }
    }
}
