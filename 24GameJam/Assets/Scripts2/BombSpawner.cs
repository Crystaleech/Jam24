using System.Collections;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bombPrefab; // ը��Ԥ����
    public Vector2 spawnRangeX = new Vector2(-8f, 8f); // X�����ɷ�Χ
    public float spawnHeight = 10f; // ���ɸ߶�

    private void Start()
    {
        SpawnBomb();
    }

    // ����ը��
    public void SpawnBomb()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnRangeX.x, spawnRangeX.y), // ���Xλ��
            spawnHeight,
            0f
        );

        GameObject newBomb = Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
        ThrowBomb bombScript = newBomb.GetComponent<ThrowBomb>();

        // ����ը����ը��Ļص�
        if (bombScript != null)
        {
            bombScript.OnBombExploded += SpawnBomb;
        }
    }
}
