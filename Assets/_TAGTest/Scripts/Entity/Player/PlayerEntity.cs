using UnityEngine;

public class PlayerEntity : Entity
{
    [SerializeField]
    private Rigidbody rb = null;

    [SerializeField]
    private Animator anim = null;

    [SerializeField]
    private MenuManager menuManager = null;

    [SerializeField, ReadOnlyField]
    private float timeSinceLastAttack = -100;

    public override void Move(Vector3 direction)
    {
        if (canMove == false)
        {
            return;
        }

        if (rb == null)
        {
            Debug.LogError("Missing Ref to RigidBody on Player");
            return;
        }

        //Since I don't really want to do the mental math of converting world space input into a isometric transform, I'm just gonna rotate the environment. (RIP Artists) 
        //FULL DISCALIMER: If I was working at a company I would do this in a heart beat, but I have like 3 other programming tests that are all due this week and I need to get this done.

        rb.AddForce(direction * speed * Time.deltaTime);
    }

    public override void LookAt(Vector3 direction)
    {
        //Things do get funky when you go leave worldPos.y = 0
        direction = new Vector3(direction.x, transform.position.y, direction.z);

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public override void Attack()
    {
        if (canAttack == false)
        {
            return;
        }

        if (anim == null)
        {
            Debug.LogError("Missing Ref to animator on Player");
            return;
        }

        if (Time.time - timeSinceLastAttack > attackSpeed)
        {
            //spawn bullet
            timeSinceLastAttack = Time.time;
            anim.SetTrigger("Attack");
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        //Open Menu with restart option
        if (menuManager == null)
        {
            Debug.LogError($"Missing ref to menuManager on: {gameObject.name}");
            return;
        }
        menuManager.OpenMenu();
    }

}
