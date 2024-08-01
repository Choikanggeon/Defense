using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    public GameObject towerObj;
    public Turret turret;
    public DoubleTurret doubleturret;
    public TurretSlowmo slowmoturret;
    public RocketLauncher doublerocketturret;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (towerObj != null)
        {
            switch (BuildManager.main.selectedTower)
            {
                case 0:
                    turret.OpenUpgradeUI();
                    break;
                case 1:
                    doubleturret.OpenUpgradeUI();
                    break;
                case 2:
                    slowmoturret.OpenUpgradeUI();
                    break;
                case 3:
                    doublerocketturret.OpenUpgradeUI();
                    break;
            }
            if (UIManager.main.IsHoveringUI())//ui에 마우스커서가 올라왔을때 클릭하면
            {
                switch (BuildManager.main.selectedTower)
                {
                    case 0:
                        turret.Upgrade();
                        break;
                    case 1:
                        doubleturret.Upgrade();
                        break;
                    case 2:
                        slowmoturret.Upgrade();
                        break;
                    case 3:
                        doublerocketturret.Upgrade();
                        break;
                }
            }
            

        }
        else//tower == null
        {
            Tower towerToBuild = BuildManager.main.GetSelectedTower();

            if (towerToBuild.cost > LevelManager.main.currency)
            {
                Debug.Log("타워를 지을수 없습니다.");
                return;
            }

            LevelManager.main.SpendCurrency(towerToBuild.cost);
            towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            turret = towerObj.GetComponent<Turret>();
            doubleturret = towerObj.GetComponent<DoubleTurret>();
            slowmoturret = towerObj.GetComponent<TurretSlowmo>();
            doublerocketturret = towerObj.GetComponent<RocketLauncher>();
        }
    }
}
