using System;
using UnityEngine;

namespace Assets.Physics
{
    //Functions needed for collision info
    public static class CollisionLib
    {
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
        {
            return new Vector2(Math.Clamp(value.x, min.x, max.x), Math.Clamp(value.y, min.y, max.y));
        }

        public static float Cross(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        public static Vector2 Project(Vector2 v1, Vector2 v2)
        {
            float k = Vector2.Dot(v1, v2) / Vector2.Dot(v2, v2);
            return new Vector2(k * v2.x, k * v2.y);
        }

    }

    //Shape Primatives
    public class CirclePrimitive 
    {
        public Vector2 center;
        public float radius;

        public CirclePrimitive(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        
    }
    public class AABBPrimitive 
    {
        public Vector2 center;
        public float width;
        public float height;

        //Bounds (AKA half width and half height in a vector
        public Vector2 bounds;

        //Vector points for the corners
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;

        public AABBPrimitive(Vector2 center, float width, float height)
        {
            this.center = center;
            this.width = width;
            this.height = height;

            this.bounds = new Vector2(width / 2, height / 2);

            //Calculate corners
            topLeft = new Vector2(center.x - bounds.x, center.y + bounds.y);
            topRight = new Vector2(center.x + bounds.x, center.y + bounds.y);
            bottomLeft = new Vector2(center.x - bounds.x, center.y - bounds.y);
            bottomRight = new Vector2(center.x + bounds.x, center.y - bounds.y);
        }

    }
    public class LinePrimitive 
    {
        public Vector2 start;
        public Vector2 end;

        public Vector2 vectorLine;

        public LinePrimitive(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;

            vectorLine = end - start;
        }

        public void UpdateLine(Vector2 start, Vector2 end)
        {
            //Update the line with new info
            this.start = start;
            this.end = end;

            vectorLine= end - start;
        }

    }

}

