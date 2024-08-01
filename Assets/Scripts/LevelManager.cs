using UnityEngine;


public class LevelManager : MonoBehaviour
{//LevelManagerŸ���� ���� �����ʵ� ����-> �����Ҽ��ִ� LevelManger �ν��Ͻ��� �ϳ��� ������ �ǹ�
    public static LevelManager main;
    //GameObject�� ��ġ,ȸ����ũ�⸦ �����ϴ� ��������� TransformŬ������ ����Ͽ�
    public Transform startPoint;//startPoint�����ʵ� ����
    public Transform[] path;//path�����ʵ� ����


    public int currency;


    private void Awake()//Awake()�޼���� ��ũ��Ʈ �ν��Ͻ��� �ε�ɶ� ȣ���
    {
        main = this;//main�����ʵ尡 LevelManager�ν��Ͻ��� �����ϵ��� �Ѵ�.
    }


    private void Start()
    {
        currency = 300;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if(amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item");
            return false;
        }
    }

    public void Heart()
    {

    }
}
