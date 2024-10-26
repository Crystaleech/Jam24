using System.Collections;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bombPrefab; // 炸弹预制体
    public Vector2 spawnRangeX = new Vector2(-8f, 8f); // X轴生成范围
    public float spawnHeight = 10f; // 生成高度

    private void Start()
    {
        SpawnBomb();
    }

    // 生成炸弹
    public void SpawnBomb()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnRangeX.x, spawnRangeX.y), // 随机X位置
            spawnHeight,
            0f
        );

        GameObject newBomb = Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
        ThrowBomb bombScript = newBomb.GetComponent<ThrowBomb>();

        // 设置炸弹爆炸后的回调
        if (bombScript != null)
        {
            bombScript.OnBombExploded += SpawnBomb;
        }
    }
}
