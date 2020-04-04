using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {

            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            //if auto corr
            if (InputSignal2 == null)
            {
                InputSignal2 = new Signal(new List<float>(), InputSignal1.Periodic);
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    InputSignal2.Samples.Add(InputSignal1.Samples[i]);
                    InputSignal2.SamplesIndices.Add(InputSignal1.SamplesIndices[i]);
                }

                    

            }


            if (!InputSignal1.Periodic)
            {
                //if signals have different sizes
                int n1 = InputSignal1.Samples.Count;
                int n2 = InputSignal2.Samples.Count;
                if (n1 < n2)
                {
                    for (int i = 0; i < n2 - n1; i++)
                    {
                        InputSignal1.Samples.Add(0);
                        InputSignal1.SamplesIndices.Add(InputSignal1.SamplesIndices[n1 - 1] + 1);
                    }
                }
                else
                {
                    for (int i = 0; i < n1 - n2; i++)
                    {
                        InputSignal2.Samples.Add(0);
                        InputSignal2.SamplesIndices.Add(InputSignal2.SamplesIndices[n2 - 1] + 1);

                    }
                }


                //corr
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {

                    float sample = 0;

                    for (int j = 0; j < InputSignal2.Samples.Count; j++)
                    {
                        if (j + i < InputSignal2.Samples.Count)
                            sample += (InputSignal1.Samples[j] * InputSignal2.Samples[j + i]);

                    }
                    sample /= InputSignal1.Samples.Count;
                    OutputNonNormalizedCorrelation.Add(sample);

                }
            }


            else if (InputSignal1.Periodic)
            {
                //if signals have different sizes
                int n1 = InputSignal1.Samples.Count;
                int n2 = InputSignal2.Samples.Count;
                int newSize = n1 + n2 - 1;

                if (n1 != n2)
                {
                    for (int i = n1; i < newSize; i++)
                    {
                        InputSignal1.Samples.Add(0);
                        InputSignal1.SamplesIndices.Add(InputSignal1.SamplesIndices[n1 - 1] + 1);
                    }
                    for (int i = n2; i < newSize; i++)
                    {
                        InputSignal2.Samples.Add(0);
                        InputSignal2.SamplesIndices.Add(InputSignal2.SamplesIndices[n2 - 1] + 1);

                    }
                }

                //corr
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {

                    float sample = 0;

                    for (int j = 0; j < InputSignal2.Samples.Count; j++)
                    {
                        int index = j + i;
                        if (index >= InputSignal2.Samples.Count)
                            index -= InputSignal2.Samples.Count;
                        sample += (InputSignal1.Samples[j] * InputSignal2.Samples[index]);

                    }
                    sample /= InputSignal1.Samples.Count;
                    OutputNonNormalizedCorrelation.Add(sample);

                }








            }

            //calc normalized value
            float norm1 = 0;
            for (int i = 0; i < InputSignal1.Samples.Count; i++) norm1 += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);

            float norm2 = 0;
            for (int i = 0; i < InputSignal2.Samples.Count; i++) norm2 += (InputSignal2.Samples[i] * InputSignal2.Samples[i]);

            float norm = (1 / (float)InputSignal2.Samples.Count) * (float)(Math.Sqrt(norm1 * norm2));

            //update samples in norm
            for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++) OutputNormalizedCorrelation.Add(OutputNonNormalizedCorrelation[i] / (float)norm); 

        }
    }
}