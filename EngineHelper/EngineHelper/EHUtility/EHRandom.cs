using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EngineHelper;

namespace EngineHelper.EHUtility
{
    public class EHRandom
    {
        private Random random = new Random();

        public Random Random
        {
            get { return random; }
            private set { random = value; }
        }

        public EHRandom()
        {
            Seed();
        }

        public void Seed(int seed)
        {
            random = new Random(seed);
        }

        public void Seed()
        {
            random = new Random();
        }

        public int GetRandomInt()
        {
            return random.Next();
        }

        public int GetRandomInt(int max)
        {
            return random.Next(max);
        }

        // Returns a random int between min and max - 1.
        public int GetRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        //Helper to generate a random float in the range of [-1, 1].
        public float NextFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }

        public float GetRandomFloat(float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }

        public double GetRandomDouble()
        {
            return random.NextDouble();
        }

        public double GetRandomDouble(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        public Vector2 GetRandomPointInCircle(Vector2 center, float radius)
        {
            float radian = GetRandomFloat(0.0f, MathHelper.TwoPi);
            Vector2 toPoint = Vector2.Transform(Vector2.UnitY, Matrix.CreateRotationZ(radian));
            float length = GetRandomFloat(0.0f, radius);
            return center + (toPoint * length);
        }

        public Vector2 GetRandomPointInRect(Rectangle r)
        {
            int x = GetRandomInt(r.Left, r.Right);
            int y = GetRandomInt(r.Top, r.Bottom);

            return new Vector2(x, y);
        }

        public Vector2 GetRandomVector2(float xMin, float xMax,
                                        float yMin, float yMax)
        {
            return new Vector2(GetRandomFloat(xMin, xMax), GetRandomFloat(yMin, yMax));
        }

        //0 = up
        public Vector2 GetRandomAngleVector2(float minAngle, float maxAngle)
        {
            float radian = GetRandomFloat(minAngle, maxAngle);

            return Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ(radian));
        }

        public Vector2 GetRandomWaverVector2(Vector2 baseVec, float waverAngle)
        {
            float radian = GetRandomFloat(-waverAngle, waverAngle);

            return Vector2.Transform(baseVec, Matrix.CreateRotationZ(radian));
        }

        public Vector2 GetRandomOffsetAlongVector2(Vector2 baseVec, float offsetAmount)
        {
            float offset = GetRandomFloat(-offsetAmount, offsetAmount);

            baseVec.Normalize();

            return baseVec * offset;
        }
    }
}
