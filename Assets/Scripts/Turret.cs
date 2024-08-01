using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;//transformŬ������ ���°��� ������Ʈ�� �����Ͽ� �־��� �ʵ�
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;//GameObjectŬ������ ���°��� �������� �����Ͽ� �־��� �ʵ�
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;



    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f; // �ʴ� �Ѿ� ��
    [SerializeField] private int baseUpgradeCost = 100;


    private float bpsBase;
    private float targetingRangeBase;
    

    private Transform target;
    private float timeUntilFire;

    private int level = 1;


    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

       if(!CheckTargetIsInRange())
        {
            target = null;
        }
       else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)//�ѽ���İ���ð� >= �Ѿ� �߻� �ֱ�
            {
                Shoot();
                Flash();
                timeUntilFire = 0f;
            }
        }
    }
    private void Flash()
    {
        muzzleFlash.SetActive(true);
        StartCoroutine(FlashOut());
    }

    private IEnumerator FlashOut()
    {
        yield return new WaitForSeconds(0.1f);

        muzzleFlash.SetActive(false);
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
             target = hits[0].transform;
        }

    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - 
        transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));//���Ϸ����� ���ʹϾ����� ��ȯ
        turretRotationPoint.rotation = /*targetRotation;*/Quaternion.RotateTowards/*��ġȸ�������Ҷ� ȸ������*/(turretRotationPoint.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (baseUpgradeCost > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;

        bps = CalculateBPS();
        targetingRange = CalculateRange();

        CloseUpgradeUI();
        Debug.Log("New BPS :" + bps);
        Debug.Log("New BPS :" + targetingRange);
        Debug.Log("New BPS :" + CalculateCost());

    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.5f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
