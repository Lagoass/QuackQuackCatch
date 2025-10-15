using UnityEngine;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform healthBar;
    private int lives = 4;
    private int max_lives;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreEnd;
    private Vector3 initialHealthBarScale;

    public GameObject endScreen; // assign in Inspector if possible

    // 2 Audio Variables, one for enemy hit and one for collectible
    public AudioSource enemyHitSound;
    public AudioSource collectibleSound;
    public AudioSource deathSound;

    private bool isAlive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAlive = true;
        endScreen.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        max_lives = lives;
        initialHealthBarScale = healthBar.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb.MovePosition(rb.position + movement * 10 * Time.fixedDeltaTime);
    }

    //Trigger colide
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlive) return;
        // If collided with an enemy, take damage
        if (collision.gameObject.CompareTag("Inimigo") || collision.gameObject.CompareTag("Enemy"))
        {
            enemyHitSound.PlayOneShot(enemyHitSound.clip);
            TakeDamage(1);
        }
        // if collided with a collectable, increase score
        else if (collision.gameObject.CompareTag("Collectible"))
        {
            collectibleSound.PlayOneShot(collectibleSound.clip);
            score++;
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
                scoreEnd.text = score.ToString();
            }
            Debug.Log($"Score: {score}");
        }
    }

    private void TakeDamage(int amount)
    {
        lives -= amount;

        // Update health bar scale
        if (healthBar != null)
        {
            float healthPercent = (float)lives / max_lives;
            healthBar.localScale =  Vector3.Scale(initialHealthBarScale, new Vector3(healthPercent, 1f, 1f));
        }


        Debug.Log($"Player took {amount} damage. Lives remaining: {lives}");
        if (lives <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        deathSound.PlayOneShot(deathSound.clip);
        Debug.Log("Player died.");
        if (endScreen != null)
        {
            endScreen.SetActive(true);
        }
    }

    public bool IsAlive()
    {
        return lives > 0 && gameObject.activeSelf;
    }

}
