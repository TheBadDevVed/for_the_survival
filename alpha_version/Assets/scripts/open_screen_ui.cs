using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class open_screen_ui: MonoBehaviour
{
    public static int flag;
    public void startgame(int n)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        flag = n;
    }

    // Update is called once per frame
    public void quit()
    {
        Application.Quit();
    }


}
