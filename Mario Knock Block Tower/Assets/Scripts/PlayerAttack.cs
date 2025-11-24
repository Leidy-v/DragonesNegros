using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ataque")]
    public float attackRange = 1.2f;
    public float attackRadius = 0.5f;
    public float attackCooldown = 0.5f;
    public float attackHeight = 0.8f;
    public LayerMask enemyLayer;

    bool canAttack = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canAttack)
        {
            Attack();
        }
    }

    void Attack()
    {
        canAttack = false;

        Vector3 attackOrigin = transform.position
                             + Vector3.up * attackHeight
                             + transform.forward * attackRange;

        Collider[] hitObjects = Physics.OverlapSphere(attackOrigin, attackRadius, enemyLayer);

        foreach (Collider obj in hitObjects)
        {
            CoinChest chest = obj.GetComponent<CoinChest>();
            if (chest != null)
                chest.OpenChest();
            else
                Destroy(obj.gameObject);
        }

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 attackOrigin = transform.position
                             + Vector3.up * attackHeight
                             + transform.forward * attackRange;

        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
    }
}
