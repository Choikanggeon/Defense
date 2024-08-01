using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    public GameObject bombPrefab; // 폭탄 프리팹을 설정합니다.
    private bool isDestroyed = false;

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if(hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();//살아있는 적숫자 1감소
            LevelManager.main.IncreaseCurrency(currencyWorth);//돈 50원 증가
            isDestroyed = true;//적 파괴됨 감지
            Destroy(gameObject);//적 파괴
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rocket")) // 미사일과 충돌했는지 확인합니다.
        {
            Vector3 explosionPosition = transform.position; // 적과 충돌한 위치를 가져옵니다.
            CreateBomb(explosionPosition); // 폭탄을 생성합니다.
        }
    }

    void CreateBomb(Vector3 position)
    {
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity); // 폭탄을 생성합니다.
    }

    
}
