using Assets.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Physics
{

    public class AABBCollider : BaseCollider
    {
        public AABBPrimitive aabb = null;

        // Start is called before the first frame update
        void Awake()
        {
            //Set the collider box to be eqaul to the size of the bounds
            Bounds b = gameObject.GetComponent<SpriteRenderer>().bounds;
            
            //Create bounding primative
            aabb = new AABBPrimitive(b.center, b.size.x, b.size.y);

            //Register with the GameManager
            this.index = GameManager.RegisterAABB(this);

        }

        // Visualize the collider bounds
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                UpdatePosition();
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(aabb.center, new Vector2(aabb.width, aabb.height));
            }

        }

        public override void UpdatePosition()
        {
            if (aabb!= null)
            {
                aabb.center = transform.position;
            }
        }

        public override void UpdatePosition(Vector2 position)
        {
            aabb.center = position;
        }


    }
}