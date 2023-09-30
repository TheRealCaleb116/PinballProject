using Assets.Physics;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : BaseCollider
{

    public List<LinePrimitive> linePrimitives = new List<LinePrimitive>();
    private LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Awake()
    {
        //Get the Line render
        lineRenderer = GetComponent<LineRenderer>();

        //Assemble a list of line primatives from the vertexes in the line renderer
        Vector2 p1 = lineRenderer.GetPosition(0);
        Vector2 p2 = new Vector2();

        for (int x=1; x < lineRenderer.positionCount; x++)
        {
            //Get end position
            p2 = lineRenderer.GetPosition(x);

            //Create new line
            LinePrimitive l = new LinePrimitive(p1, p2);
            linePrimitives.Add(l);

            //Set new start
            p1 = p2;
        }


        //Debug.Log(linePrimitives.Count);

        //Register with the GameManager
        index = GameManager.RegisterLine(this);
    }


    //Should not be used
    public override void UpdatePosition()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdatePosition(Vector2 position)
    {
        throw new System.NotImplementedException();
    }

}
