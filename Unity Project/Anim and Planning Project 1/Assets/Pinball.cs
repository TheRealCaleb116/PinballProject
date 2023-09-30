using Assets.Physics;
using UnityEngine;

public class Pinball : MonoBehaviour
{
    public CircleCollider col;


    public float maxVelocity = 200.0f;
    public Vector2 velocity = new Vector2();
    public Vector2 acceleration = new Vector2();
    public float gravity = 10;
    public float mass = 100;
    public bool affectedByGravity = true;
    public float cor = 0.95f;
    private int index = -1;

    void Start()
    {
        //Calculate gravity for this obejct
        col = GetComponent<CircleCollider>();

        //Register myself as a pinball with the collision lists
        this.index = GameManager.RegisterPinball(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Calculate acceleration due to gravity
        Vector2 gravityAcceleration = (Vector2.down * gravity);


        if (affectedByGravity)
        {
            velocity += gravityAcceleration * Time.fixedDeltaTime;
        }

        //Clamp the velocity under the max
        ClampVelocity();

        //Calculate the new positon of the ball
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y) + (velocity * Time.fixedDeltaTime);

        //Update the balls colider for that new position
        col.UpdatePosition(newPosition);


        //Check movement against other pinballs
        for (int i = 0; i < GameManager.Pinballs.Count; i++)
        {
            //Check for collisions against pinballs

            //Make sure we are not colliding against ourself
            if (GameManager.Pinballs[i].index == col.index) { continue; }

            //Get the other collider and test for an overlap
            CirclePrimitive otherPinball = GameManager.Pinballs[i].col.circle;

            if (IntersectionUtility.CircleIntersectsCircle(col.circle, otherPinball))
            {
                //They intersect
                Vector2 hitVector = col.circle.center - otherPinball.center;
                float overlapDist = (hitVector.magnitude - (col.circle.radius + otherPinball.radius));

                //Correct positon
                newPosition += hitVector.normalized * -overlapDist;
                col.UpdatePosition(newPosition);

                //Reflect velocity making sure to preserve momentum in a realistic way
                float v1 = Vector2.Dot(velocity, hitVector.normalized);
                float v2 = Vector2.Dot(GameManager.Pinballs[i].velocity, hitVector.normalized);

                float m1 = mass;
                float m2 = GameManager.Pinballs[i].mass;

                float newV1 = (m1 * v1 + m2 * v2 - m2 * (v1 - v2) * cor) / (m1 + m2);
                float newV2 = (m1 * v1 + m2 * v2 - m1 * (v2- v1) * cor) / (m1 + m2);

                //Adjust velocity
                velocity += hitVector.normalized * (newV1 - v1);
                GameManager.Pinballs[i].velocity += hitVector.normalized * (newV2 - v2);


            }
        }

        //Lines
        for (int x = 0; x < GameManager.lines.Count; x++)
        {
            LineCollider lc = GameManager.lines[x];

            for (int y = 0; y < lc.linePrimitives.Count; y++)
            {
                //Iterate through all the lines in the collider
                if (IntersectionUtility.CircleIntersectsLine(col.circle, lc.linePrimitives[y]))
                {
                    //Based on https://stackoverflow.com/questions/1073336/circle-line-segment-collision-detection-algorithm
                    //https://paulbourke.net/geometry/pointlineplane/

                    //get closest point'
                    Vector2 p1 = lc.linePrimitives[y].end;
                    Vector2 p2 = lc.linePrimitives[y].start;
                    Vector2 p3 = col.circle.center;

                    Vector2 delta = p2 - p1;
                    float u = ((p3.x - p1.x) * delta.x + (p3.y - p1.y) * delta.y) / (delta.x * delta.x + delta.y * delta.y);

                    Vector2 closestPoint;
                    if (u < 0.0f)
                    {
                        closestPoint = p1;
                    }
                    else if (u > 1.0f)
                    {
                        closestPoint = p2;
                    }
                    else
                    {
                        closestPoint = new Vector2(p1.x + u * delta.x, p1.y + u * delta.y);
                    }

                    Vector2 hitVector = col.circle.center - closestPoint;
                    float overlapDist = (hitVector.magnitude - (col.circle.radius));
              
                    //Correct positon
                    newPosition += hitVector.normalized * -(overlapDist);
                    col.UpdatePosition(newPosition);

                    //Reflect velocity
                    Vector2 normal = hitVector.normalized;
                    Vector2 z = (Vector2.Dot(velocity, normal) * normal);
                    velocity = velocity - (1.80f) * z;

                }
            }
        }

        //AABBs
        for (int i = 0; i < GameManager.AABBs.Count; i++)
        {
            //Check for collisions against AABBs

            AABBPrimitive box = GameManager.AABBs[i].aabb;

            if (IntersectionUtility.CircleIntersectsAABB(col.circle, box))
            {
                //They intersect
                Vector2 cPoint = CollisionLib.Clamp(col.circle.center, box.center - box.bounds, box.center + box.bounds);

                Vector2 hitVector = cPoint - col.circle.center;

                float overlap = (hitVector.magnitude) - col.circle.radius;
                if (overlap.Equals(0))
                {
                    overlap = 0;
                }
                Vector2 corVector = hitVector.normalized * overlap * 1.0f;

                //Correct position
                newPosition += corVector;
                col.UpdatePosition(newPosition);


                //Reflect velocity
                Vector2 normal = hitVector.normalized;
                Vector2 b = (Vector2.Dot(velocity, normal) * normal);
                velocity = velocity - (1.80f) * b;


                //Call on hit
                GameManager.AABBs[i].OnHit();
            }
        }

        //Circles
        for (int i = 0; i < GameManager.circles.Count; i++)
        {
            //Check for collisions against Circles

            CirclePrimitive otherC = GameManager.circles[i].circle;

            if (IntersectionUtility.CircleIntersectsCircle(col.circle, otherC))
            {
                //They intersect
                Vector2 hitVector = col.circle.center - otherC.center;

                float overlapDist = (hitVector.magnitude - (col.circle.radius + otherC.radius));

                //Correct positon
                newPosition += hitVector.normalized * -overlapDist;
                col.UpdatePosition(newPosition);

                //Reflect velocity
                Vector2 normal = hitVector.normalized;
                Vector2 b = (Vector2.Dot(velocity, normal) * normal);
                velocity = velocity - (1.80f) * b;

                //Call on hit
                GameManager.circles[i].OnHit();
                
            }
        }

        //Check collision against paddles
        for (int x = 0; x < GameManager.flippers.Count; x++)
        {
            GameManager.flippers[x].UpdatePrimatives();
            LinePrimitive[] sides = GameManager.flippers[x].sides;
            Debug.DrawLine(sides[0].start, sides[0].end, Color.magenta);

            //Check the flippers
            for (int y = 0; y < sides.Length; y++)
            {
                if (IntersectionUtility.CircleIntersectsLine(col.circle, sides[y]))
                {
                    //get closest point
                    Vector2 p1 = sides[y].end;
                    Vector2 p2 = sides[y].start;
                    Vector2 p3 = col.circle.center;

                    Vector2 delta = p2 - p1;
                    float u = ((p3.x - p1.x) * delta.x + (p3.y - p1.y) * delta.y) / (delta.x * delta.x + delta.y * delta.y);


                    Vector2 closestPoint;
                    if (u < 0.0f)
                    {
                        closestPoint = p1;
                    }
                    else if (u > 1.0f)
                    {
                        closestPoint = p2;
                    }
                    else
                    {
                        closestPoint = new Vector2(p1.x + u * delta.x, p1.y + u * delta.y);
                    }

                    Vector2 dir = col.circle.center - closestPoint;

                    //Correct positon
                    newPosition = closestPoint + (dir.normalized * col.circle.radius);
                    col.UpdatePosition(newPosition);

                    //Handle collision Physics
                    Vector2 radius = closestPoint - sides[y].start;
                    Vector2 surfaceVelocity = (Mathf.Deg2Rad * GameManager.flippers[x].angularVelocity) * new Vector2(-radius.y, radius.x);

                    //Calculate the new velocities
                    float vBall = Vector2.Dot(this.velocity, dir.normalized);
                    float vFlip = Vector2.Dot(surfaceVelocity, dir.normalized);

                    float m1 = this.mass;
                    float m2 = GameManager.flippers[x].mass;

                    //float newVBall = (m1 * vBall + m2 * vFlip - m2 * (vBall - vFlip) * cor) / (m1 + m2);
                    float newVBall = (m1 * vBall + m2 * vFlip - m2 * (vBall - vFlip) * cor) / (m1 + m2);

                    //Update the balls velocity.
                    velocity += dir.normalized * (newVBall - vBall);

                }
            }

        }

        
        //Check if we are colliding with the launcher
        if (IntersectionUtility.CircleIntersectsLine(col.circle, GameManager.launcher.front))
        {
            //We are overlapping
            LinePrimitive front = GameManager.launcher.front;

            //get closest point
            Vector2 p1 = front.end;
            Vector2 p2 = front.start;
            Vector2 p3 = col.circle.center;

            Vector2 delta = p2 - p1;

            float u = ((p3.x - p1.x) * delta.x + (p3.y - p1.y) * delta.y) / (delta.x * delta.x + delta.y * delta.y);

            Vector2 closestPoint;
            if (u < 0.0f)
            {
                closestPoint = p1;
            }
            else if (u > 1.0f)
            {
                closestPoint = p2;
            }
            else
            {
                closestPoint = new Vector2(p1.x + u * delta.x, p1.y + u * delta.y);
            }

            Vector2 dir = col.circle.center - closestPoint;

            //Correct positon
            newPosition = closestPoint + (dir.normalized * col.circle.radius);
            col.UpdatePosition(newPosition);

            //Handle collision Physics
            //Calculate the new velocities
            float vBall = Vector2.Dot(this.velocity, dir.normalized);
            float vFlip = Vector2.Dot(new Vector2(0.0f, GameManager.launcher.velocityY), dir.normalized);

            float m1 = this.mass;
            float m2 = GameManager.launcher.mass;

            float newVBall = (m1 * vBall + m2 * vFlip - m2 * (vBall - vFlip) * cor) / (m1 + m2);

            //Update the balls velocity.
            velocity += dir.normalized * (newVBall - vBall);

            //Tell the game that we have launched
            GameManager.BallLaunched();
        }


        //Draw Debug stuff
        Debug.DrawRay(gameObject.transform.position,velocity, Color.yellow);

        //Move the game object to its new position
        gameObject.transform.position = newPosition;


        //Check is the position below the kill line
        if (gameObject.transform.position.y < GameManager.killYLevel)
        {
            //The pinball is in the dead zone so kill it.
            this.Die();
        }

    }


    private void ClampVelocity()
    {
        Vector2.ClampMagnitude(velocity, maxVelocity);
    }


    public void Die()
    {
        GameManager.PinballDie(this);
        Destroy(gameObject);
    }

}
