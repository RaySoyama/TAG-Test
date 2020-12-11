using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField]
    private List<string> targetTag = new List<string>();

    [SerializeField]
    private float damage = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (targetTag.Contains(other.tag)) //Tag based, it could be layer, or custom tag based, but this was the quickest
        {
            //Do Damage
            Entity target;

            if (other.TryGetComponent(out target) == true)
            {
                target.TakeDamage(damage);
            }
            else
            {
                Debug.Log($"The target is not a entity and thus cannot take damage: {gameObject.name}");
            }
        }
    }
}
