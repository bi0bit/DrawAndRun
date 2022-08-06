using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Triggers
{
    public class AddUnitTrigger : MonoBehaviour
    {
        private bool _triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Unit unit) && !_triggered)
            {
                UnitManager manager = unit.GetComponentInParent<UnitManager>();
                if (manager != null)
                {
                    _triggered = true;
                    var futurePosition = manager.CenterMove.position + (manager.RunVelocity * 0.8f);
                    transform.DOJump(futurePosition, 2f, 1, 0.8f)
                        .OnComplete(() =>
                        {
                            manager.AddUnit();
                            Destroy(gameObject);
                        });
                }
            }
        }
    }
}