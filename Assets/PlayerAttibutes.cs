using UnityEngine;
using System.Collections;

public class PlayerAttibutes : MonoBehaviour
{
    [SerializeField] public int health = 10;

    void Start()
    {

    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        // StartCoroutine(FlashRed());
    }

    // private IEnumerator FlashRed()
    // {
    //     // Flash red if hit
    // }
}
