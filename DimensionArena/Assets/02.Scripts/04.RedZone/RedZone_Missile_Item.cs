using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

using Photon.Pun;
public class RedZone_Missile_Item : RedZone_Missile
{
    [SerializeField] private GameObject itemBox;
    private bool isCreate;

    protected override void OnTriggerEnter(Collider other)
    {
        if (itemBox == null)
            return;

        if (other.name != gameObject.name && !isCreate)
        {
            base.OnTriggerEnter(other);
            if(!PhotonNetwork.IsConnected)
            {
                Instantiate(itemBox,
               new Vector3((float)((int)transform.position.x + 0.5f),
               0.5f,
               (float)((int)transform.position.z + 0.5f)),
               itemBox.transform.rotation);
            }
            else
            {
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_ITEMPREFABFOLDER + "ItemBox"
                                            , new Vector3((float)((int)transform.position.x + 0.5f), 0.5f, (float)((int)transform.position.z + 0.5f))
                                            , Quaternion.identity);
            }

            isCreate = true;
        }
    }
}
