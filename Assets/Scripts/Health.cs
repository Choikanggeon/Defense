using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    public GameObject bombPrefab; // ��ź �������� �����մϴ�.
    private bool isDestroyed = false;

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if(hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();//����ִ� ������ 1����
            LevelManager.main.IncreaseCurrency(currencyWorth);//�� 50�� ����
            isDestroyed = true;//�� �ı��� ����
            Destroy(gameObject);//�� �ı�
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rocket")) // �̻��ϰ� �浹�ߴ��� Ȯ���մϴ�.
        {
            Vector3 explosionPosition = transform.position; // ���� �浹�� ��ġ�� �����ɴϴ�.
            CreateBomb(explosionPosition); // ��ź�� �����մϴ�.
        }
    }

    void CreateBomb(Vector3 position)
    {
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity); // ��ź�� �����մϴ�.
    }

    
}
