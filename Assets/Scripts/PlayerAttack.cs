using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private float attackDistance = 1.5f; // Distance from player to spawn slash

    private GameObject attackArea = default;

    private bool attacking = false;

    private float timeToAttack = 0.25f;
    private float attackCooldown = 2f;
    private float attackTimer = 0f;
    private float attackDurationTimer = 0f;

    private Vector3 originalScale;
    private Renderer attackRenderer;

    private Plane attackPlane;

    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        attackArea.SetActive(false);

        originalScale = attackArea.transform.localScale;
        attackRenderer = attackArea.GetComponent<Renderer>();

        attackPlane = new Plane(Vector3.up, Vector3.zero); // For top-down (X-Z) plane
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            Attack();
            attackTimer = 0f;
        }

        if (attacking)
        {
            attackDurationTimer += Time.deltaTime;
            AnimateAttackArea();

            if (attackDurationTimer >= timeToAttack)
            {
                attacking = false;
                attackArea.SetActive(false);
                attackDurationTimer = 0f;
                ResetAttackArea();
            }
        }
    }

    private void Attack()
    {
        attacking = true;

        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        Vector3 direction = (mouseWorldPosition - transform.position).normalized;

        Vector3 spawnPosition = transform.position + direction * attackDistance;

        // Face the slash toward the mouse direction
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

        attackArea.transform.position = spawnPosition;
        attackArea.transform.rotation = rotation;
        attackArea.SetActive(true);

        if (slashEffectPrefab != null)
        {
            GameObject slash = Instantiate(slashEffectPrefab, spawnPosition, rotation);
            Destroy(slash, 1f);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (attackPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return transform.position + transform.forward; // fallback
    }

    private void AnimateAttackArea()
    {
        float scale = 1f + Mathf.PingPong(Time.time * 5f, 0.3f);
        attackArea.transform.localScale = originalScale * scale;
    }

    private void ResetAttackArea()
    {
        attackArea.transform.localScale = originalScale;

        if (attackRenderer != null)
        {
            attackRenderer.material.color = Color.white;
        }
    }
}
