using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Accumulator : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), new List<int>(), false);

            OutputSignal.SamplesIndices = InputSignal.SamplesIndices;
            OutputSignal.Samples = new List<float>();
            float sum = 0;
            for(int i=0;i<InputSignal.Samples.Count;i++)
            {
                sum += InputSignal.Samples[i];
                OutputSignal.Samples.Add(sum);
            }
        }
    }
}
