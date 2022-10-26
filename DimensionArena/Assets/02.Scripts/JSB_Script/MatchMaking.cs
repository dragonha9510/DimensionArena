using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MatchMaking : MonoBehaviourPun
{

    [SerializeField]
    private Button matchingOutBtn;
    [SerializeField]
    private Button matchingOutCantBtn;
    // Start is called before the first frame update
    void Start()
    {
        LobbyManagerRenewal.Instance.JoinOrCreateRoom(MODE.MODE_SURVIVAL);
    }

    public void GetOutMatchingBtn()
    {
        SoundManager.Instance.PlaySFXOneShot("CancelEffect");
        if (LobbyManagerRenewal.Instance.TryGetOutRoom())
            SceneManager.LoadScene("Lobby_Main");
    }

    private void Update()
    {
        if(PhotonNetwork.InRoom && LobbyManagerRenewal.Instance.IsWillStartGame)
        {
            matchingOutBtn.gameObject.SetActive(false);
            matchingOutCantBtn.gameObject.SetActive(true);
        }
    }


}
