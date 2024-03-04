using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]               //구조체도 에디터에서 값변경 가능하게 해줌 -원래는 할당된 메모리 위치를 몰라서(값타입) 에디터에서 안보임
    public struct SpawnData
    {
        public SpawnData(PoolObjectType type = PoolObjectType.EnemyBlue, float timeLaps = 0.5f)
        {
            this.spawnType = type;
            this.timeLaps = timeLaps;
        }

        public PoolObjectType spawnType;
        public float timeLaps;
    }

    public SpawnData[] spawnDatas;

    private void Start()
    {
        GameManager.Instance.GameStarting += Starting;
        GameManager.Instance.GameEnding += Ending;
    }

    void Starting() 
    {
        foreach (var data in spawnDatas)
        {
            StartCoroutine(SpawnCoroutine(data));
        }
    }

    void Ending() 
    {
        foreach (var data in spawnDatas)
        {
            StopCoroutine(SpawnCoroutine(data));
        }
    }

    private IEnumerator SpawnCoroutine(SpawnData data)
    {
        while (true)
        {
            yield return new WaitForSeconds(data.timeLaps);

            GameObject obj = Factory.Instance.GetObject(data.spawnType, transform.position, transform.eulerAngles);
        }
    }
}
