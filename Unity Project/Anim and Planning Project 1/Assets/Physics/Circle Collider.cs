using System.Collections;
using UnityEngine;

namespace Assets.Physics
{
    public class CircleCollider : BaseCollider
    {
        public CirclePrimitive circle = null;
        private SpriteRenderer spriteRenderer = null;
        public bool isStatic = true;
        public bool flashOnHit = false;

        // Start is called before the first frame update
        void Awake()
        {
            //Set the collider box to be eqaul to the size of the bounds
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Bounds b = spriteRenderer.bounds;

            //Create bounding primative
            circle = new CirclePrimitive(b.center, b.size.x / 2);

            //Register with the GameManager
            if (isStatic == true)
            {
                this.index = GameManager.RegisterCircle(this);
            }

        }

        // Visualize the collider bounds
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                UpdatePosition();
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(circle.center, circle.radius);
            }

        }

        public override void UpdatePosition()
        {
            if (circle != null)
            {
                circle.center = transform.position;
            }
        }

        public override void UpdatePosition(Vector2 position)
        {
            circle.center = position;
        }

        public override void OnHit()
        {
            //Increase Score
            GameManager.IncreaseScore(score);

            //Flash
            if (flashOnHit)
            {
                Color c = new Color(0.7f, 0.7f, 0.7f);
                StartCoroutine(Flash(c, 0.2f, 4));

            }
        }

        IEnumerator Flash(Color flashColor, float duration, int times)
        {
            float time = 0;
            Color baseColor = spriteRenderer.color;
            float flashHDur = duration / 2;

            for (int i = 0; i < times; i++)
            {
                while (time < flashHDur)
                {
                    time += Time.deltaTime;
                    spriteRenderer.color = Color.Lerp(baseColor, flashColor, time);
                    yield return null;
                }
                time = 0;
                while (time < flashHDur)
                {
                    time += Time.deltaTime;
                    spriteRenderer.color = Color.Lerp(flashColor, baseColor, time);
                    yield return null;
                }
                time = 0;
            }
            spriteRenderer.color = Color.white;

        }

    }

}



