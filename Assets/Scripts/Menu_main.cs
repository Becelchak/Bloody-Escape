using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_main : MonoBehaviour
{
    private CanvasGroup thisGroup;
    private CanvasGroup infoGroup;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        thisGroup = transform.Find("MainMenu").GetComponent<CanvasGroup>();
        infoGroup = transform.Find("InfoMenu").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueGame()
    {
        thisGroup.alpha = 0;
        thisGroup.interactable = false;
        thisGroup.blocksRaycasts = false;
        player.GetComponent<PlayerControl>().ChangeMenuStatus();
    }

    public void InfoAboutGame()
    {
        ContinueGame();
        infoGroup.alpha = 1;
        infoGroup.interactable = true;
        infoGroup.blocksRaycasts = true;

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackFromInfoMenu()
    {
        infoGroup.alpha = 0;
        infoGroup.interactable = false;
        infoGroup.blocksRaycasts = false;

        thisGroup.alpha = 1;
        thisGroup.interactable = true;
        thisGroup.blocksRaycasts = true;
        player.GetComponent<PlayerControl>().ChangeMenuStatus();
    }
}
