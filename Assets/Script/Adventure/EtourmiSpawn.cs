using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EtourmiSpawn : MonoBehaviour
{
    private GameObject etourmi;
    [SerializeField] private int nbrEtourmi;

    private void Awake()
    {
        etourmi = Resources.Load<GameObject>("Etourmi");
    }

    private void Start()
    {
        StartCoroutine(SpawnFirstEtourmi());
    }

    private IEnumerator SpawnFirstEtourmi()
    {
        for (int i = 0; i < nbrEtourmi; i++)
        {
            StartCoroutine(SpawnEtourmi());
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator SpawnEtourmi()
    {
        float randomTime = Random.Range(3, 8);
        yield return new WaitForSeconds(randomTime);
        float randomY = Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2);
        GameObject etourmiSpawn = Instantiate(etourmi, new Vector3(transform.position.x, randomY), Quaternion.identity);
        float randomDuration = Random.Range(10, 20);
        etourmiSpawn.transform.DOMoveX(-68.4f, randomDuration).SetEase(Ease.Linear).OnComplete(() => Respawn(etourmiSpawn));
    }

    private void Respawn(GameObject etourmi)
    {
        Destroy(etourmi);
        StartCoroutine(SpawnEtourmi());
    }
}
