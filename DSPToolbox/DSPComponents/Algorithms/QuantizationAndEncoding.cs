using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            if (InputLevel != 0)
            {
                InputNumBits = Convert.ToInt32( Math.Log(InputLevel, 2));
            }
            else if (InputNumBits != 0)
            {
                InputLevel = Convert.ToInt32(Math.Pow(2, InputNumBits));
            }

            //get max of samples
            float max = 0.0f;
            for (int i=0; i<InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] > max)
                    max = InputSignal.Samples[i];
            }


            //get min of samples
            float min = 1000000.0f;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] < min)
                    min = InputSignal.Samples[i];
            }

            float interval = (max - min)*1.0f / InputLevel*1.0f;



            OutputQuantizedSignal = new Signal(new List<float>(), false);
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();
            for (int i=0; i<InputSignal.Samples.Count; i++)
            {
                float start = min;
                float end = 0;
                
                for (int j=0; j<InputLevel; j++)
                {
                    end = start + interval;
                    if ((InputSignal.Samples[i] >= start && InputSignal.Samples[i] < end )||(j == InputLevel-1))
                    {
                        float Midpoint = (start + end) * 1.0f / 2 * 1.0f;
                        OutputQuantizedSignal.Samples.Add(Midpoint);
                        OutputIntervalIndices.Add(j+1);
                        string binValue = Convert.ToString(j, 2);

                        for (int k=InputNumBits-binValue.Length; k>0; k--)
                        {
                            binValue = binValue.Insert(0, "0");
                        }

                        OutputEncodedSignal.Add(binValue);
                        OutputSamplesError.Add(Midpoint-InputSignal.Samples[i]);
                        
                        break;
                    }
                    start = end;
                }



            }
  




        }


    }
}
