using UnityEngine;
using System.Collections;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] mob;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(2f, 4f);

    private void Start()
    {
        StartCoroutine(SpawnMobs());
    }

    private IEnumerator SpawnMobs()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(waitTime);

            int randomIndex = Random.Range(0, mob.Length);
            GameObject mobToSpawn = mob[randomIndex];

            Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position;
            Instantiate(mobToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
