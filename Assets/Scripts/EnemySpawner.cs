using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]//����
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]//Ư��
    [SerializeField] private int baseEnemies = 8; //�⺻ ���� ����
    [SerializeField] private float enemiesPerSecond = 0.5f; //�ʴ� ���� �� ��
    [SerializeField] private float timeBetweenWaves = 5f; //���̺갣 ���ð�
    [SerializeField] private float difficultyScalingFactor = 0.75f;//���̵� ���� ���
    [SerializeField] private float enemiesPerSecondCap = 15f;
    [Header("Events")]//�̺�Ʈ�߻�
    public static UnityEvent onEnemyDestroy = new UnityEvent();


    private int currentWave = 1;//���� ���̺�
    private float timeSinceLastSpawn;//������ ���� �������� ����ð�
    private int enemiesAlive;//���� ����ִ� ���� ��
    private int enemiesLeftToSpawn;//����Wave���� ���� �����ؾ��� ���Ǽ�
    private float eps; //�ʴ� �� ��������
    private bool isSpawning = false;//���� ���̺꿡�� ���������ϰ��ִ��� ����

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed); //�̺�Ʈ Ŭ������ �ν��Ͻ�onEnemyDestroy�� EnemyDestroyed()�޼��带 ȣ���ϴ� ��ư�� �߰��Ѵ�.
    }

    private void Start()
    {
        //enemiesLeftToSpawn = baseEnemies;
       StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime; //���������� �������� ����ð� �þ����
        //������ ���� �������� ����� �ð��� ���� �����ؾߵǴ� �ֱ⸦ �Ѱų� ���� �����ؾߵǴ� �� ���ڰ� 0����ũ�� if�� ����
        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn>0)
        {//���������� �������� ����ð�2�� >= �ʴ������������ ������ ������ �ɸ��½ð�
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

    private int EnemiesPerWave()//���̺긶�� ������ ����
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0f, enemiesPerSecondCap);
    }
}
