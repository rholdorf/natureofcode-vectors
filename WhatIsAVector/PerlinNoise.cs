﻿using System;
namespace WhatIsAVector
{
    // code by https://github.com/formalatist/Perlin
    public abstract class Perlin<GradientType>
    {

        /// <summary>
        /// The function we use to smooth the interpolation between the
        /// different corners of the cube. With a linear interpolation we'll
        /// get hard edges.
        /// </summary>
        private Func<double, double> SmoothingFunction;

        /// <summary>
        /// PermutationTable, shortened for readability
        /// </summary>
        protected int[] PT;

        /// <summary>
        /// The defaultPermutationTable is 512 ints long and an contains values 0..255
        /// </summary>
        private static int[] defaultPermutationTable = {
            151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,
            30,69,142,8,99,37,240,21,10,23,190,6,148,247,120,234,75,0,26,197,
            62,94,252,219,203,117,35,11,32,57,177,33,88,237,149,56,87,174,20,
            125,136,171,168, 68,175,74,165,71,134,139,48,27,166,77,146,158,231,
            83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,102,
            143,54,65,25,63,161,1,216,80,73,209,76,132,187,208,89,18,169,200,
            196,135,130,116,188,159,86,164,100,109,198,173,186,3,64,52,217,226,
            250,124,123,5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,
            58,17,182,189,28,42,223,183,170,213,119,248,152,2,44,154,163,70,221,
            153,101,155,167,43,172,9,129,22,39,253,19,98,108,110,79,113,224,232,
            178,185,112,104,218,246,97,228,251,34,242,193,238,210,144,12,191,
            179,162,241,81,51,145,235,249,14,239,107,49,192,214,31,181,199,106,
            157,184,84,204,176,115,121,50,45,127,4,150,254,138,236,205,93,222,
            114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,151,160,137,91,
            90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,
            37,240,21,10,23,190, 6,148,247,120,234,75,0,26,197,62,94,252,219,
            203,117,35,11,32,57,177,33,88,237,149,56,87,174,20,125,136,171,168,
            68,175,74,165,71,134,139,48,27,166,77,146,158,231,83,111,229,122,60,
            211,133,230,220,105,92,41,55,46,245,40,244,102,143,54,65,25,63,161,
            1,216,80,73,209,76,132,187,208,89,18,169,200,196,135,130,116,188,
            159,86,164,100,109,198,173,186,3,64,52,217,226,250,124,123,5,202,38,
            147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152,2,44,154,163,70,221,153,101,155,167,43,
            172,9,129,22,39,253,19,98,108,110,79,113,224,232,178,185,112,104,
            218,246,97,228,251,34,242,193,238,210,144,12,191,179,162,241,81,51,
            145,235,249,14,239,107,49,192,214,31,181,199,106,157,184,84,204,176,
            115,121,50,45,127,4,150,254,138,236,205,93,222,114,67,29,24,72,243,
            141,128,195,78,66,215,61,156,180};

        private GradientType[] gradients;

        /// <summary>
        /// Performs the dot product (inner product) of two 3D vectors where one of the vectors is stored in the GradientType type.
        /// </summary>
        private Func<GradientType, double, double, double, double> Dot;

        protected Perlin(
            GradientType[] gradients,
            Func<GradientType, double, double, double, double> dot,
            Func<double, double> smoothingFunction)
        {
            this.gradients = gradients;
            this.Dot = dot;
            this.SmoothingFunction = smoothingFunction;
            PT = defaultPermutationTable;
        }

        /// <summary>
        /// Standard Perlin Noise function, returns smooth noise in the range
        /// (-1,1). Provide the x, y and z coordinate to sample the noise
        /// function. y and z are optional parameters.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public double Noise(double x, double y = 0.5d, double z = 0.5d)
        {
            //determine what cube we are in
            int cubeX = ((int)x) & (PT.Length / 2 - 1);
            int cubeY = ((int)y) & (PT.Length / 2 - 1);
            int cubeZ = ((int)z) & (PT.Length / 2 - 1);

            /*Find the gradients for the 8 corners of the cube

						*V011---------*V111
						|\            |\
						| \           | \
						|  \          |  \
						|   *V010---------*V110
					V001*---|---------*V101 
						 \  |          \  |
						  \ |           \ |
						   \|            \|
						V000*-------------*V100
			**/
            int XIndex = PT[cubeX] + cubeY;
            int X1Index = PT[cubeX + 1] + cubeY;
            //indexes for the gradients
            GradientType V000 = gradients[PT[PT[XIndex] + cubeZ] % gradients.Length];
            GradientType V001 = gradients[PT[PT[XIndex] + cubeZ + 1] % gradients.Length];
            GradientType V010 = gradients[PT[PT[XIndex + 1] + cubeZ] % gradients.Length];
            GradientType V011 = gradients[PT[PT[XIndex + 1] + cubeZ + 1] % gradients.Length];
            GradientType V100 = gradients[PT[PT[X1Index] + cubeZ] % gradients.Length];
            GradientType V101 = gradients[PT[PT[X1Index] + cubeZ + 1] % gradients.Length];
            GradientType V110 = gradients[PT[PT[X1Index + 1] + cubeZ] % gradients.Length];
            GradientType V111 = gradients[PT[PT[X1Index + 1] + cubeZ + 1] % gradients.Length];

            //calculate the local x, y and z coordinates (0..1)
            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);

            //calculate dot products
            double V000Dot = Dot(V000, x, y, z);
            double V001Dot = Dot(V001, x, y, z - 1);
            double V010Dot = Dot(V010, x, y - 1, z);
            double V011Dot = Dot(V011, x, y - 1, z - 1);
            double V100Dot = Dot(V100, x - 1, y, z);
            double V101Dot = Dot(V101, x - 1, y, z - 1);
            double V110Dot = Dot(V110, x - 1, y - 1, z);
            double V111Dot = Dot(V111, x - 1, y - 1, z - 1);

            //calculate smoothed x, y and z values. These are used to get
            //a smoother interpolation between the dot products of the 
            //gradients and local coords
            double smoothedX = SmoothingFunction(x);
            double smoothedY = SmoothingFunction(y);
            double smoothedZ = SmoothingFunction(z);

            //linearly interpolate the dot products
            double V000V100Val = LinearlyInterpolate(V000Dot, V100Dot, smoothedX);
            double V001V101Val = LinearlyInterpolate(V001Dot, V101Dot, smoothedX);
            double V010V110Val = LinearlyInterpolate(V010Dot, V110Dot, smoothedX);
            double V011V111Val = LinearlyInterpolate(V011Dot, V111Dot, smoothedX);

            double ZZeroPlaneVal = LinearlyInterpolate(V000V100Val, V010V110Val, smoothedY);
            double ZOnePlaneVal = LinearlyInterpolate(V001V101Val, V011V111Val, smoothedY);

            return LinearlyInterpolate(ZZeroPlaneVal, ZOnePlaneVal, smoothedZ);
        }

        /// <summary>
        /// Tile Perlin Noise function, the noise is tiled over a region of
        /// tileRegion^3. Creates noise that will seamlessly tile in all
        /// directions in all dimensions. y, z and tileRedion are optional parameters.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="tileRegion">Specifies the size of the region to "tile over", a larger value means it will take longer for the noise to repeat.</param>
        /// <returns></returns>
        public double NoiseTiled(double x, double y = 0.5d, double z = 0.5d, int tileRegion = 2)
        {
            int cubeX = ((int)x) & (PT.Length / 2 - 1);
            int cubeY = ((int)y) & (PT.Length / 2 - 1);
            int cubeZ = ((int)z) & (PT.Length / 2 - 1);
            int XIndex = PT[cubeX % tileRegion] + cubeY % tileRegion;
            int X1Index = PT[(cubeX + 1) % tileRegion] + cubeY % tileRegion;
            int XIndex1 = PT[cubeX % tileRegion] + (cubeY + 1) % tileRegion;
            int X1Index1 = PT[(cubeX + 1) % tileRegion] + (cubeY + 1) % tileRegion;
            GradientType V000 = gradients[PT[PT[XIndex] + cubeZ % tileRegion] % gradients.Length];
            GradientType V001 = gradients[PT[PT[XIndex] + (cubeZ + 1) % tileRegion] % gradients.Length];
            GradientType V010 = gradients[PT[PT[XIndex1] + cubeZ % tileRegion] % gradients.Length];
            GradientType V011 = gradients[PT[PT[XIndex1] + (cubeZ + 1) % tileRegion] % gradients.Length];
            GradientType V100 = gradients[PT[PT[X1Index] + cubeZ % tileRegion] % gradients.Length];
            GradientType V101 = gradients[PT[PT[X1Index] + (cubeZ + 1) % tileRegion] % gradients.Length];
            GradientType V110 = gradients[PT[PT[X1Index1] + cubeZ % tileRegion] % gradients.Length];
            GradientType V111 = gradients[PT[PT[X1Index1] + (cubeZ + 1) % tileRegion] % gradients.Length];
            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);
            double V000Dot = Dot(V000, x, y, z);
            double V001Dot = Dot(V001, x, y, z - 1);
            double V010Dot = Dot(V010, x, y - 1, z);
            double V011Dot = Dot(V011, x, y - 1, z - 1);
            double V100Dot = Dot(V100, x - 1, y, z);
            double V101Dot = Dot(V101, x - 1, y, z - 1);
            double V110Dot = Dot(V110, x - 1, y - 1, z);
            double V111Dot = Dot(V111, x - 1, y - 1, z - 1);
            double smoothedX = SmoothingFunction(x);
            double smoothedY = SmoothingFunction(y);
            double smoothedZ = SmoothingFunction(z);
            double V000V100Val = LinearlyInterpolate(V000Dot, V100Dot, smoothedX);
            double V001V101Val = LinearlyInterpolate(V001Dot, V101Dot, smoothedX);
            double V010V110Val = LinearlyInterpolate(V010Dot, V110Dot, smoothedX);
            double V011V111Val = LinearlyInterpolate(V011Dot, V111Dot, smoothedX);
            double ZZeroPlaneVal = LinearlyInterpolate(V000V100Val, V010V110Val, smoothedY);
            double ZOnePlaneVal = LinearlyInterpolate(V001V101Val, V011V111Val, smoothedY);
            return LinearlyInterpolate(ZZeroPlaneVal, ZOnePlaneVal, smoothedZ);
        }

        /// <summary>
        /// Creates noise combined of multiple noise values at different
        /// octaves. Samples the standard noise functions multiple times and
        /// adds the results together. Each sample is sampled at a higher
        /// frequency and lower amplitude, adding more  and smaller features to
        /// the noise. All parameters except x are optional.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="numOctaves">Determines how many times to  sample the standard noise function</param>
        /// <param name="lacunarity">Specifies how quickly the frequency increases.</param>
        /// <param name="persistence">Specifies how quickly the amplitude of consecutive samples decreases</param>
        /// <returns></returns>
        public double NoiseOctaves(
            double x,
            double y,
            double z = 0.5d,
            int numOctaves = 6,
            double lacunarity = 2d,
            double persistence = 0.5d)
        {
            double noiseValue = 0d;
            double amp = 1d;
            double freq = 1d;
            double totalAmp = 0d;

            for (int i = 0; i < numOctaves; i++)
            {
                noiseValue += amp * Noise(x * freq, y * freq, z * freq);
                totalAmp += amp;
                amp *= persistence;
                freq *= lacunarity;
            }

            return noiseValue / totalAmp;
        }

        /// <summary>
        /// Creates tiled noise with multiple octaves. Combines the Octaves and
        /// Tiled nosise functions to create tilable noise that consists of
        /// multiple octaves.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="tileRegion"></param>
        /// <param name="numOctaves"></param>
        /// <param name="lacunarity"></param>
        /// <param name="persistence"></param>
        /// <returns></returns>
        public double NoiseTiledOctaves(
            double x,
            double y,
            double z,
            int tileRegion = 2,
            int numOctaves = 6,
            double lacunarity = 2d,
            double persistence = 0.5d)
        {
            double noiseValue = 0d;
            double amp = 1d;
            double freq = 1d;
            double totalAmp = 0d;

            for (int i = 0; i < numOctaves; i++)
            {
                noiseValue += amp * NoiseTiled(x * freq, y * freq, z * freq, tileRegion);
                totalAmp += amp;
                amp *= persistence;
                freq *= lacunarity;
            }

            return noiseValue / totalAmp;
        }

        /// <summary>
        /// Use a different permutationTable then the provided default. This
        /// will change the look of the noise
        /// </summary>
        /// <param name="newPermutationTable"></param>
        public void SetPermutationTable(int[] newPermutationTable)
        {
            //make sure the new PT has Length = 2^N (this property is 
            //used in the Noise function)
            if ((newPermutationTable.Length & newPermutationTable.Length - 1) == 0)
            {
                PT = newPermutationTable;
            }
        }

        private static double LinearlyInterpolate(double a, double b, double t)
        {
            return a + t * (b - a);
        }

        /// <summary>
        /// Takes a val in the range 0..1 and returns an s-curve in the range 0..1
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected static double SmoothToSCurve(double val)
        {
            //This is a recommended replacement for the original 3t^2 - 2t^3
            //from https://mrl.nyu.edu/~perlin/paper445.pdf
            return val * val * val * (val * (val * 6d - 15d) + 10d);
        }
    }

    public class Perlin : Perlin<Perlin.SimpleVector3>
    {
        private static SimpleVector3[] gradients = {
            new SimpleVector3(1,1,0), new SimpleVector3(-1,1,-0), new SimpleVector3(1,-1,0),
            new SimpleVector3(-1,-1,0), new SimpleVector3(1,0,1), new SimpleVector3(-1,0,1),
            new SimpleVector3(1,0,-1), new SimpleVector3(-1,0,-1), new SimpleVector3(0,1,1),
            new SimpleVector3(0,-1,1), new SimpleVector3(0,1,-1), new SimpleVector3(0,-1,-1)};

        public Perlin(Func<double, double> smoothingFunction) : base(gradients, Dot, smoothingFunction) { }

        public Perlin() : this(SmoothToSCurve) { }

        private static double Dot(SimpleVector3 gradient, double x, double y, double z)
        {
            return gradient.x * x + gradient.y * y + gradient.z * z;
        }

        public struct SimpleVector3
        {
            public double x, y, z;

            public SimpleVector3(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }
    }
}