using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoubleTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;//transform클래스를 쓰는것은 오브젝트를 참조하여 넣어줄 필드
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;//GameObject클래스를 쓰는것은 프리팹을 참조하여 넣어줄 필드
    [SerializeField] private GameObject muzzleFlash1;
    [SerializeField] private GameObject muzzleFlash2;
    [SerializeField] private Transform firingPoint1;
    [SerializeField] private Transform firingPoint2;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;



    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 0.25f; // 초당 총알 수
    [SerializeField] private int baseUpgradeCost = 120;


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

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1.5f / bps)//총쏘고난후경과시간 >= 총알 발사 주기 1.5초후에 메서드 실행
            {
                Shoot();
                
                timeUntilFire = 0f;
            }
        }
    }
    

    private IEnumerator FlashOut(GameObject muzzleFlash)
    {
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);
    }

    

    private void Shoot()
    {
        
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint1.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        muzzleFlash1.SetActive(true);
        StartCoroutine(FlashOut(muzzleFlash1));
        StartCoroutine(LateShoot());
    }

    private IEnumerator LateShoot()
    {
        yield return new WaitForSeconds(0.75f);
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint2.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        muzzleFlash2.SetActive(true);
        StartCoroutine(FlashOut(muzzleFlash2));
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

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));//오일러각을 쿼터니언으로 변환
        turretRotationPoint.rotation = /*targetRotation;*/Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
