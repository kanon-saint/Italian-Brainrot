using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] public int attackDamage = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mob"))
        {
            PlayerAttibutes enemyAttributes = other.GetComponent<PlayerAttibutes>();
            if (enemyAttributes != null)
            {
                enemyAttributes.health -= attackDamage;
                Debug.Log("Enemy hit! Remaining HP: " + enemyAttributes.health);
            }
        }
    }
}
