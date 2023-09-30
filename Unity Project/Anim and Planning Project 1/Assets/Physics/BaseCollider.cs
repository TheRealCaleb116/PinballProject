using UnityEngine;


namespace Assets.Physics
{
    public abstract class BaseCollider : MonoBehaviour
    {

        public int index = -1;
        public float score = 0;

        public abstract void UpdatePosition();
        public abstract void UpdatePosition(Vector2 position);

        public virtual void OnHit()
        {
            GameManager.IncreaseScore(score);
        }
    }
}