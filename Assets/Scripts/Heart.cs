using UnityEngine;

public class Heart : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Life 클래스의 이벤트 구독
        Life.main.OnLifeChanged += OnLifeChangedHandler;
    }

    void OnDestroy()
    {
        // 구독 해제
        Life.main.OnLifeChanged -= OnLifeChangedHandler;
    }

    void OnLifeChangedHandler(int life)
    {
        // life가 1 줄어들 때마다 애니메이션을 실행
        animator.Play("Heart", 0, Time.time);
    }
}
