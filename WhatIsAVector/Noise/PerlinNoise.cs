using System;

namespace Noise
{
    // code by https://github.com/formalatist/Perlin
    public abstract class Perlin<GradientType>
    {
        /// <summary>
        /// The function we use to smooth the interpolation between the
        /// different corners of the cube. With a linear interpolation we'll
        /// get hard edges.
        /// </summary>
        private readonly Func<float, float> _smoothingFunction;

        /// <summary>
        /// PermutationTable, shortened for readability
        /// </summary>
        protected int[] PT;

        /// <summary>
        /// The defaultPermutationTable is 512 ints long and an contains values 0..255
        /// </summary>
        private static readonly int[] DefaultPermutationTable = {
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
            141,128,195,78,66,215,61,156,180
        };

        private readonly GradientType[] _gradients;

        /// <summary>
        /// Performs the dot product (inner product) of two 3D vectors where one of the vectors is stored in the GradientType type.
        /// </summary>
        private readonly Func<GradientType, float, float, float, float> _dot;

        protected Perlin(
            GradientType[] gradients,
            Func<GradientType, float, float, float, float> dot,
            Func<float, float> smoothingFunction)
        {
            _gradients = gradients;
            _dot = dot;
            _smoothingFunction = smoothingFunction;
            PT = DefaultPermutationTable;
        }


        /// <summary>
        /// Standard Perlin Noise function, returns smooth noise in the range
        /// (0x00,0xFF). Provide the x, y and z coordinate to sample the noise
        /// function. y and z are optional parameters.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public byte NoiseByte(float x, float y = 0.5f, float z = 0.5f)
        {
            // noise ranges from -1 to 1
            // adding 1 to that will make the range from 0 to 2
            // multiplying that by 128 will make the range from 0 to 255
            return (byte)((Noise(x, y, z) + 1f) * 128f);
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
        public float Noise(float x, float y = 0.5f, float z = 0.5f)
        {
            //determine what cube we are in
            var cubeX = ((int)x) & (PT.Length / 2 - 1);
            var cubeY = ((int)y) & (PT.Length / 2 - 1);
            var cubeZ = ((int)z) & (PT.Length / 2 - 1);

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
            var xIndex = PT[cubeX] + cubeY;
            var x1Index = PT[cubeX + 1] + cubeY;
            //indexes for the gradients
            var v000 = _gradients[PT[PT[xIndex] + cubeZ] % _gradients.Length];
            var v001 = _gradients[PT[PT[xIndex] + cubeZ + 1] % _gradients.Length];
            var v010 = _gradients[PT[PT[xIndex + 1] + cubeZ] % _gradients.Length];
            var v011 = _gradients[PT[PT[xIndex + 1] + cubeZ + 1] % _gradients.Length];
            var v100 = _gradients[PT[PT[x1Index] + cubeZ] % _gradients.Length];
            var v101 = _gradients[PT[PT[x1Index] + cubeZ + 1] % _gradients.Length];
            var v110 = _gradients[PT[PT[x1Index + 1] + cubeZ] % _gradients.Length];
            var v111 = _gradients[PT[PT[x1Index + 1] + cubeZ + 1] % _gradients.Length];

            //calculate the local x, y and z coordinates (0..1)
            x -= (float)Math.Floor(x);
            y -= (float)Math.Floor(y);
            z -= (float)Math.Floor(z);

            //calculate dot products
            var v000Dot = _dot(v000, x, y, z);
            var v001Dot = _dot(v001, x, y, z - 1);
            var v010Dot = _dot(v010, x, y - 1, z);
            var v011Dot = _dot(v011, x, y - 1, z - 1);
            var v100Dot = _dot(v100, x - 1, y, z);
            var v101Dot = _dot(v101, x - 1, y, z - 1);
            var v110Dot = _dot(v110, x - 1, y - 1, z);
            var v111Dot = _dot(v111, x - 1, y - 1, z - 1);

            //calculate smoothed x, y and z values. These are used to get
            //a smoother interpolation between the dot products of the 
            //gradients and local coords
            var smoothedX = _smoothingFunction(x);
            var smoothedY = _smoothingFunction(y);
            var smoothedZ = _smoothingFunction(z);

            //linearly interpolate the dot products
            var v000V100Val = LinearlyInterpolate(v000Dot, v100Dot, smoothedX);
            var v001V101Val = LinearlyInterpolate(v001Dot, v101Dot, smoothedX);
            var v010V110Val = LinearlyInterpolate(v010Dot, v110Dot, smoothedX);
            var v011V111Val = LinearlyInterpolate(v011Dot, v111Dot, smoothedX);

            var zZeroPlaneVal = LinearlyInterpolate(v000V100Val, v010V110Val, smoothedY);
            var zOnePlaneVal = LinearlyInterpolate(v001V101Val, v011V111Val, smoothedY);

            return LinearlyInterpolate(zZeroPlaneVal, zOnePlaneVal, smoothedZ);
        }

        /// <summary>
        /// Tile Perlin Noise function, the noise is tiled over a region of
        /// tileRegion^3. Creates noise that will seamlessly tile in all
        /// directions in all dimensions. y, z and tileRegion are optional parameters.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="tileRegion">Specifies the size of the region to "tile over", a larger value means it will take longer for the noise to repeat.</param>
        /// <returns></returns>
        public float NoiseTiled(float x, float y = 0.5f, float z = 0.5f, int tileRegion = 2)
        {
            var cubeX = ((int)x) & (PT.Length / 2 - 1);
            var cubeY = ((int)y) & (PT.Length / 2 - 1);
            var cubeZ = ((int)z) & (PT.Length / 2 - 1);
            var xIndex = PT[cubeX % tileRegion] + cubeY % tileRegion;
            var x1Index = PT[(cubeX + 1) % tileRegion] + cubeY % tileRegion;
            var xIndex1 = PT[cubeX % tileRegion] + (cubeY + 1) % tileRegion;
            var x1Index1 = PT[(cubeX + 1) % tileRegion] + (cubeY + 1) % tileRegion;
            var v000 = _gradients[PT[PT[xIndex] + cubeZ % tileRegion] % _gradients.Length];
            var v001 = _gradients[PT[PT[xIndex] + (cubeZ + 1) % tileRegion] % _gradients.Length];
            var v010 = _gradients[PT[PT[xIndex1] + cubeZ % tileRegion] % _gradients.Length];
            var v011 = _gradients[PT[PT[xIndex1] + (cubeZ + 1) % tileRegion] % _gradients.Length];
            var v100 = _gradients[PT[PT[x1Index] + cubeZ % tileRegion] % _gradients.Length];
            var v101 = _gradients[PT[PT[x1Index] + (cubeZ + 1) % tileRegion] % _gradients.Length];
            var v110 = _gradients[PT[PT[x1Index1] + cubeZ % tileRegion] % _gradients.Length];
            var v111 = _gradients[PT[PT[x1Index1] + (cubeZ + 1) % tileRegion] % _gradients.Length];
            x -= (float)Math.Floor(x);
            y -= (float)Math.Floor(y);
            z -= (float)Math.Floor(z);
            var v000Dot = _dot(v000, x, y, z);
            var v001Dot = _dot(v001, x, y, z - 1);
            var v010Dot = _dot(v010, x, y - 1, z);
            var v011Dot = _dot(v011, x, y - 1, z - 1);
            var v100Dot = _dot(v100, x - 1, y, z);
            var v101Dot = _dot(v101, x - 1, y, z - 1);
            var v110Dot = _dot(v110, x - 1, y - 1, z);
            var v111Dot = _dot(v111, x - 1, y - 1, z - 1);
            var smoothedX = _smoothingFunction(x);
            var smoothedY = _smoothingFunction(y);
            var smoothedZ = _smoothingFunction(z);
            var v000V100Val = LinearlyInterpolate(v000Dot, v100Dot, smoothedX);
            var v001V101Val = LinearlyInterpolate(v001Dot, v101Dot, smoothedX);
            var v010V110Val = LinearlyInterpolate(v010Dot, v110Dot, smoothedX);
            var v011V111Val = LinearlyInterpolate(v011Dot, v111Dot, smoothedX);
            var zZeroPlaneVal = LinearlyInterpolate(v000V100Val, v010V110Val, smoothedY);
            var zOnePlaneVal = LinearlyInterpolate(v001V101Val, v011V111Val, smoothedY);
            return LinearlyInterpolate(zZeroPlaneVal, zOnePlaneVal, smoothedZ);
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
        public float NoiseOctaves(
            float x,
            float y,
            float z = 0.5f,
            int numOctaves = 6,
            float lacunarity = 2f,
            float persistence = 0.5f)
        {
            var noiseValue = 0f;
            var amp = 1f;
            var freq = 1f;
            var totalAmp = 0f;

            for (var i = 0; i < numOctaves; i++)
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
        /// Tiled noise functions to create tileable noise that consists of
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
        public float NoiseTiledOctaves(
            float x,
            float y,
            float z,
            int tileRegion = 2,
            int numOctaves = 6,
            float lacunarity = 2f,
            float persistence = 0.5f)
        {
            var noiseValue = 0f;
            var amp = 1f;
            var freq = 1f;
            var totalAmp = 0f;

            for (var i = 0; i < numOctaves; i++)
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

        private static float LinearlyInterpolate(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        /// <summary>
        /// Takes a val in the range 0..1 and returns an s-curve in the range 0..1
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected static float SmoothToSCurve(float val)
        {
            //This is a recommended replacement for the original 3t^2 - 2t^3
            //from https://mrl.nyu.edu/~perlin/paper445.pdf
            return val * val * val * (val * (val * 6f - 15f) + 10f);
        }
    }

    public class Perlin : Perlin<Perlin.SimpleVector3>
    {
        private static readonly SimpleVector3[] Gradients = {
            new(1,1,0), new(-1,1,-0), new(1,-1,0),
            new(-1,-1,0), new(1,0,1), new(-1,0,1),
            new(1,0,-1), new(-1,0,-1), new(0,1,1),
            new(0,-1,1), new(0,1,-1), new(0,-1,-1)};

        public Perlin(Func<float, float> smoothingFunction) : base(Gradients, Dot, smoothingFunction) { }

        public Perlin() : this(SmoothToSCurve) { }

        private static float Dot(SimpleVector3 gradient, float x, float y, float z)
        {
            return gradient.X * x + gradient.Y * y + gradient.Z * z;
        }

        public struct SimpleVector3
        {
            public readonly float X;
            public readonly float Y;
            public readonly float Z;

            public SimpleVector3(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
    }
}