using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform[] points;
    public float speed = 2f;
    private int currentPointIndex = 0;
    private PlayerUIController playerUI;

    void Start()
    {
        playerUI = FindObjectOfType<PlayerUIController>();
    }

    void Update()
    {
        if (points.Length == 0) return;

        Transform targetPoint = points[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % points.Length;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (playerUI != null)
        {
            playerUI.Die("Why you died from ghost?");
        }
        Destroy(gameObject);
    }
}
