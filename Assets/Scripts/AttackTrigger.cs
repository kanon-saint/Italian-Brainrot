using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mob"))
        {
            PlayerAttibutes enemyAttributes = other.GetComponent<PlayerAttibutes>();
            if (enemyAttributes != null)
            {
                enemyAttributes.health -= 2;
                Debug.Log("Enemy hit! Remaining HP: " + enemyAttributes.health);
            }
        }
    }
}
