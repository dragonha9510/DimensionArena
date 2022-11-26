using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;
public class Parabola_Projectile : AttackObject
{
    // Destroy Effect
    [SerializeField] private GameObject destroyEffect;

    // 0 ~ 1 鳖瘤 啊绰 加档.
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

    // 雀傈
    [SerializeField] private float rotationSpeed = 180f;
    private Vector3 rotationAxis;


    [SerializeField] private bool delayDestroy = false;
    [SerializeField] private float delayTime = 1;
    private bool isDestroy;

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

    private IEnumerator DelayDestroy()
    {
        isDestroy = true;
        yield return new WaitForSeconds(delayTime);
        Destroy(this.gameObject);
        GetComponent<KnockBackObject>().KnockBackStartDamage(ownerID, Damage, ultimatePoint);

        if (destroyEffect != null)
        {
            GameObject fxSize = Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation);
            fxSize.transform.localScale *= tempRatio;
        }
    }

    private IEnumerator DelayDestroy_Server()
    {
        isDestroy = true;
        yield return new WaitForSeconds(delayTime);
        GetComponent<KnockBackObject>().KnockBackStartDamage(ownerID, Damage, ultimatePoint);

        // Effect 积己
        if (destroyEffect != null)
        {
            photonView.RPC(nameof(CreateDestroyEffect), RpcTarget.All, transform.position, destroyEffect.transform.rotation, tempRatio);
            PhotonNetwork.Destroy(this.gameObject);
        }

    }

    private void Update()
    {
        if (isDestroy)
            return;

        if(!PhotonNetwork.InRoom)
        {
            if (!isReady)
                return;

            myLocation += realSpeed * Time.deltaTime;
            transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

            if (myLocation > 1)
            {
                if (!delayDestroy)
                {
                    Destroy(this.gameObject);

                    // Effect 积己
                    GetComponent<KnockBackObject>().KnockBackStartDamage(ownerID, Damage, ultimatePoint);

                    if (destroyEffect != null)
                    {
                        GameObject fxSize = Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation);
                        fxSize.transform.localScale *= tempRatio;
                    }
                }
                else
                    StartCoroutine(DelayDestroy());
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
                if (!delayDestroy)
                {
                    GetComponent<KnockBackObject>().KnockBackStartDamage(ownerID, Damage, ultimatePoint);

                    // Effect 积己
                    if (destroyEffect != null)
                    {
                        photonView.RPC(nameof(CreateDestroyEffect), RpcTarget.All, transform.position, destroyEffect.transform.rotation, tempRatio);
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                }
                else
                    StartCoroutine(DelayDestroy_Server());
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
