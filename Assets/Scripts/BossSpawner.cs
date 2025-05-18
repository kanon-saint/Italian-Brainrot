using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] boss;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(2f, 4f);

    private void Start()
    {
        StartCoroutine(SpawnBoss());
    }

    private IEnumerator SpawnBoss()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(waitTime);

            int randomIndex = Random.Range(0, boss.Length);
            GameObject bossToSpawn = boss[randomIndex];

            Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position;
            Instantiate(bossToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
