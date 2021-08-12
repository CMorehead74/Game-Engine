using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using EngineHelper;

namespace EngineHelper.EHUtility
{
    public class EHMath
    {
        public float Seek(Vector2 facing, Vector2 toTarget)
        {
            return (facing.X * toTarget.Y) - (facing.Y * toTarget.X);
        }

        public int Clamp(int val, int low, int high)
        {
            val = ((val < low) ? low : val);
            return ((val > high) ? high : val);
        }

        public uint Clamp(uint val, uint low, uint high)
        {
            val = ((val < low) ? low : val);
            return ((val > high) ? high : val);
        }

        public byte Clamp(byte val, byte low, byte high)
        {
            val = ((val < low) ? low : val);
            return ((val > high) ? high : val);
        }

        public float Clamp(float val, float low, float high)
        {
            val = ((val < low) ? low : val);
            return ((val > high) ? high : val);
        }

        public double Clamp(double val, double low, double high)
        {
            val = ((val < low) ? low : val);
            return ((val > high) ? high : val);
        }

        public float Wrap(float val, float low, float high)
        {
            if (val < low)
                val += high;
            else if (val > high)
                val -= high;

            return val;
        }

        public byte GetByte(int MultiByteVal, int ByteIndex)
        {
            if (ByteIndex > sizeof(int))
            {
                return 0;
            }
            return (byte)(MultiByteVal >> (8 * ByteIndex));
        }

        public byte GetByte(uint MultiByteVal, int ByteIndex)
        {
            if (ByteIndex > sizeof(uint))
            {
                return 0;
            }
            return (byte)(MultiByteVal >> (8 * ByteIndex));
        }

        public byte GetByte(short MultiByteVal, int ByteIndex)
        {
            if (ByteIndex > sizeof(short))
            {
                return 0;
            }
            return (byte)(MultiByteVal >> (8 * ByteIndex));
        }

        public byte GetByte(ushort MultiByteVal, int ByteIndex)
        {
            if (ByteIndex > sizeof(ushort))
            {
                return 0;
            }
            return (byte)(MultiByteVal >> (8 * ByteIndex));
        }

        public byte GetByte(long MultiByteVal, int ByteIndex)
        {
            if (ByteIndex > sizeof(long))
            {
                return 0;
            }
            return (byte)(MultiByteVal >> (8 * ByteIndex));
        }

        public byte GetByte(ulong MultiByteVal, int ByteIndex)
        {
            if (ByteIndex > sizeof(ulong))
            {
                return 0;
            }
            return (byte)(MultiByteVal >> (8 * ByteIndex));
        }

        public Vector2 GetBezzierValue(float interporlation, Vector2[] points)
        {
            return GetBezzierValue(interporlation, points, points.Length, 0);
        }

        public Vector2 GetBezzierValue(float interporlation, Vector2[] points, int pointCount)
        {
            return GetBezzierValue(interporlation, points, pointCount, 0);
        }

        public Vector2 GetBezzierValue(float interporlation, Vector2[] points, int pointCount, int firstPoint)
        {
            interporlation = Clamp(interporlation, 0.0f, 1.0f);
            if (firstPoint >= points.Length)
            {
                return new Vector2(0.0f, 0.0f);
            }

            if (firstPoint + pointCount - 1 > points.Length)
            {
                return points[firstPoint];
            }

            Vector2[] oldPoints = points;
            Vector2[] newPoints = oldPoints;
            while (newPoints.Length > 1)
            {
                newPoints = new Vector2[oldPoints.Length - 1];

                for (int i = 0; i < oldPoints.Length - 1; i++)
                {
                    newPoints[i] = new Vector2(oldPoints[i].X, oldPoints[i].Y);
                    float x = (oldPoints[i + 1].X - oldPoints[i].X) * interporlation;
                    float y = (oldPoints[i + 1].Y - oldPoints[i].Y) * interporlation;
                    newPoints[i].X += x;
                    newPoints[i].Y += y;
                }

                oldPoints = newPoints;
            }

            return newPoints[0];
        }

        //Determines the angle between vectors.
        public float AngleBetweenVectors(Vector2 v1, Vector2 v2)
        {
            return (float)Math.Acos((double)(Vector2.Dot(v1, v2) / (v1.Length() * v2.Length())));
        }

        //Wraps an angle between 0.0 and maxRadians.
        public float WrapAngle(float angle, float maxRadians)
        {
            if (angle < 0.0f)
                angle += maxRadians;
            else if (angle > maxRadians)
                angle -= maxRadians;

            return angle;
        }

        public int Index1DArray(int col, int row, int maxCols)
        {
            return col + (row * maxCols);
        }

        public Rectangle NormalizeRect(Vector2 topLeft, Vector2 bottomRight)
        {
            int nLeft = (int)topLeft.X;

            if (nLeft > (int)bottomRight.X)
            {
                nLeft = (int)bottomRight.X;
                bottomRight.X = topLeft.X;
            }
            int nTop = (int)topLeft.Y;

            if (nTop > bottomRight.Y)
            {
                nTop = (int)bottomRight.Y;
                bottomRight.Y = (int)topLeft.Y;
            }
            return new Rectangle(nLeft, nTop, (int)bottomRight.X - nLeft, (int)bottomRight.Y - nTop);
        }

        public Rectangle NormalizeRect(int left, int top, int right, int bottom)
        {
            int nLeft = left;

            if (nLeft > right)
            {
                nLeft = right;
                right = left;
            }
            int nTop = top;

            if (nTop > bottom)
            {
                nTop = bottom;
                bottom = top;
            }
            return new Rectangle(nLeft, nTop, right - nLeft, bottom - nTop);
        }

        public Vector2 Arrive(Vector2 toTarget, float decelerationTime, float maxSpeed)
        {
            //calculate the distance to the target
            float dist = toTarget.Length();

            if (dist > 0)
            {
                //because Deceleration is enumerated as an int, this value is required
                //to provide fine tweaking of the deceleration..
                //const float DecelerationTweaker = 0.3f;

                //calculate the speed required to reach the target given the desired
                //deceleration
                // vel = dist/time

                float speed = dist / decelerationTime;// ((float)deceleration * DecelerationTweaker);     

                //make sure the velocity does not exceed the max
                speed = MathHelper.Min(speed, maxSpeed);

                //from here proceed just like Seek except we don't need to normalize 
                //the ToTarget vector because we have already gone to the trouble
                //of calculating its length: dist. 
                Vector2 DesiredVelocity = (toTarget * speed) / dist;
                return (DesiredVelocity);
            }

            return Vector2.Zero;
        }

        public float GetRotationDirBetweenAngles(float fromRotation, float toRotation)
        {
            float angleDiff = fromRotation - toRotation;

            float posOrNeg = 1.0f;

            //if angleDiff > 180
            if (Math.Abs(angleDiff) > (float)Math.PI)
            {
                //Choose smallest angle direction to rotate by: 45 to 325
                if (fromRotation < toRotation)
                    posOrNeg = -1.0f;
            }
            //if angleDiff < 180
            else if (angleDiff > 0.0f)
                posOrNeg = -1.0f;

            return posOrNeg;
        }

        public int GetSignedAngleBetweenAnglesDeg(int facing, int target)
        {
            int degAngle = target - facing;
            degAngle = ((degAngle + 180) % 360) - 180;
            return degAngle;
        }

        public float GetSignedAngleBetweenAnglesRad(float facing, float target)
        {
            float radAngle = target - facing;
            float pi = (float)Math.PI;
            radAngle = ((radAngle + pi) % pi) - pi;
            return radAngle;

        }

        //Find the angle between two vectors. This will not only give the angle difference, but the direction.
        //For example, it may give you -1 radian, or 1 radian, depending on the direction. Angle given will be the 
        //angle from the FromVector to the DestVector, in radians. All three vectors must lie along the same plane.
        public static double GetSignedAngleBetween2DVectors(Vector3 FromVector, Vector3 DestVector, Vector3 DestVectorsRight)
        {
            FromVector.Normalize();
            DestVector.Normalize();
            DestVectorsRight.Normalize();

            float forwardDot = Vector3.Dot(FromVector, DestVector);
            float rightDot = Vector3.Dot(FromVector, DestVectorsRight);

            // Keep dot in range to prevent rounding errors
            forwardDot = MathHelper.Clamp(forwardDot, -1.0f, 1.0f);

            double angleBetween = Math.Acos(forwardDot);

            if (rightDot < 0.0f)
                angleBetween *= -1.0f;

            return angleBetween;
        }

        public float UnsignedAngleBetweenTwoV3(Vector3 v1, Vector3 v2)
        {
            v1.Normalize();
            v2.Normalize();
            double Angle = (float)Math.Acos(Vector3.Dot(v1, v2));
            return (float)Angle;
        }

        //Returns velocity of deflection
        public Vector3 CalculateDeflection(Vector3 CurrentVelocity, float Elasticity, Vector3 CollisionNormal)
        {
            Vector3 newDirection = Elasticity * (-2 * Vector3.Dot(CurrentVelocity, CollisionNormal) * CollisionNormal + CurrentVelocity);

            return newDirection;
        }

        //The XNA Vector library already includes a method for deflection, accessible through 
        //Vector3.Reflect(...), however it lacks elasticity.
        //public Vector3 CalculateDeflection(Vector3 CurrentVelocity, float Elasticity, Vector3 CollisionNormal)
        //{
        //    return Vector3.Reflect(CurrentVelocity, CollisionNormal) * Elasticity;
        //}

        //Matrix already has a LookAt function!
        //public Matrix LookAt(Vector3 position, Vector3 lookat)
        //{
        //    Matrix rotation = new Matrix();

        //    rotation.Forward = Vector3.Normalize(lookat - position);
        //    rotation.Right = Vector3.Normalize(Vector3.Cross(rotation.Forward, Vector3.Up));
        //    rotation.Up = Vector3.Normalize(Vector3.Cross(rotation.Right, rotation.Forward));

        //    return rotation;
        //}

        //Translates a point around an origin
        public Vector3 RotateAroundPoint(Vector3 point, Vector3 originPoint, Vector3 rotationAxis, float radiansToRotate)
        {
            Vector3 diffVect = point - originPoint;
            Vector3 rotatedVect = Vector3.Transform(diffVect, Matrix.CreateFromAxisAngle(rotationAxis, radiansToRotate));
            rotatedVect += originPoint;
            return rotatedVect;
        }

        //Convert a Quaternion to Vector3
        public Vector3 QuaternionToEuler(Quaternion q)
        {
            Vector3 v = Vector3.Zero;
            v.X = (float)Math.Atan2
            (
                2 * q.Y * q.W - 2 * q.X * q.Z,
                   1 - 2 * Math.Pow(q.Y, 2) - 2 * Math.Pow(q.Z, 2)
            );

            v.Z = (float)Math.Asin
            (
                2 * q.X * q.Y + 2 * q.Z * q.W
            );

            v.Y = (float)Math.Atan2
            (
                2 * q.X * q.W - 2 * q.Y * q.Z,
                1 - 2 * Math.Pow(q.X, 2) - 2 * Math.Pow(q.Z, 2)
            );

            if (q.X * q.Y + q.Z * q.W == 0.5)
            {
                v.X = (float)(2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }

            else if (q.X * q.Y + q.Z * q.W == -0.5)
            {
                v.X = (float)(-2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }

            return v;
        }

        //This method will calculate a Vector3 that cycles the origin.
        //This returns a vector, which circles arounds the previous position (positive X-axis as 0°)
        static public Vector3 Cycle2(Vector3 Distance, float Period)
        {
            Vector3 Position = new Vector3();

            Period *= MathHelper.TwoPi;

            Position.X = Distance.X * (float)Math.Cos(Period);
            Position.Y = Distance.Y * (float)Math.Sin(Period);
            Position.Z = Distance.Z * (float)Math.Sin(Period);

            return Position;
        }

        //From Angle to Vector2
        public Vector2 AngleToV2(float angle)//, float length)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)Math.Cos(angle);// *length;
            direction.Y = (float)Math.Sin(angle);// *length;
            return direction;
        }

        //From Vector2 to Angle
        public float V2ToAngle(Vector2 direction)
        {
            return (float)Math.Atan2(direction.Y, direction.X);
        }

        public void GetAngleVectorArray(Vector2[] vectors, float angle)
        {
            int len = vectors.Length;

            float slice = angle / len;

            for (int i = 0; i < len; i++)
            {
                vectors[i] = AngleToV2(slice * i);
            }
        }
    }

    public class EHLineSegment
    {
        private Vector2 startPoint;
        private Vector2 endPoint;
        private Vector2 startToEnd;
        private float magnitude;
        private Rectangle boundingRect;

        public Vector2 StartPoint
        {
            get { return startPoint; }
            set { GenerateLineSegment(value, endPoint); }
        }

        public Vector2 EndPoint
        {
            get { return endPoint; }
            set { GenerateLineSegment(startPoint, value); }
        }

        public Vector2 StartToEnd
        {
            get { return startToEnd; }
        }

        public float Length
        {
            get { return magnitude; }
        }

        public Rectangle BoundingRect
        {
            get { return boundingRect; }
        }

        public bool IsHorizontal
        {
            get { return startPoint.Y == endPoint.Y; }
        }

        public bool IsVertical
        {
            get { return startPoint.X == endPoint.X; }
        }

        public bool IsDiagonal
        {
            get { return !IsHorizontal && !IsVertical; }
        }

        public EHLineSegment()
        {
            GenerateLineSegment(Vector2.Zero, Vector2.Zero);
        }

        public EHLineSegment(Vector2 startPoint, Vector2 endPoint)
        {
            GenerateLineSegment(startPoint, endPoint);
        }

        public float DistAlongLine(float posX)
        {
            float distX = posX - StartPoint.X;
            float t = distX / startToEnd.X;
            return t;
        }

        public float PositionYOnLine(float t)
        {
            Debug.WriteLine("sp: {0} ste: {1} t: {2}", StartPoint.Y, startToEnd.Y, t);
            return StartPoint.Y + (startToEnd.Y * t);
        }

        public void GenerateLineSegment(Vector2 startPoint, Vector2 endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;

            NormalizeLine();

            startToEnd = EndPoint - StartPoint;
            magnitude = startToEnd.Length();

            int top = (int)Math.Min(StartPoint.Y, EndPoint.Y);
            int height = (int)Math.Abs(startToEnd.Y);

            boundingRect = new Rectangle((int)StartPoint.X, top, (int)startToEnd.X, height);

            //the smallest the width and/or height of a line can be
            int minDimension = 15;

            int inflateW = 0;
            if (boundingRect.Width < minDimension)
                inflateW = minDimension;

            int inflateH = 0;
            if (boundingRect.Height < minDimension)
                inflateH = minDimension;

            boundingRect.Inflate(inflateW, inflateH);
        }

        private void NormalizeLine()
        {
            float swap = 0.0f;

            if (startPoint.X > endPoint.X)
            {
                swap = startPoint.X;
                startPoint.X = endPoint.X;
                endPoint.X = swap;

                // only swap Y's if swapped X
                swap = startPoint.Y;
                startPoint.Y = endPoint.Y;
                endPoint.Y = swap;
            }
        }
    }
}
