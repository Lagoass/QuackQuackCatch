using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector2 initialDirection = new Vector2(1f, 1f);
    [SerializeField] private bool randomizeInitialDirection = true;
    [SerializeField] private bool flipSpriteOnTurn = true;
    [SerializeField] private bool invertFlip = false; // marque se o sprite estiver invertido por padrão

    private Rigidbody2D rb;
    private Vector2 direction;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalScale = (spriteRenderer != null ? spriteRenderer.transform.localScale : transform.localScale);
    }

    void Start()
    {
        if (randomizeInitialDirection)
        {
            float x = Random.value < 0.5f ? -1f : 1f;
            float y = Random.value < 0.5f ? -1f : 1f;
            direction = new Vector2(x, y).normalized; // sempre diagonal
        }
        else
        {
            direction = initialDirection.sqrMagnitude < 0.0001f ? new Vector2(1f, 1f).normalized : initialDirection.normalized;
        }

        rb.gravityScale = 0f; // top-down 2D
        rb.linearVelocity = direction * speed;
        ApplyFlip();
    }

    void FixedUpdate()
    {
        // mantém velocidade constante
        rb.linearVelocity = direction * speed;
        ApplyFlip();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            // Reflete a direção com base na normal de contato (rebate)
            Vector2 normal = collision.GetContact(0).normal;
            direction = Vector2.Reflect(direction, normal).normalized;
            rb.linearVelocity = direction * speed;
            ApplyFlip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            // Fallback quando a parede é trigger (sem normal): inverte eixo predominante
            Vector2 closest = other.ClosestPoint(transform.position);
            Vector2 delta = (Vector2)transform.position - closest;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                direction.x = -direction.x;
            else
                direction.y = -direction.y;

            direction.Normalize();
            rb.linearVelocity = direction * speed;
            ApplyFlip();
        }
    }

    private void ApplyFlip()
    {
        if (!flipSpriteOnTurn) return;
        bool faceLeft = direction.x > 0f; // Corrigido: invertido para corresponder ao movimento
        if (spriteRenderer != null)
        {
            // inverte se necessário (para sprites que já estão virados ao contrário por padrão)
            spriteRenderer.flipX = invertFlip ? !faceLeft : faceLeft;
        }
        else
        {
            var scale = originalScale;
            int sign = faceLeft ? -1 : 1;
            if (invertFlip) sign *= -1;
            scale.x = Mathf.Abs(scale.x) * sign;
            transform.localScale = scale;
        }
    }
}
