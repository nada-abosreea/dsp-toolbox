using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; } // sampling freq
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; } //for bandpass or bandstop
        public float? InputF2 { get; set; } //for bandpass or bandstop
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; } //transition width
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        enum window_type
        {
            rectangular,
            hanning,
            hamming,
            blackman

        }


        window_type get_window_type()
        {

            if (InputStopBandAttenuation <= 21)
                return window_type.rectangular;
            else if (InputStopBandAttenuation <= 44)
                return window_type.hanning;
            else if (InputStopBandAttenuation <= 53)
                return window_type.hamming;
            else
                return window_type.blackman;
        }

        int computeN(window_type window_Type)
        {
            float deltaF = InputTransitionBand / InputFS;
            if (window_Type == window_type.rectangular)
                return (int)Math.Ceiling( 0.9 / deltaF);
            else if (window_Type == window_type.hanning)
                return (int)Math.Ceiling(3.1 / deltaF);
            else if (window_Type == window_type.hamming)
                return (int)Math.Ceiling(3.3 / deltaF);
            else 
                return (int)Math.Ceiling(5.5 / deltaF);
        }

        float get_coeff(window_type window_Type, int n, float fc_dash, float f1, float f2, int N)
        {
            float h = 0;
            float w = 0;
            //compute h
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                if (n == 0)
                {
                    h = 2 * fc_dash;
                }
                else
                {
                    double nw = n * 2 * Math.PI * fc_dash;
                    h = 2 * fc_dash * (float)(Math.Sin(nw) / nw);
                }

            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                if (n == 0)
                {
                    h = 1 - (2 * fc_dash);
                }
                else
                {
                    double nw = n * 2 * Math.PI * fc_dash;
                    h = - 2 * fc_dash * (float)(Math.Sin(nw) / nw);
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                if (n == 0)
                {
                    h = 2 * (f2 - f1);
                }
                else
                {
                    double nw1 = n * 2 * Math.PI * f1;
                    double nw2 = n * 2 * Math.PI * f2;
                    h = 2 * f2 * (float)(Math.Sin(nw2) / nw2) - 2 * f1 * (float)(Math.Sin(nw1) / nw1);
                    
                }
            }
            else
            {
                if (n == 0)
                {
                    h = 1 - (2 * (f2 - f1));
                }
                else
                {
                    double nw1 = n * 2 * Math.PI * f1;
                    double nw2 = n * 2 * Math.PI * f2;
                    h = (2 * f1 * (float)(Math.Sin(nw1) / nw1)) - (2 * f2 * (float)(Math.Sin(nw2) / nw2));

                }
            }

            //compute w
            if (window_Type == window_type.rectangular)
            {
                w = 1;
            }
            else if (window_Type == window_type.hanning)
            {
                w = (float)( 0.5 + (0.5 * Math.Cos((2 * Math.PI * n) / N)));
            }
            else if (window_Type == window_type.hamming)
            {
                w = (float)(0.54 + (0.46 * Math.Cos((2 * Math.PI * n) / N)));
            }
            else
            {
                double term1 = (0.5 * Math.Cos((2 * Math.PI * n) / (N - 1)));
                double term2 = (0.08 * Math.Cos((4 * Math.PI * n) / (N - 1)));
                w = (float)(0.42 + term1 + term2);
            }

            return w * h;


        }

        public override void Run()
        {
            OutputYn = new Signal(new List<float>(), false);

            //if high or low 
            float fc_dash = 0;
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                fc_dash = ((float)InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS;
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                fc_dash = ((float)InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS;
            }

            //if bandpass or bandstop
            float f1_dash = 0;
            float f2_dash = 0;

            if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                f1_dash = ((float)InputF1 - (InputTransitionBand / 2)) / InputFS;
                f2_dash = ((float)InputF2 + (InputTransitionBand / 2)) / InputFS;
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                f1_dash = ((float)InputF1 + (InputTransitionBand / 2)) / InputFS;
                f2_dash = ((float)InputF2 - (InputTransitionBand / 2)) / InputFS;

            }


            window_type window_Type = get_window_type();

            int N = computeN(window_Type);

            if (N % 2 == 0)
                N += 1;

            float[] coeff_samples = new float[N];
            int[] coeff_indices = new int[N];

            int middle = (int)Math.Ceiling((double)N / 2) - 1;

            for (int i = 0; i < middle + 1; i++)
            {

                float coeff = get_coeff(window_Type, i, (float)fc_dash, (float)f1_dash, (float)f2_dash, N);
                coeff_samples[middle + i] = coeff;
                coeff_samples[middle - i] = coeff;

                coeff_indices[middle + i] = i;
                coeff_indices[middle - i] = -i;

            }

            OutputHn = new Signal(new List<float>(coeff_samples), new List<int>(coeff_indices), false);


            //compute signal values(convolution)

            DirectConvolution directConvolution = new DirectConvolution();
            directConvolution.InputSignal1 = InputTimeDomainSignal;
            directConvolution.InputSignal2 = OutputHn;

            directConvolution.Run();

            OutputYn = directConvolution.OutputConvolvedSignal;

        }
    }
}
