using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] public int life = 10;
    public static Life main;
    public event Action<int> OnLifeChanged; // life 값이 변경될 때 호출할 이벤트

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
        // 이벤트 호출
        OnLifeChanged?.Invoke(life);
    }
}
