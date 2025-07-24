using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("Game");
    }
    public void quit()
    {
        Application.Quit();// thoat luon game
        Debug.Log("Quit Game");
    }
    public void back()
    {
        SceneManager.LoadScene(0);
    }
}