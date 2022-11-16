using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class MatchMaking : MonoBehaviourPun
{
    [SerializeField]
    private float rotateSpeed = 2f;
    [SerializeField]
    private GameObject rotateImage;
    [SerializeField]
    private Button matchingOutBtn;
    [SerializeField]
    private Button matchingOutCantBtn;

    [SerializeField]
    private GameObject plusPlayerWait;
    [SerializeField]
    private GameObject plusPlayerWaitText;
    [SerializeField]
    private TextMeshProUGUI plusPlayerWaitTimeText;

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
            do
            {
                plusPlayerWait.SetActive(true);
                plusPlayerWaitText.SetActive(true);
            } while (plusPlayerWait.activeInHierarchy == false
            && plusPlayerWaitText.activeInHierarchy == false);
            plusPlayerWaitTimeText.text = LobbyManagerRenewal.Instance.WaitTimeRemain.ToString();

            matchingOutBtn.gameObject.SetActive(false);
            matchingOutCantBtn.gameObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        rotateImage.transform.Rotate(Vector3.forward * rotateSpeed);
    }

}
