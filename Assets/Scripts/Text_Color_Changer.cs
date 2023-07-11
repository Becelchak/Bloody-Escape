using UnityEngine.UI;
using UnityEngine;

public class Text_Color_Changer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EditorOnly")
            GetComponentInChildren<Text>().color = Color.white;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EditorOnly")
            GetComponentInChildren<Text>().color = Color.black;
    }
}
