using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professor : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        BlockPlayerPNJ.Instance.CanPass = true;
    }
}
