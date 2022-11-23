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
        LobbyManagerRenewal.Instance.JoinOrCreateRoom();
    }

    public void GetOutMatchingBtn()
    {
        SoundManager.Instance.PlaySFXOneShot("CancelEffect");
        if (LobbyManagerRenewal.Instance.TryGetOutRoom())
            SceneManager.LoadScene("Lobby_Main");
    }

    public void MatchBtnSetFalse()
    {
        matchingOutBtn.gameObject.SetActive(false);
        matchingOutCantBtn.gameObject.SetActive(true);
    }

    public void SetWaitingState()
    {
        plusPlayerWait.SetActive(true);
        plusPlayerWaitText.SetActive(true);
    }

    private void FixedUpdate()
    {
        rotateImage.transform.Rotate(Vector3.forward * rotateSpeed);
        if(true == plusPlayerWait.activeInHierarchy)
            plusPlayerWaitTimeText.text = LobbyManagerRenewal.Instance.WaitTimeRemain.ToString();


    }

}
