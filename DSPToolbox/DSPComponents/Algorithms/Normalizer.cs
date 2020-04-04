using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            OutputNormalizedSignal = InputSignal;
            float maxi = -1000000;
            float mini = 1000000;

            for (int i = 0; i < OutputNormalizedSignal.Samples.Count; i++)
            {
                if (OutputNormalizedSignal.Samples[i] > maxi)
                    maxi = OutputNormalizedSignal.Samples[i];
            }


            for (int i = 0; i < OutputNormalizedSignal.Samples.Count; i++)
            {
                if (OutputNormalizedSignal.Samples[i] < mini)
                    mini = OutputNormalizedSignal.Samples[i];
            }


            for (int i = 0; i < OutputNormalizedSignal.Samples.Count; i++)
            {
                OutputNormalizedSignal.Samples[i] = ((OutputNormalizedSignal.Samples[i] - mini)*1.0f) / ((maxi - mini)*1.0f);
                OutputNormalizedSignal.Samples[i] = InputMinRange + (OutputNormalizedSignal.Samples[i] * (InputMaxRange - InputMinRange));
            }

            //throw new NotImplementedException();
        }
    }
}
