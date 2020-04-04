using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; } // x
        public Signal InputSignal2 { get; set; } // h
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            OutputConvolvedSignal = new Signal(new List<float>(), new List<int>(), false);
            // OutputConvolvedSignal.SamplesIndices = new List<int>();
            // OutputConvolvedSignal.Samples = new List<float>();

            if(InputSignal1.SamplesIndices.Count == 0)
            {
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    InputSignal1.SamplesIndices.Add(i);
            }

            if (InputSignal2.SamplesIndices.Count == 0)
            {
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    InputSignal1.SamplesIndices.Add(i);
            }

            bool Stop = true;
            List<float> y = new List<float>();
            int xdif = 0 - InputSignal1.SamplesIndices[0]; //use +
            int hdif = 0 - InputSignal2.SamplesIndices[0]; //use +

            /*int minInd = Math.Min(InputSignal1.SamplesIndices[0], InputSignal2.SamplesIndices[0]);
            int maxInd = Math.Max(InputSignal1.SamplesIndices[InputSignal1.Samples.Count - 1],
                                    InputSignal2.SamplesIndices[InputSignal2.Samples.Count - 1]);
                                    */

            int minInd = InputSignal1.SamplesIndices[0]+InputSignal2.SamplesIndices[0];
            int maxInd = InputSignal1.SamplesIndices[InputSignal1.Samples.Count - 1] +
                                    InputSignal2.SamplesIndices[InputSignal2.Samples.Count - 1];

            //int n = InputSignal1.SamplesIndices[0]; 
            int n = minInd;

            while (Stop)
            {
              
                float summation = 0;
                //bool end = true;
                //bool xout = false;
                //bool hout = false;
                //foreach(int k in InputSignal1.SamplesIndices)
                //for(int k = InputSignal1.SamplesIndices[0]; k<=n; k++) //signal 2
                for(int k=minInd; k<=maxInd;k++)
                {
                    float X; 
                    float h;
                    if (k + xdif >= InputSignal1.Samples.Count || k + xdif < 0)
                        X = 0;
                    else
                    {
                        X = InputSignal1.Samples[k + xdif];
                        //end = false;
                        //xout = true;
                    }

                    if ((n - k) + hdif >= InputSignal2.Samples.Count || (n - k) + hdif < 0)
                        h = 0;
                    else
                    {
                        h = InputSignal2.Samples[(n - k) + hdif];
                        //end = false;
                        //hout = true;
                    }

                    summation += X * h;
                }

                
                if (summation == 0 &&
                   //&& InputSignal1 //end &&   
                    y.Count > InputSignal1.Samples.Count && 
                    y.Count > InputSignal2.Samples.Count)
                {
                    Stop = false;
                }
                else
                {
                    y.Add(summation);
                    OutputConvolvedSignal.SamplesIndices.Add(n);
                   // end = true;
                }

                n++;
            }

            /*for(int i = InputSignal1.Samples.Count-1; i>=0; i++)
            {
                if (InputSignal1.Samples[i] != 0) break;
                else
                {
                    y.Add(0);
                    OutputConvolvedSignal.SamplesIndices.Add(y[y.Count - 1] + 1);
                }
            }*/
          

            OutputConvolvedSignal.Samples = y;
            for(int i=0;i<y.Count;i++)
            {
                y[i] = y[i];
            }
        }
    }
}
