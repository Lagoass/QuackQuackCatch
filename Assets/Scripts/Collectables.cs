using UnityEngine;
public class Collectables : MonoBehaviour
{
    [Header("Spawn Aleatório - Área")]
    [SerializeField] private BoxCollider2D spawnArea; // opcional: define a área de respawn

    [Header("Spawn Aleatório - Limites (caso não use área)")]
    [SerializeField] private Vector2 minBounds = new Vector2(-8, -4.5f);
    [SerializeField] private Vector2 maxBounds = new Vector2(8, 4.5f);

    [ContextMenu("Respawn Now")]
    public void Respawn()
    {
        Vector2 newPos;
        if (spawnArea != null)
        {
            Bounds b = spawnArea.bounds;
            newPos = new Vector2(
                Random.Range(b.min.x, b.max.x),
                Random.Range(b.min.y, b.max.y)
            );
        }
        else
        {
            newPos = new Vector2(
                Random.Range(minBounds.x, maxBounds.x),
                Random.Range(minBounds.y, maxBounds.y)
            );
        }

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        // Caso o coletável tenha sido desativado em algum fluxo, garante que ele esteja ativo
        if (!gameObject.activeSelf) gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Respawn();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (spawnArea == null)
        {
            Vector3 center = new Vector3((minBounds.x + maxBounds.x) / 2f, (minBounds.y + maxBounds.y) / 2f, 0f);
            Vector3 size = new Vector3(Mathf.Abs(maxBounds.x - minBounds.x), Mathf.Abs(maxBounds.y - minBounds.y), 0f);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
