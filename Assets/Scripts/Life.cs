using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] public int life = 10;
    public static Life main;
    public event Action<int> OnLifeChanged; // life ���� ����� �� ȣ���� �̺�Ʈ

    private void Awake()
    {
        main = this;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LifeDown();
        }
    }

    public void LifeDown()
    {
        life -= 1;
        // �̺�Ʈ ȣ��
        OnLifeChanged?.Invoke(life);
    }
}
