using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {

            OutputFoldedSignal = new Signal(new List<float>(), new List<int>(), false);
            //OutputFoldedSignal.SamplesIndices = InputSignal.SamplesIndices;
            
            int N = InputSignal.Samples.Count;
            for (int i = 0; i < N; i++)
            {
                OutputFoldedSignal.Samples.Add(InputSignal.Samples[N - i - 1]);
                OutputFoldedSignal.SamplesIndices.Add(-1*InputSignal.SamplesIndices[N - i - 1]);
            }
            if (InputSignal.Periodic)
            {
                OutputFoldedSignal.Periodic = false;
            }
            else
            {
                OutputFoldedSignal.Periodic = true;
            }
           

        }
    }
}
