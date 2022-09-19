using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TP_Manager : MonoBehaviour
{
    public static TP_Manager Instance;

    [Serializable]
    public struct PairCollider
    {
        public Collider2D doorOne;
        public Collider2D doorTwo;
    }

    [SerializeField] List<PairCollider> listPairDoor;

    private Dictionary<Collider2D, Collider2D> dictHouseDoor = new Dictionary<Collider2D, Collider2D>();
    public Dictionary<Collider2D, Collider2D>  DictHouseDoor { get => dictHouseDoor; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        foreach (PairCollider pair in listPairDoor)
        {
            dictHouseDoor.Add(pair.doorOne, pair.doorTwo);
            dictHouseDoor.Add(pair.doorTwo, pair.doorOne);
        }
    }
}
