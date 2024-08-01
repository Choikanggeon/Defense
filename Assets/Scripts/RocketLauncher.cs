using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RocketLauncher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;//transform클래스를 쓰는것은 오브젝트를 참조하여 넣어줄 필드
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject rocketPrefab;//GameObject클래스를 쓰는것은 프리팹을 참조하여 넣어줄 필드
    [SerializeField] private Transform firingPoint1;
    [SerializeField] private Transform firingPoint2;
    [SerializeField] private GameObject buster;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject rocket1;
    [SerializeField] private GameObject rocket2;



    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 0.25f; // 초당 총알 수
    [SerializeField] private int baseUpgradeCost = 120;

    private float bpsBase;
    private float targetingRangeBase;
    private Transform target;
    private float timeUntilFire;
    private bool isRocketReady = true;
    private bool isFirstShot = true;

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

        
        if (CheckTargetIsInRange() && isFirstShot)
        {
            Shoot();
        }

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1.5f / bps && isRocketReady)
            {
                Shoot();

                timeUntilFire = 0f;
            }
        }
    }





    private void Shoot()
    {
        if (isFirstShot)
        {
            StartCoroutine(LaunchRocket());
            isFirstShot = false;
        }
        else
        {
            StartCoroutine(LaunchRocket());
        }
    }

    private IEnumerator LaunchRocket()
    {
        isRocketReady = false;
        buster.SetActive(true);
        rocket1.SetActive(false);
        GameObject rocketObj1 = Instantiate(rocketPrefab, firingPoint1.position, Quaternion.identity);
        Rocket rocketScript1 = rocketObj1.GetComponent<Rocket>();
        rocketScript1.SetTarget(target);
        buster.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        buster.SetActive(true);
        rocket2.SetActive(false);
        GameObject rocketObj2 = Instantiate(rocketPrefab, firingPoint2.position, Quaternion.identity);
        Rocket rocketScript2 = rocketObj2.GetComponent<Rocket>();
        rocketScript2.SetTarget(target);
        buster.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        rocket1.SetActive(true);
        rocket2.SetActive(true);
        isRocketReady = true;
        
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
