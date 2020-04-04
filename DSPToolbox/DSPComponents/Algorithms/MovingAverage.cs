using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            OutputAverageSignal = new Signal(new List<float>(), false);
            int i = 0;
            while(i < InputSignal.Samples.Count-InputWindowSize+1)
            {
                double sum = 0;
                for (int j=i; j<Math.Min(InputSignal.Samples.Count, i + InputWindowSize); j++)
                {
                    sum += InputSignal.Samples[j];
                }
                i += 1;
                OutputAverageSignal.Samples.Add((float)sum / InputWindowSize);
                OutputAverageSignal.SamplesIndices.Add(i);
            }
        }
    }
}
