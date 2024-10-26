using System.Collections;
using UnityEngine;

public class ExplodeDestroy : MonoBehaviour
{
    public float knockbackForce = 4f;
    public float knockbackRange = 3.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Blocks"))
        {
            Debug.Log("Explosion hit: " + other.gameObject.name);
            if (other != null)
            {
                Destroy(other.gameObject);
            }
            ApplyKnockback();
        }
    }

    private void ApplyKnockback()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, knockbackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider == null) continue;

            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
                Debug.Log("Knockback applied to: " + collider.gameObject.name);
            }
        }

        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, knockbackRange);
    }
}
