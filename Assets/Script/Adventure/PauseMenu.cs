using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [SerializeField] private GameObject pauseMenu;
    private RectTransform selection;
    private int actualSelection = 0;
    private RectTransform[] tabMenuObject;
    private bool isOpen = false;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        tabMenuObject = new RectTransform[pauseMenu.transform.childCount - 1];
        for (int i = 0; i < tabMenuObject.Length; i++)
        {
            tabMenuObject[i] = pauseMenu.transform.GetChild(i).GetComponent<RectTransform>();
        }

        selection = pauseMenu.transform.GetChild(pauseMenu.transform.childCount - 1).GetComponent<RectTransform>();
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

        //Changer sur la bonne fonction
    }

    private void ChangeSelection()
    {
        tabMenuObject[actualSelection].gameObject.SetActive(true);
        selection.anchoredPosition = new Vector3(0, tabMenuObject[actualSelection].anchoredPosition.y);
    }
}
