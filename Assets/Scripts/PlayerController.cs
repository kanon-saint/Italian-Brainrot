using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

    private bool facingLeft = true;

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.deltaTime;
        transform.position += move;

        bool isMoving = moveX != 0f || moveZ != 0f;
        animator.SetFloat("Speed", isMoving ? 1f : 0f);

        // Flip sprite using Y-axis rotation
        if (moveX > 0f && facingLeft)
        {
            Flip();
        }
        else if (moveX < 0f && !facingLeft)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 rotation = transform.eulerAngles;
        rotation.y += 180f;
        transform.eulerAngles = rotation;
    }
}
