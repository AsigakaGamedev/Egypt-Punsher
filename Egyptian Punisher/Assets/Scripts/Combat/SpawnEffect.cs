using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : AttackEffect
{
    [SerializeField] private PoolableObject prefab;
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnRadius;

    private ObjectPoolingManager poolingManager;

    public override void OnInit()
    {
        poolingManager = ServiceLocator.GetService<ObjectPoolingManager>();
    }

    public override void OnAttack(Vector3 usePoint)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            PoolableObject newPollable = poolingManager.GetPoolable(prefab);
            newPollable.transform.position = usePoint + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0,
                                                                    Random.Range(-spawnRadius, spawnRadius));
        }
    }
}
