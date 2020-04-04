using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {

            int maxC = 0;
            for (int i=0; i<InputSignals.Count; i++)
            {
                if (InputSignals[i].Samples.Count > maxC)
                    maxC = InputSignals[i].Samples.Count;
            }

            for (int i = 0; i < InputSignals.Count; i++)
            {
                if (InputSignals[i].Samples.Count < maxC)
                {
                    for (int j=0; j<maxC-InputSignals[i].Samples.Count; j++)
                    {
                        InputSignals[i].Samples.Add(0);
                    }


                }
            }


            OutputSignal = InputSignals[0];
            for (int i = 0; i < InputSignals[0].Samples.Count; i++)
            {
                for (int j = 1; j < InputSignals.Count; j++)
                {

                    OutputSignal.Samples[i] += InputSignals[j].Samples[i];

                }

            }

            //throw new NotImplementedException();


        }
    }
}