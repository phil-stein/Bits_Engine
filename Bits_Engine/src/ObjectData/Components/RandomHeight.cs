using BitsCore.Debugging;
using System;
using System.Diagnostics;
using System.Numerics;

namespace BitsCore.ObjectData.Components
{
    [System.Serializable]
    public class RandomHeight : Component
    {
        public int xSize { get; private set; }
        public int ySize { get; private set; }
        public float strength { get; private set; }
        public float frequency { get; private set; }
        public float lacunarity { get; private set; }
        public float gain { get; private set; }
        public int seed { get; private set; }

        /// <summary> Sets all verts in a mesh to a random height(y-coordinate) based on generated noise. </summary>
        /// <param name="_xSize"> The amount of vertices in x-direction. </param>
        /// <param name="_ySize"> The amount of vertices in y-direction. </param>
        /// <param name="_strength"> The strength of the applied noise. </param>
        /// <param name="_frequency"> The frequenzy(scale) of the applied noise. </param>
        /// <param name="_lacunarity"> The lacunarity(size of layered noise) of the applied noise. </param>
        /// <param name="_gain"> The gain of the applied noise. </param>
        /// <param name="_seed"> The seed of the applied noise. </param>
        public RandomHeight(int _xSize, int _ySize, float _strength = 1f, float _frequency = .005f, float _lacunarity = 1.2f, float _gain = .4f, int _seed = 1339)
        {
            this.xSize = _xSize;
            this.ySize = _ySize;
            this.strength = _strength;
            this.frequency = _frequency;
            this.lacunarity = _lacunarity;
            this.gain = _gain;
            this.seed = _seed;
        }

        internal override void OnAdd()
        {
            //exits if 'gameObject' doesn't have a Mesh-Component
            if (!gameObject.HasComp<Model>()) { throw new Exception("!!!RandomHeigh Component attached to GameObject without Mesh-Compoenent!!!"); }

            RandHeight();

            gameObject.GetComp<Model>().CalcNormals(false);
        }
        internal override void OnRemove()
        {
        }

        /// <summary> Sets all verts in a mesh to a random height(y-coordinate) based on generated noise. </summary>
        public void ReRandomizeHeigh(float _strength = 1f, float _frequency = .005f, float _lacunarity = 1.2f, float _gain = .4f, int _seed = 1339)
        {
            //exits if 'gameObject' doesn't have a Mesh-Component
            if (!gameObject.HasComp<Model>()) { throw new Exception("!!!RandomHeigh Component attached to GameObject without Mesh-Compoenent!!!"); }

            strength = _strength;
            frequency = _frequency;
            lacunarity = _lacunarity;
            gain = _gain;
            seed = _seed;

            RandHeight();
            gameObject.GetComp<Model>().CalcNormals(true);
        }

        /// <summary> Sets all verts in a mesh to a random height(y-coordinate) based on generated noise. </summary>
        private void RandHeight()
        {
            // Create and configure FastNoise object
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetSeed(seed); //the seed
            noise.SetFrequency(frequency); //"scales" the noise pattern
            noise.SetFractalLacunarity(lacunarity); //adds variety / a second noise layer 
            noise.SetFractalGain(gain); //smoothes it

            // Create and configure FastNoise object
            FastNoiseLite secondaryNoise = new FastNoiseLite();
            secondaryNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            secondaryNoise.SetSeed(seed + 2); //the seed
            secondaryNoise.SetFrequency(frequency * 2); //"scales" the secondaryNoise pattern
            secondaryNoise.SetFractalLacunarity(lacunarity * .5f); //adds variety / a second secondaryNoise layer 
            secondaryNoise.SetFractalGain(gain < 0.5f ? 5f : gain); //smoothes it

            // Create and configure FastNoise object
            FastNoiseLite tertiaryNoise = new FastNoiseLite();
            tertiaryNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            tertiaryNoise.SetSeed(seed + 4); //the seed
            tertiaryNoise.SetFrequency(frequency * 10); //"scales" the tertiaryNoise pattern
            tertiaryNoise.SetFractalLacunarity(lacunarity * 2); //adds variety / a second tertiaryNoise layer 
            tertiaryNoise.SetFractalGain(gain < 0.5f ? 5f : gain); //smoothes it

            float[] vertices = gameObject.GetComp<Model>().mesh.vertices;

            //make copy of vertices, could prob. just assign it but idk
            float[] verts = new float[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                verts[i] = vertices[i];
            }

            //goes through the vertices row by row and sets their value to the corresponding one in the generated noise-texture
            for (int row = 0; row < ySize; row++)
            {
                for (int vert = 0; vert < xSize; vert++)
                {
                    Vector2 center = Vector2.Zero;
                    Vector2 curVert = new Vector2(verts[((row * xSize * 8) + vert * 8) + 0], verts[((row * xSize * 8) + vert * 8) + 2]);
                    float distCenterMask = Vector2.Distance(center, curVert); distCenterMask *= .666f; distCenterMask -= 1f; distCenterMask *= distCenterMask; distCenterMask *= distCenterMask * .75f;

                    //noise-layers
                    float val = 0;
                    float priVal = noise.GetNoise(row, vert); priVal += 1.2f; priVal *= 1f; ;
                    float secVal = secondaryNoise.GetNoise(row, vert); secVal += 1f; secVal *= 0.5f;
                    float terVal = tertiaryNoise.GetNoise(row, vert); terVal += 1f; terVal *= 0.5f;
                    val += priVal * 1f;

                    //float secondNoise = (priVal > 2f ? 0f : (secVal * (priVal - 2f))); secondNoise *= .4f;
                    val += secVal > 2f ? 0 : secVal;

                    //only add the terNoise over y: 1.5 and fade it in
                    float thirdNoise = (priVal > 2f ? 0f : (terVal * (priVal - 2f))); thirdNoise *= .02f;
                    val += thirdNoise;

                    val *= .1f;
                    val = MathF.Abs(val);

                    float newVal = (-distCenterMask * -distCenterMask);
                    newVal = newVal > .1f ? .1f : newVal; newVal *= 50f; newVal *= 0.2f;

                    val *= 2;
                    val *= ((newVal - 1f) * 3f);
                    val *= distCenterMask * 20f;
                    val += newVal * 1f;

                    //layered noise on the plateau
                    val += (distCenterMask * priVal) * 0.5f;
                    val += (distCenterMask * secVal) * 0.4f;
                    val += (distCenterMask * terVal) * 0.02f;

                    verts[((row * xSize * 8) + vert * 8) + 1] = verts[((row * xSize * 8) + vert * 8) + 1] + val * strength * .1f; // newVal * .1f;
                    //Debug.WriteLine("Noise: " + noiseData[vert].ToString() + " at " + (vert).ToString() + ", vert: " + vert.ToString());
                }
            }

            gameObject.GetComp<Model>().mesh.SetVertices(verts);
        }

        //The old RandomHeight function, from GameObject class
        /*
        /// <summary> Sets all verts in a grid-mesh to a random height, aka. y-coordinate. </summary>
        /// <param name="vertices"> The vertices to be modified. </param>
        /// <param name="xSize"> The amount of vertices in x-direction. </param>
        /// <param name="ySize"> The amount of vertices in y-direction. </param>
        /// <param name="strength"> The strength of the applied noise. </param>
        /// <param name="frequency"> The frequenzy(scale) of the applied noise. </param>
        /// <param name="lacunarity"> The lacunarity(size of layered noise) of the applied noise. </param>
        /// <param name="gain"> The gain of the applied noise. </param>
        /// <param name="seed"> The seed of the applied noise. </param>
        /// <returns></returns>
        static float[] RandHeight(float[] vertices, int xSize, int ySize, float strength = 1f, float frequency = .005f, float lacunarity = 1.2f, float gain = .4f, int seed = 1339)
        {
            // Create and configure FastNoise object
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetSeed(seed); //the seed
            noise.SetFrequency(frequency); //"scales" the noise pattern
            noise.SetFractalLacunarity(lacunarity); //adds variety / a second noise layer 
            noise.SetFractalGain(gain); //smoothes it

            // Create and configure FastNoise object
            FastNoiseLite secondaryNoise = new FastNoiseLite();
            secondaryNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            secondaryNoise.SetSeed(seed + 2); //the seed
            secondaryNoise.SetFrequency(frequency * 2); //"scales" the secondaryNoise pattern
            secondaryNoise.SetFractalLacunarity(lacunarity * .5f); //adds variety / a second secondaryNoise layer 
            secondaryNoise.SetFractalGain(gain < 0.5f ? 5f : gain); //smoothes it

            // Create and configure FastNoise object
            FastNoiseLite tertiaryNoise = new FastNoiseLite();
            tertiaryNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            tertiaryNoise.SetSeed(seed + 4); //the seed
            tertiaryNoise.SetFrequency(frequency * 10); //"scales" the tertiaryNoise pattern
            tertiaryNoise.SetFractalLacunarity(lacunarity * 2); //adds variety / a second tertiaryNoise layer 
            tertiaryNoise.SetFractalGain(gain < 0.5f ? 5f : gain); //smoothes it

            //make copy of vertices, could prob. just assign it but idk
            float[] verts = new float[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                verts[i] = vertices[i];
            }

            //goes through the vertices row by row and sets their value to the corresponding one in the generated noise-texture
            for (int row = 0; row < ySize; row++)
            {
                for (int vert = 0; vert < xSize; vert++)
                {
                    Vector2 center = Vector2.Zero;
                    Vector2 curVert = new Vector2(verts[((row * xSize * 8) + vert * 8) + 0], verts[((row * xSize * 8) + vert * 8) + 2]);
                    float distCenterMask = Vector2.Distance(center, curVert); distCenterMask *= .666f; distCenterMask -= 1f; distCenterMask *= distCenterMask; distCenterMask *= distCenterMask * .75f;

                    //noise-layers
                    float val = 0;
                    float priVal = noise.GetNoise(row, vert); priVal += 1.2f; priVal *= 1f; ;
                    float secVal = secondaryNoise.GetNoise(row, vert); secVal += 1f; secVal *= 0.5f;
                    float terVal = tertiaryNoise.GetNoise(row, vert); terVal += 1f; terVal *= 0.5f;
                    val += priVal * 1f;

                    //float secondNoise = (priVal > 2f ? 0f : (secVal * (priVal - 2f))); secondNoise *= .4f;
                    val += secVal > 2f ? 0 : secVal;

                    //only add the terNoise over y: 1.5 and fade it in
                    float thirdNoise = (priVal > 2f ? 0f : (terVal * (priVal - 2f))); thirdNoise *= .02f;
                    val += thirdNoise;

                    val *= .1f;
                    val = MathF.Abs(val);

                    float newVal = (-distCenterMask * -distCenterMask);
                    newVal = newVal > .1f ? .1f : newVal; newVal *= 50f; newVal *= 0.2f;

                    val *= 2;
                    val *= ((newVal - 1f) * 3f);
                    val *= distCenterMask * 20f;
                    val += newVal * 1f;

                    //layered noise on the plateau
                    val += (distCenterMask * priVal) * 0.5f;
                    val += (distCenterMask * secVal) * 0.4f;
                    val += (distCenterMask * terVal) * 0.02f;

                    verts[((row * xSize * 8) + vert * 8) + 1] = verts[((row * xSize * 8) + vert * 8) + 1] + val * strength * .1f; // newVal * .1f;
                    //Debug.WriteLine("Noise: " + noiseData[vert].ToString() + " at " + (vert).ToString() + ", vert: " + vert.ToString());



                    /*
                    //vert rgb color
                    //map noise to 0f - 1f and move it up by 0.2 to  avoid the really dark areas
                    val += 1; val /= 2; //val *= secVal;
                    val = val < 0.1f ? 0.1f : val;
                    verts[((row * xSize * 8) + vert * 8) + 3] = val * .6f;
                    verts[((row * xSize * 8) + vert * 8) + 4] = val * .8f;
                    verts[((row * xSize * 8) + vert * 8) + 5] = val * .1f;
                    * /
                }
            }

            return verts;
        }
        */

    }
}
