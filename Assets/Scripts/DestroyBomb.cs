using System.Collections;
using UnityEngine;

public class DestroyBomb : MonoBehaviour
{
    private float bombLifeTime = 0.4f; 
    void Start()
    {
        StartCoroutine(Destroybomb());
    }

    private IEnumerator Destroybomb()
    {
        yield return new WaitForSeconds(bombLifeTime);
        Destroy(gameObject);
    }

}
