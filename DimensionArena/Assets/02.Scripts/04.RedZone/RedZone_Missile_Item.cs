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

        base.OnTriggerEnter(other);

        if (effectOn && !isCreate)
        {

            if(!PhotonNetwork.InRoom)
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

                int layerMask = (1 << LayerMask.NameToLayer("Obstacle") | 
                                 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("GroundObject_Brick") 
                               | 1 << LayerMask.NameToLayer("Bush") | 1 << LayerMask.NameToLayer("Water"));

                Vector3 position = new Vector3((float)((int)transform.position.x + 0.5f), 0.5f, (float)((int)transform.position.z + 0.5f));

                RaycastHit info;

                if (Physics.Raycast(position + Vector3.up, Vector3.down, 2.0f, layerMask))
                    return;


                PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_ITEMPREFABFOLDER + "ItemBox"
                                            , position
                                            , Quaternion.identity);
            }

            isCreate = true;
        }
    }
}
