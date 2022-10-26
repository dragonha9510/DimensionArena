using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MatchingStartButton : MonoBehaviour
{
    public void MatchMakingSceneLoad()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");

        if (LobbyManagerRenewal.Instance.playMode != MODE.MODE_TRAINING)
            SceneManager.LoadScene("MatchMaking");
        else
            LobbyManagerRenewal.Instance.EnterTrainingMode();
    }



}
