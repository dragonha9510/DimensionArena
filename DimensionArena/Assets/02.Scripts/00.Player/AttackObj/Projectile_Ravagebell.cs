using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile_Ravagebell : Projectile
{
    [SerializeField] private GameObject PoisonTile;

    protected override void OnTriggerEnter(Collider other)
    {
        isWater = false;
        if (PhotonNetwork.OfflineMode)
        {
            switch (other.tag)
            {
                //��� Player���� �������� �� ���, 
                case "Player":
                    {
                        if (ownerID != other.gameObject.name)
                        {
                            photonView.RPC("OnCollisionToPlayer",
                            RpcTarget.All,
                            ownerID,
                            other.gameObject.name,
                            other.transform.position);
                        }
                        else
                            return;
                    }
                    break;
                //Damaged�� Obstacle ����ü �������� ��¦ ��鸮�� ���
                case "ParentObstacle":
                    {
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                    break;
                case "Water":
                    isWater = true;
                    break;
                case "Player_Detection":
                    return;
                default:
                    break;
            }

            if (isWater)
                return;

            Quaternion rot = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);/*Quaternion.FromToRotation(Vector3.up, contact.normal);*/
            Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, pos, rot);
                var psHit = hitVFX.GetComponent<ParticleSystem>();
                if (psHit != null)
                    Destroy(hitVFX, psHit.main.duration);
                else
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
            }
            Vector3 fieldPosition = transform.position;
            fieldPosition.y = 0.038f;

            Instantiate(PoisonTile, fieldPosition, PoisonTile.transform.rotation);
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            switch (other.tag)
            {
                //��� Player���� �������� �� ���, 
                case "Player":
                    {
                        if (ownerID != other.gameObject.name)
                        {
                            photonView.RPC("OnCollisionToPlayer",
                            RpcTarget.All,
                            ownerID,
                            other.gameObject.name,
                            other.transform.position);
                        }
                        else
                            return;
                    }
                    break;
                //Damaged�� Obstacle ����ü �������� ��¦ ��鸮�� ���
                case "ParentObstacle":
                    {
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                    break;
                case "Water":
                    isWater = true;
                    break;
                case "Player_Detection":
                    return;
                default:
                    break;
            }

            if (isWater)
                return;

            Quaternion rot = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);/*Quaternion.FromToRotation(Vector3.up, contact.normal);*/
            Vector3 pos = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (hitPrefab != null)
            {
                var hitVFX = PhotonNetwork.Instantiate(hitPrefab.name, pos, rot);
                var psHit = hitVFX.GetComponent<ParticleSystem>();
                if (psHit != null)
                    Destroy(hitVFX, psHit.main.duration);
                else
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
            }

            Vector3 fieldPosition = transform.position;
            fieldPosition.y = 0.038f;

            PhotonNetwork.Instantiate(PoisonTile.name, fieldPosition, PoisonTile.transform.rotation);
        }
    }
}
