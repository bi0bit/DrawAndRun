using DG.Tweening;
using UnityEngine;

namespace Game.Triggers
{
    public class PointTrigger : MonoBehaviour
    {

        private bool _triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Unit unit) && !_triggered)
            {
                _triggered = true;
                Game.Instance.PointCounter.Add();
                Destroy(gameObject);
            }
        }
    }
}