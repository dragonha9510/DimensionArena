using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class InGameServerManager : MonoBehaviourPun
{

    static private InGameServerManager Instance;

    static public InGameServerManager GetInstance()
    {
        if (Instance == null)
            Instance = new InGameServerManager();

        return Instance;
    }

    public void PhotonNetworkInstantiate(string prefabPath,Vector3 position , Quaternion quaternion)
    {
        PhotonNetwork.Instantiate(prefabPath, position, quaternion);
    }


}
