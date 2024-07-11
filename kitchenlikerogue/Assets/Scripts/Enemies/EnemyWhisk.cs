using System.Collections;
using UnityEngine;

public class EnemyWhisk : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 360f;
    public float spinDuration = 2f;
    public int damage = 10;
    public float attackRange = 1.5f;

    private Transform player;
    private bool isSpinning = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!isSpinning)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                StartCoroutine(Spin());
            }
            else
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private IEnumerator Spin()
    {
        isSpinning = true;
        float timer = 0f;

        while (timer < spinDuration)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            timer += Time.deltaTime;

            yield return null;
        }

        isSpinning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSpinning && collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}