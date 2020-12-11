using UnityEngine;
using UnityEngine.UI;

public class EntityHealthBar : MonoBehaviour
{
    [SerializeField]
    private Entity entity = null;

    [SerializeField]
    private Image healthBar = null;

    //Ideally, you would only update this when the hp value actually changes
    void FixedUpdate()
    {
        if (entity == null || healthBar == null)
        {
            Debug.LogError($"MISSING REFS IN ENTITY HEALTHBAR: {gameObject.name}");
            return;
        }

        healthBar.fillAmount = entity.CurrentHealth / entity.MaxHealth;
    }
}
