using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;
public class Parabola_Projectile : AttackObject
{
    // Destroy Effect
    [SerializeField] private GameObject destroyEffect;

    // 0 ~ 1 까지 가는 속도.
    // realSpeed = 1 / time
    [SerializeField] private float time;
    private float realSpeed;
    private float myLocation;

    private float gravity;

    private float distance;
    private float velocity;
    private float maxY;
    private Vector3 direction;
    private Vector3 oriPosition;
    private bool isReady;

    [SerializeField] private float tempRatio = 1.5f;

    // 회전
    [SerializeField] private float rotationSpeed = 180f;
    private Vector3 rotationAxis;

    public void SetArcInfo(Vector3 dir, float dist, float vel, float ypos)
    {
        if(!PhotonNetwork.OfflineMode)
            photonView.RPC(nameof(SetAllClientArcInfo), RpcTarget.All, dir, dist, vel, ypos);
        else
            SetAllClientArcInfo(dir, dist, vel, ypos);

    }


    [PunRPC]
    public void SetAllClientArcInfo(Vector3 dir, float dist, float vel, float ypos)
    {
        rotationAxis = new Vector3(Random.Range(0.5f, 2f), Random.Range(0.5f, 2), Random.Range(0.5f, 2));
        oriPosition = transform.position;
        direction = dir.normalized;
        distance = dist;
        velocity = vel;
        maxY = ypos;
        isReady = true;
    }

    public void ClientArcInfo(Vector3 dir, float dist, float vel, float ypos)
    {
        rotationAxis = new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
        oriPosition = transform.position;
        direction = dir.normalized;
        distance = dist;
        velocity = vel;
        maxY = ypos;
        isReady = true;
    }


    private void Awake()
    {
        gravity = Mathf.Abs(Physics.gravity.y);
        realSpeed = 1 / time;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if(!PhotonNetwork.InRoom)
        {
            if (!isReady)
                return;

            myLocation += realSpeed * Time.deltaTime;
            transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

            if (myLocation > 1)
            {
                Destroy(this.gameObject);
                // Effect 생성

                GetComponent<KnockBackObject>().KnockBackStartDamage(ownerID, Damage, ultimatePoint);

                if (destroyEffect != null)
                {
                    GameObject fxSize = Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation);
                    fxSize.transform.localScale *= tempRatio;
                }
            }

            Vector3 tempPosition = oriPosition + (direction * distance * myLocation);
            tempPosition.y = CalculatePosition(myLocation);
            transform.position = tempPosition;
        }
        if(PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            if (!isReady)
                return;

            myLocation += realSpeed * Time.deltaTime;
            transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

            if (myLocation > 1)
            {
                GetComponent<KnockBackObject>().KnockBackStartDamage(ownerID, Damage, ultimatePoint);

                // Effect 생성
                if (destroyEffect != null)
                {
                    photonView.RPC(nameof(CreateDestroyEffect), RpcTarget.All , transform.position, destroyEffect.transform.rotation,tempRatio);
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }

            Vector3 tempPosition = oriPosition + (direction * distance * myLocation);
            tempPosition.y = CalculatePosition(myLocation);
            transform.position = tempPosition;
        }
    }

    [PunRPC]
    private void CreateDestroyEffect(Vector3 pos,Quaternion rot,float ratio)
    {
        SoundManager.Instance.PlaySFXOneShotInRange(18.0f, transform, audioClipName);
        GameObject fxSize = Instantiate(destroyEffect, pos, rot);
        fxSize.transform.localScale *= ratio;
    }

    float CalculatePosition(float t)
    {
        float x = t * distance;
        float y = x * ((4 * maxY) / (maxY * distance)) * ((x / distance) - 1) * -maxY;

        return y;
    }
}
