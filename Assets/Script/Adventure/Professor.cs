using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professor : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueID[] dialogue;

    public void Interact()
    {
        BlockPlayerPNJ.Instance.CanPassNow();
        DialogueManager.Instance.InitDialogue(this, dialogue);
    }
}
