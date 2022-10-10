using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnLobbyButton : MonoBehaviour
{
    public void gotoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
