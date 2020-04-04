using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            if (InputSignal1.Samples.Count < InputSignal2.Samples.Count)
            {
                for (int i=0; i<InputSignal2.Samples.Count - InputSignal1.Samples.Count; i++)
                {
                    InputSignal1.Samples.Add(0.0f);
                }

            }

            if (InputSignal2.Samples.Count < InputSignal1.Samples.Count)
            {
                for (int i = 0; i < InputSignal1.Samples.Count - InputSignal2.Samples.Count; i++)
                {
                    InputSignal2.Samples.Add(0.0f);
                }

            }

            OutputSignal = InputSignal1;
            for (int i =0; i<InputSignal1.Samples.Count; i++)
            {
                OutputSignal.Samples[i] -= InputSignal2.Samples[i];
            }

            //throw new NotImplementedExceptSamplesion();
        }
    }
}