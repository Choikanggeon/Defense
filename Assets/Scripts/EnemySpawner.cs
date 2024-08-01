using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]//참조
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]//특성
    [SerializeField] private int baseEnemies = 8; //기본 몬스터 숫자
    [SerializeField] private float enemiesPerSecond = 0.5f; //초당 생성 적 수
    [SerializeField] private float timeBetweenWaves = 5f; //웨이브간 대기시간
    [SerializeField] private float difficultyScalingFactor = 0.75f;//난이도 조절 계수
    [SerializeField] private float enemiesPerSecondCap = 15f;
    [Header("Events")]//이벤트발생
    public static UnityEvent onEnemyDestroy = new UnityEvent();


    private int currentWave = 1;//현재 웨이브
    private float timeSinceLastSpawn;//마지막 적이 생성된후 경과시간
    private int enemiesAlive;//현재 살아있는 적의 수
    private int enemiesLeftToSpawn;//현재Wave에서 아직 생성해야할 적의수
    private float eps; //초당 적 생성숫자
    private bool isSpawning = false;//현재 웨이브에서 적을생성하고있는지 여부

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed); //이벤트 클래스의 인스턴스onEnemyDestroy에 EnemyDestroyed()메서드를 호출하는 버튼을 추가한다.
    }

    private void Start()
    {
        //enemiesLeftToSpawn = baseEnemies;
       StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime; //마지막적이 생성된후 경과시간 늘어나는중
        //마지막 적이 생성된후 경과된 시간이 적을 생성해야되는 주기를 넘거나 같고 생성해야되는 적 숫자가 0보다크면 if문 실행
        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn>0)
        {//마지막적이 생성된후 경과시간2초 >= 초당생성마리수의 역수인 마리당 걸리는시간
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);


        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn  = enemyPrefabs[index];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()//웨이브마다 적몬스터 숫자
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0f, enemiesPerSecondCap);
    }
}
