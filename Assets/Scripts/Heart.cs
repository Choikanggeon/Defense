using UnityEngine;

public class Heart : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Life Ŭ������ �̺�Ʈ ����
        Life.main.OnLifeChanged += OnLifeChangedHandler;
    }

    void OnDestroy()
    {
        // ���� ����
        Life.main.OnLifeChanged -= OnLifeChangedHandler;
    }

    void OnLifeChangedHandler(int life)
    {
        // life�� 1 �پ�� ������ �ִϸ��̼��� ����
        animator.Play("Heart", 0, Time.time);
    }
}
