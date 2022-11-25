using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private Camera mapCamera;
    [SerializeField] private Text playerName;
    private RectTransform selection;
    private int actualSelection = 0;
    private RectTransform[] tabMenuObject;
    private bool isOpen = false;

    [SerializeField] private DialogueID[] saveDialogue;

    private Dictionary<int, PlayerMovement.InteractionDelegate> dicDelegateSelection = new Dictionary<int, PlayerMovement.InteractionDelegate>();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        tabMenuObject = new RectTransform[5];
        for (int i = 0; i < tabMenuObject.Length; i++)
        {
            tabMenuObject[i] = pauseMenu.transform.GetChild(i).GetComponent<RectTransform>();

            Tween a = tabMenuObject[i].DOScale(new Vector3(1.1f, 1.1f), 0.5f);
            Tween b = tabMenuObject[i].DOScale(new Vector3(1, 1), 0.5f);
            Sequence seq = DOTween.Sequence();
            seq.Append(a).Append(b).SetLoops(-1);
        }

        selection = pauseMenu.transform.GetChild(pauseMenu.transform.childCount - 1).GetComponent<RectTransform>();

        dicDelegateSelection.Add(0, CombatManager.Instance.HealPlayerPoke);
        dicDelegateSelection.Add(1, ActivateMap);
        dicDelegateSelection.Add(2, Nothing);
        dicDelegateSelection.Add(3, SaveParty);
        dicDelegateSelection.Add(4, Nothing);
    }

    private void Start()
    {
        if (SaveSystemManager.Instance != null)
            playerName.text = SaveSystemManager.Instance.GetNameSave();
        else
            playerName.text = "PLAYER";
    }

    public void OpenPauseMenu()
    {
        if (!isOpen)
        {
            pauseMenu.SetActive(true);
            ChangeSelection();
            GameManager.Instance.ActualPlayerState = PlayerState.Inventory;
        }
        else
        {
            pauseMenu.SetActive(false);
            GameManager.Instance.ActualPlayerState = PlayerState.Idle;
            PlayerMovement.Instance.ResetInteractionFunction();
        }

        isOpen = !isOpen;
    }

    public void SwitchPauseSelection(float val)
    {
        int change = 0;
        if (val < 0)
            change++;
        else
            change--;

        if (actualSelection + change < 0 || actualSelection + change > tabMenuObject.Length - 1)
            return;

        tabMenuObject[actualSelection].gameObject.SetActive(false);
        actualSelection += change;
        ChangeSelection();
    }

    private void ChangeSelection()
    {
        tabMenuObject[actualSelection].gameObject.SetActive(true);
        selection.anchoredPosition = new Vector3(0, tabMenuObject[actualSelection].anchoredPosition.y);
        PlayerMovement.Instance.ActualInteractionDelegate = dicDelegateSelection[actualSelection];
    }

    private void ActivateMap()
    {
        if (gameCamera.isActiveAndEnabled)
        {
            pauseMenu.SetActive(false);
            isOpen = false;
            gameCamera.enabled = false;
            mapCamera.enabled = true;
            GameManager.Instance.ActualPlayerState = PlayerState.Map;
        }
        else
        {
            gameCamera.enabled = true;
            mapCamera.enabled = false;
            GameManager.Instance.ActualPlayerState = PlayerState.Idle;
            PlayerMovement.Instance.ResetInteractionFunction();
        }
    }

    private void SaveParty()
    {
        SaveSystem.SaveSettingData();
        OpenPauseMenu();
        DialogueManager.Instance.InitDialogue(this, saveDialogue);
    }

    private void Nothing()
    {
        Debug.Log("Ya r");
        OpenPauseMenu();
    }
}
