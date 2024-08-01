using UnityEngine;


public class LevelManager : MonoBehaviour
{//LevelManager타입의 공개 정적필드 선언-> 접근할수있는 LevelManger 인스턴스가 하나만 있음을 의미
    public static LevelManager main;
    //GameObject의 위치,회전및크기를 저장하는 구성요소인 Transform클래스를 사용하여
    public Transform startPoint;//startPoint공개필드 선언
    public Transform[] path;//path공개필드 선언


    public int currency;


    private void Awake()//Awake()메서드는 스크립트 인스턴스가 로드될때 호출됨
    {
        main = this;//main정적필드가 LevelManager인스턴스를 참조하도록 한다.
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
