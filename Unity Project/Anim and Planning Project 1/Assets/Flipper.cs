using Assets.Physics;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public GameObject paddle;
    private SpriteRenderer spriteRenderer;

    public bool flip = false;

    //Bounds and Physics
    public float maxAngle = 90;
    public float minAngle = -20;
    public float currentAngle = 0;

    private bool pressing = false;

    public float angularVelocity = 0;
    public float speed = 10.0f;
    public float resetAcceleration = 10.0f;
    public float cor = 0.95f;
    public float mass = 100;

    public int dirMod = 1;

    //Flipper Line primatives
    public LinePrimitive[] sides;

    // Start is called before the first frame update
    void Start()
    {
        //Get Z as a euler angle to set our intital angle
        currentAngle = transform.rotation.eulerAngles.z;
        spriteRenderer = paddle.GetComponent<SpriteRenderer>();

        if (flip)
        {
            //Set the position of the child
            paddle.transform.localPosition = new Vector2(-spriteRenderer.bounds.extents.x, 0.0f);
            //Set direction
            dirMod = -1;
        }
        else
        {
            //Set the position of the child
            paddle.transform.localPosition = new Vector2(spriteRenderer.bounds.extents.x, 0.0f);
            //Set direction
            dirMod = 1;
        }

        //Assemble the line primatives
        Vector2[] corners = GetSpriteCorners(spriteRenderer);
        if (flip)
        {
            sides = new LinePrimitive[3];
            sides[0] = new LinePrimitive(corners[0], corners[3]);
            sides[1] = new LinePrimitive(corners[0], corners[2]);
            sides[2] = new LinePrimitive(corners[1], corners[0]);
        }
        else
        {
            sides = new LinePrimitive[3];
            sides[0] = new LinePrimitive(corners[3], corners[0]);
            sides[1] = new LinePrimitive(corners[2], corners[1]);
            sides[2] = new LinePrimitive(corners[0], corners[1]);
        }

        //Register this flipper
        GameManager.flippers.Add(this);
    }

    public void UpdatePrimatives()
    {
        //Update the positions of the lines given the current locations of the corners of the paddle gameobject
        if (flip)
        {
            Vector2[] corners = GetSpriteCorners(spriteRenderer);
            sides[0].UpdateLine(corners[0], corners[3]);
            sides[1].UpdateLine(corners[0], corners[2]);
            sides[2].UpdateLine(corners[1], corners[0]);
        }
        else
        {
            Vector2[] corners = GetSpriteCorners(spriteRenderer);
            sides[0].UpdateLine(corners[3], corners[0]);
            sides[1].UpdateLine(corners[2], corners[1]);
            sides[2].UpdateLine(corners[0], corners[1]);
        }
    }

    private void FixedUpdate()
    {
        if (pressing)
        {
            //currently holding the button
            angularVelocity += speed * Time.fixedDeltaTime * dirMod;
        }
        else
        {
            angularVelocity += resetAcceleration * Time.fixedDeltaTime * dirMod;
        }

        currentAngle += angularVelocity * Time.fixedDeltaTime;
        if (currentAngle> maxAngle)
        {
            currentAngle = maxAngle;
            angularVelocity = 0;

        }else if (currentAngle < minAngle) 
        { 
            currentAngle= minAngle;
            angularVelocity= 0;
        }
        
        //Set current position
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, currentAngle ));

        //Update the flippers primatives
        UpdatePrimatives();



    }

    // Update is called once per frame
    void Update()
    {
        //Get button
        if (flip)
        {
            pressing = Input.GetMouseButton(1);
        }
        else
        {
            pressing = Input.GetMouseButton(0);
        }

    }

    public static Vector2[] GetSpriteCorners(SpriteRenderer renderer)
    {
        //Transforms the positions of the corners of the spirte acording to the sprites transform
        Vector2 topRight = renderer.transform.TransformPoint(renderer.sprite.bounds.max);
        Vector2 topLeft = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.max.x, renderer.sprite.bounds.min.y, 0));
        Vector2 botLeft = renderer.transform.TransformPoint(renderer.sprite.bounds.min);
        Vector2 botRight = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0));
        return new Vector2[] { topRight, topLeft, botLeft, botRight };
    }

}
