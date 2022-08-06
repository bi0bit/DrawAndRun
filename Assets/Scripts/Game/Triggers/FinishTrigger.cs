using UnityEngine;

namespace Game.Triggers
{
    [RequireComponent(typeof(BoxCollider))]
    public class FinishTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Unit unit))
            {
                Game.Instance.FinishRun();
            }
        }
    }
}