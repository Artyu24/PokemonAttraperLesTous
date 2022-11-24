using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [SerializeField] private GameObject pauseMenu;
    private RectTransform selection;
    private int actualSelection = 0;
    private RectTransform[] tabMenuObject;
    private bool isOpen = false;

    private delegate void MenuDelegate();
    private Dictionary<int, MenuDelegate> dicDelegateSelection = new Dictionary<int, MenuDelegate>();

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


        dicDelegateSelection.Add(1, ActivateMap);

        dicDelegateSelection.Add(3, SaveParty);

    }

    public void OpenPauseMenu()
    {
        if (!isOpen)
        {
            pauseMenu.SetActive(true);
            ChangeSelection();
            GameManager.Instance.ActualGameState = GameState.Paused;
        }
        else
        {
            pauseMenu.SetActive(false);
            GameManager.Instance.ActualGameState = GameState.Adventure;
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

        if (actualSelection == 3)
            dicDelegateSelection[3]();
        //Changer sur la bonne fonction
    }

    private void ChangeSelection()
    {
        tabMenuObject[actualSelection].gameObject.SetActive(true);
        selection.anchoredPosition = new Vector3(0, tabMenuObject[actualSelection].anchoredPosition.y);
    }

    private void ActivateMap()
    {
        //Afficher la map
    }

    private void SaveParty()
    {
        SaveSystem.SaveSettingData();
    }

    private void Nothing()
    {
        Debug.Log("Ya r");
        OpenPauseMenu();
    }
}
