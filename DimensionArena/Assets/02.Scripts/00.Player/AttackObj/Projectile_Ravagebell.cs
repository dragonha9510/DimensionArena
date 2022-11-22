using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile_Ravagebell : Projectile
{
    [SerializeField] private GameObject PoisonTile;
    private bool isSound;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        GameObject poisonFieldTemp;

        if (!isSound)
        {
            Debug.Log("스플래시");
            SoundManager.Instance.PlaySFXOneShot("ravagebell_splash");
            isSound = true;
        }

        if (!PhotonNetwork.InRoom)
        {
            SoundManager.Instance.PlaySFXOneShotInRange(30.0f, this.transform, "ravagebell_splash");
            switch (other.tag)
            {
                //상대 Player에게 데미지를 준 경우, 
                case "Player":
                //Damaged된 Obstacle 공격체 방향으로 살짝 흔들리는 모션
                case "ParentObstacle":
                case "ParentGround":
                        Destroy(this.gameObject);
                    break;
                default:
                    return;
            }

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

            Debug.Log("생성");
            poisonFieldTemp = Instantiate(PoisonTile, fieldPosition, PoisonTile.transform.rotation);
        }
        else
        {           
            if (!PhotonNetwork.IsMasterClient)
                return;

            switch (other.tag)
            {
                //상대 Player에게 데미지를 준 경우, 
                case "Player":
                case "ParentObstacle":
                case "ParentGround":
                    {
                        Debug.Log(photonView.ViewID);
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                    break;
                case "Item_Box":
                    break;
                default:
                    return;
            }

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

            poisonFieldTemp = PhotonNetwork.Instantiate(PoisonTile.name, fieldPosition, PoisonTile.transform.rotation);           
        }

        if(poisonFieldTemp != null)
        {
            TickDamage temp = poisonFieldTemp.GetComponent<TickDamage>();
            temp.OwnerID = ownerID;
            temp.ultimatePoint = ultimatePoint;
        }
    }


}
