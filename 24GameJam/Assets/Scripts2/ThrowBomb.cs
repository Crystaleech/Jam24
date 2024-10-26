using System;
using System.Collections;
using UnityEngine;

public class ThrowBomb : MonoBehaviour
{
    public event Action OnBombExploded; // 炸弹爆炸事件

    public GameObject explosionRangePrefab;
    public GameObject pickupText1;
    public GameObject pickupText2;
    public GameObject countText;
    public GameObject bomb;
    public float detectionRange = 2f;
    public float pickupRange = 1.5f;
    public float explosionScale = 5f;
    public float explosionDuration = 0.8f;

    private GameObject currentCarryingPlayer;
    private bool isCarried = false;
    private Rigidbody2D rb;
    private GameObject player1;
    private GameObject player2;

    private void Start()
    {
        rb = bomb.GetComponent<Rigidbody2D>();
        player1 = GameObject.FindGameObjectWithTag("P1");
        player2 = GameObject.FindGameObjectWithTag("P2");
        pickupText1.SetActive(false);
        pickupText2.SetActive(false);
    }

    private void Update()
    {
        HandlePlayerInteraction(player1, pickupText1, KeyCode.LeftShift);
        HandlePlayerInteraction(player2, pickupText2, KeyCode.RightShift);

        if (isCarried)
        {
            MoveObjectAbovePlayer();
            pickupText1.SetActive(false);
            pickupText2.SetActive(false);
        }
    }

    private void HandlePlayerInteraction(GameObject player, GameObject pickupText, KeyCode pickupKey)
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            pickupText.SetActive(true);
            if (distanceToPlayer <= pickupRange && Input.GetKeyDown(pickupKey))
            {
                pickupText.SetActive(false);

                if (!isCarried)
                {
                    PickUpObject(player);
                }
                else if (currentCarryingPlayer == player)
                {
                    ThrowObject();
                }
                else
                {
                    TransferObject(player);
                }
            }
        }
        else
        {
            pickupText.SetActive(false);
        }
    }

    private void PickUpObject(GameObject player)
    {
        isCarried = true;
        currentCarryingPlayer = player;
        MoveObjectAbovePlayer();
        bomb.SetActive(true);
        StartCoroutine(StartCountdown(5f));
    }

    private IEnumerator StartCountdown(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            countText.SetActive(true);
            countText.GetComponent<TMPro.TextMeshProUGUI>().text = $"{timer:F1}";
            timer -= Time.deltaTime;
            yield return null;
        }

        countText.SetActive(false);
        ExplodeBomb();
    }

    private void TransferObject(GameObject newPlayer)
    {
        currentCarryingPlayer = newPlayer;
        MoveObjectAbovePlayer();
    }

    private void MoveObjectAbovePlayer()
    {
        Vector2 newPosition = currentCarryingPlayer.transform.position;
        newPosition.y += 1;
        bomb.transform.position = newPosition;
    }

    private void ThrowObject()
    {
        isCarried = false;
        MoveObjectAbovePlayer();
        Vector2 force = new Vector2(0, 6);
        if (currentCarryingPlayer.CompareTag("P1"))
        {
            if (Input.GetKey(KeyCode.A))
                force += Vector2.left * 6;
            else if (Input.GetKey(KeyCode.D))
                force += Vector2.right * 6;
        }
        else if (currentCarryingPlayer.CompareTag("P2"))
        {
            if (Input.GetKey(KeyCode.Keypad4))
                force += Vector2.left * 6;
            else if (Input.GetKey(KeyCode.Keypad6))
                force += Vector2.right * 6;
        }

        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
        currentCarryingPlayer = null;
    }

    private void ExplodeBomb()
    {
        Debug.Log("Boom! The bomb exploded.");

        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        Vector3 explosionPosition = transform.position;
        Destroy(gameObject); // 销毁炸弹对象

        if (explosionRangePrefab != null)
        {
            GameObject explosionRange = Instantiate(explosionRangePrefab, explosionPosition, Quaternion.identity);
            explosionRange.tag = "Explosion";
            StartCoroutine(ScaleExplosionRange(explosionRange));
        }

        // 触发炸弹爆炸事件
        OnBombExploded?.Invoke();
    }

    private IEnumerator ScaleExplosionRange(GameObject explosionRange)
    {
        Vector3 originalScale = Vector3.one * 0.01f;
        Vector3 targetScale = Vector3.one * explosionScale;

        float elapsedTime = 0f;
        explosionRange.transform.localScale = originalScale;

        while (elapsedTime < explosionDuration)
        {
            if (explosionRange == null) yield break;
            explosionRange.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / explosionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (explosionRange != null)
        {
            explosionRange.transform.localScale = targetScale;
            Destroy(explosionRange, 0.5f);
        }
    }
}
