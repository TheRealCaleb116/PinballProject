using Assets.Physics;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    //Launcher State and Physics
    public float restingPosY = 0;
    public float backMax = 0;
    public float forwardMax = 0;
    public float backOutVelocity = 10;
    public float velocityY = 0;
    public float accelerationY = 10;
    public float mass = 100;

    // 0=resting 1=charging 2=released 3=reseting
    public int state = 0;
    public LinePrimitive front;

    private float maxForwardY = 0;
    private float maxBackY = 0;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        restingPosY = transform.position.y;
        maxBackY = restingPosY - backMax;
        maxForwardY = restingPosY + forwardMax;

        spriteRenderer= GetComponent<SpriteRenderer>();

        Vector2[] corners = GetSpriteFront(spriteRenderer);

        front = new LinePrimitive(corners[0], corners[1]);

        GameManager.RegisterPlunger(this);
    }
    private void Update()
    {
        //Button Down
        if (state == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            state= 1;
        }

        //Button up
        if (state == 1 && Input.GetKeyUp(KeyCode.Space))
        {
            state = 2;
        }
    }
    void FixedUpdate()
    {
        if (state == 1)
        {
            velocityY = backOutVelocity * Time.fixedDeltaTime;

        }
        else if (state == 2)
        {
            velocityY += accelerationY * Time.fixedDeltaTime;

        }
        else if (state == 3)
        {
            velocityY -= accelerationY * Time.fixedDeltaTime;
        }
        

        Vector2 newPos = new Vector2(transform.position.x, transform.position.y + velocityY * Time.fixedDeltaTime);


        if (state == 1)
        {
            //Clamp if charging
            if (newPos.y < maxBackY)
            {
                newPos.y = maxBackY;
                velocityY= 0;
            }
        }
        else
        {
            //Clamp forward
            if (state == 2 && newPos.y > maxForwardY)
            {
                state = 3;
                velocityY *= 0.5f;
            }
            else if (state == 3 && newPos.y < restingPosY)
            {
                newPos.y = restingPosY;
                state = 0;
                velocityY = 0;
            }
            
        }

        //Update position
        transform.position = new Vector3(newPos.x, newPos.y, 0.0f);
        UpdatePrimatives();

    }

    public static Vector2[] GetSpriteFront(SpriteRenderer renderer)
    {
        //Transforms the positions of the corners of the spirte acording to the sprites transform
        Vector2 topRight = renderer.transform.TransformPoint(renderer.sprite.bounds.max);
        Vector2 topLeft = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.max.x, renderer.sprite.bounds.min.y, 0));
        Vector2 botLeft = renderer.transform.TransformPoint(renderer.sprite.bounds.min);
        Vector2 botRight = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0));
        return new Vector2[] { topLeft, botLeft};
    }
    public void UpdatePrimatives()
    {
        Vector2[] corners = GetSpriteFront(spriteRenderer);
        front.UpdateLine(corners[0], corners[1]);
    }
}
