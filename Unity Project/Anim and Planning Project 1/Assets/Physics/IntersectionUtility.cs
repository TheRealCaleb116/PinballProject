using System;
using UnityEngine;

namespace Assets.Physics
{
    internal static class IntersectionUtility
    {
        public static bool CircleIntersectsCircle(CirclePrimitive a, CirclePrimitive b)
        {
            if ((a.center - b.center).sqrMagnitude <= (a.radius + b.radius) * (a.radius + b.radius))
            {
                return true;
            }
            return false;
        }
        public static bool CircleIntersectsAABB(CirclePrimitive c, AABBPrimitive b)
        {
            //Get the vector between the centers
            Vector2 distance = c.center - b.center;

            //Check for inside edge case 
            if (Math.Abs(distance.x) <= b.bounds.x && Math.Abs(distance.y) <= b.bounds.y)
            {
                return true;
            }

            //Otherwise get closest point
            Vector2 closestPoint = CollisionLib.Clamp(c.center, b.center - b.bounds, b.center + b.bounds);

            //Compare distance to closest point
            return (closestPoint - c.center).sqrMagnitude < (c.radius * c.radius);
        }
        public static bool CircleIntersectsLine(CirclePrimitive circle, LinePrimitive line)
        {
            //Check the easy cases
            if ((line.start - circle.center).sqrMagnitude <= (circle.radius * circle.radius))
            {
                return true;
            }

            if ((line.end - circle.center).sqrMagnitude <= (circle.radius * circle.radius))
            {
                return true;
            }

            //Difficult Case
            Vector2 toCircle = circle.center - line.start;
            float b = -2 * Vector2.Dot(line.vectorLine.normalized, toCircle);
            float c = toCircle.sqrMagnitude - (circle.radius * circle.radius);

            float d = b * b - 4 * c; //ignoring a because it is 1

            if (d >= 0)
            {
                float t1 = (-b - MathF.Sqrt(d)) / 2;
                if (t1 > 0 && t1 < line.vectorLine.magnitude)
                {
                    return true;
                }
            }

            return false;
        }

        /*
        //Unused Intersection code from HW 
        //A version of the Line Line intersect that takes in only vectors

        //Used in the AABB Line intersect test
        public static bool LineIntersectsLine(Vector2 l1Start, Vector2 l1End, Vector2 l2Start, Vector2 l2End)
        {
            if (isOnSameSide(l1Start, l1End, l2Start, l2End))
            {
                return false;
            }
            if (isOnSameSide(l2Start, l2End, l1Start, l1End))
            {
                return false;
            }

            return true;
        }
        public static bool isOnSameSide(Vector2 lineStart, Vector2 lineEnd, Vector2 p1, Vector2 p2)
        {
            float cp1 = CollisionLib.Cross(lineEnd - lineStart, p1 - lineStart);
            float cp2 = CollisionLib.Cross(lineEnd - lineStart, p2 - lineStart);

            return cp1 * cp2 >= 0;
        }
        //Used by the Line AABB test
        public static bool pointInsideAABB(Vector2 p, AABBPrimitive b)
        {
            Vector2 distance = p - b.center;

            //Check for inside edge case 
            if (Math.Abs(distance.x) <= b.bounds.x && Math.Abs(distance.y) <= b.bounds.y)
            {
                return true;
            }

            return false;
        }


        public static bool AABBIntersectsAABB(AABBPrimitive b1, AABBPrimitive b2)
        {
            if (Math.Abs(b1.center.x - b2.center.x) > (b1.bounds.x + b2.bounds.x))
            {
                return false;
            }
            if (Math.Abs(b1.center.y - b2.center.y) > (b1.bounds.y + b2.bounds.y))
            {
                return false;
            }

            return true;
        }


        public static bool LineIntersectsLine(LinePrimitive l1, LinePrimitive l2)
        {
            if (isOnSameSide(l1.start, l1.end, l2.start, l2.end))
            {
                return false;
            }
            if (isOnSameSide(l2.start, l2.end, l1.start, l1.end))
            {
                return false;
            }

            return true;
        }

        public static bool AABBIntersectsLine(AABBPrimitive b, LinePrimitive l)
        {
            //Handel of each point is inside the box, filled box
            if (pointInsideAABB(l.start, b) || pointInsideAABB(l.end, b))
            {
                return true;
            }

            //Check against the lines othat make up the box
            if (IntersectionUtility.LineIntersectsLine(l.start, l.end, b.bottomLeft, b.topLeft)) { return true; }
            if (IntersectionUtility.LineIntersectsLine(l.start, l.end, b.bottomRight, b.topRight)) { return true; }
            if (IntersectionUtility.LineIntersectsLine(l.start, l.end, b.topRight, b.topLeft)) { return true; }
            if (IntersectionUtility.LineIntersectsLine(l.start, l.end, b.bottomRight, b.bottomLeft)) { return true; }

            return false;
        }

        */
    }


}
