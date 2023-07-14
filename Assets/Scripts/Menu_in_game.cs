using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_in_game : MonoBehaviour
{
    public CanvasGroup thisGroup;
    public CanvasGroup infoGroup;
    public GameObject player;
    
    void Start()
    {
        player.GetComponent<Player_Control>().ChangeMenuNow(thisGroup);
    }

    public void ContinueGame()
    {
        thisGroup.alpha = 0;
        thisGroup.interactable = false;
        thisGroup.blocksRaycasts = false;
        player.GetComponent<Player_Control>().ChangeMenuStatus();
    }

    public void InfoAboutGame()
    {
        thisGroup.alpha = 0;
        thisGroup.interactable = false;
        thisGroup.blocksRaycasts = false;

        infoGroup.alpha = 1;
        infoGroup.interactable = true;
        infoGroup.blocksRaycasts = true;

        player.GetComponent<Player_Control>().ChangeMenuNow(infoGroup);
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
    }

    public void BackFromInfoMenu()
    {
        infoGroup.alpha = 0;
        infoGroup.interactable = false;
        infoGroup.blocksRaycasts = false;

        thisGroup.alpha = 1;
        thisGroup.interactable = true;
        thisGroup.blocksRaycasts = true;

        player.GetComponent<Player_Control>().ChangeMenuNow(thisGroup);
    }
}
