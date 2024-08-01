using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private GameObject explosionPrefab;


    [Header("Attributes")]
    [SerializeField] private float rocketSpeed = 5f;
    [SerializeField] private int rocketDamage = 1;
    [SerializeField] private float bombRadius = 1.5f;

    public Transform target;
    private bool isNotExplosion = true;

    
    public void SetTarget(Transform _target)//Å¸°Ù¼³Á¤
    {
        target = _target;
    }
    private void Start()
    {
        if(isNotExplosion)
        StartCoroutine(RocketLost());
    }
    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * rocketSpeed;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Explode();
    }


    private IEnumerator RocketLost()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    

    private void Explode()
    {
        isNotExplosion = false;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<Health>().TakeDamage(rocketDamage);
            }
        }
        Destroy(gameObject);
    }
}
