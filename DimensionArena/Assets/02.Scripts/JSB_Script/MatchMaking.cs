using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMaking : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LobbyManagerRenewal.Instance.JoinOrCreateRoom(MODE.MODE_SURVIVAL);
    }

}
