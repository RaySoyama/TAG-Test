using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [SerializeField]
    private List<string> targetTag = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        if (targetTag.Contains(other.tag)) //Tag based, it could be layer, or custom tag based, but this was the quickest
        {
            Destroy(gameObject);
        }
    }
}
