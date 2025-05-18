using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] public int attackDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mob") || other.CompareTag("Boss"))
        {
            PlayerAttibutes enemyAttributes = other.GetComponent<PlayerAttibutes>();
            if (enemyAttributes != null)
            {
                enemyAttributes.TakeDamage(attackDamage);
                Debug.Log("Enemy hit! Remaining HP: " + enemyAttributes.health);
            }
        }
    }

}
