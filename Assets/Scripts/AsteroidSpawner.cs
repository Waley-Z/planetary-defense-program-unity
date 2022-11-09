using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public float spawnRadius = 100f;
    
    private GameObject asteroidPrefab;

    // Start is called before the first frame update
    void Start()
    {
        asteroidPrefab = GameAssets.GetPrefab("Asteroid");
    }

    public IEnumerator SpawnAsteroidWave(int num, float interval, float error, float speed, bool toPlanet = true, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        int spawned = 0;
        float radius = toPlanet ? spawnRadius : (spawnRadius + 3);
        while (spawned < num)
        {
            float randomRadius = radius * Random.Range(0.95f, 1.05f);
            Vector3 pos = Random.insideUnitCircle.normalized * randomRadius;

            if (Utils.IfPositionVisible(pos) || Physics.CheckSphere(pos, 0.4f))
                continue;

            spawnAsteroid(pos, error, toPlanet, speed);
            spawned++;
            yield return new WaitForSeconds(interval);
        }
        if (!toPlanet)
        {
            yield break;
        }
        // check enemy numbers left
        while (true)
        {
            bool enemyLeft = false;
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<Orbit>().isOrbit)
                    continue;
                else
                {
                    enemyLeft = true;
                    break;
                }
            }
            if (!enemyLeft)
            {
                if (GameManager.WaveNum == 20)
                {
                    ToastManager.ToastInstruction("you survived\nbut keep moving!");
                    yield return new WaitForSeconds(10);
                }
                EventBus.Publish(new WaveNumChangeEvent(GameManager.WaveNum+1));
                yield break;
            }
            yield return new WaitForSeconds(interval);
        }
    }

    void spawnAsteroid(Vector3 position, float error, bool toPlanet, float speed)
    {
        if (!asteroidPrefab)
            asteroidPrefab = GameAssets.GetPrefab("Asteroid");
        GameObject go = Instantiate(asteroidPrefab, position, Quaternion.identity);
        go.transform.parent = transform;
        AsteroidController ac = go.GetComponent<AsteroidController>();
        ac.toPlanet = toPlanet;
        ac.directionError = error;
        ac.speed = speed;
        go.GetComponent<OffscreenIndicator>().enabled = toPlanet;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
    }
}
