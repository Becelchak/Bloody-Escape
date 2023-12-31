using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons_funcions : MonoBehaviour
{
    public Collider2D cursor;
    public GameObject mainMenu;

    void Update()
    {
        if(cursor != null)
            cursor.gameObject.transform.position = Input.mousePosition;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Locations");
    }

    public void CloseGame()
    {
        //EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void OpenInfoMenu()
    {
        mainMenu.GetComponent<CanvasGroup>().alpha = 0f;
        mainMenu.GetComponent<CanvasGroup>().interactable = false;
        mainMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
        
    }

    public void CloseInfoMenu()
    {
        mainMenu.GetComponent<CanvasGroup>().alpha = 1f;
        mainMenu.GetComponent<CanvasGroup>().interactable = true;
        mainMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Locations");
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
