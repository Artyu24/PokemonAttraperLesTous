using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professor : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueID[] dialogue;

    public void Interact()
    {
        BlockPlayerPNJ.Instance.CanPass = true;
        DialogueManager.Instance.InitDialogue(this, dialogue);
    }
}
