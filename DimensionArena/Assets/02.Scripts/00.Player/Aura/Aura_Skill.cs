using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    public class Aura_Skill : Player_Skill
    {
        [SerializeField] private GameObject skillPrefab;
        [SerializeField]
        private float projectileSpeed = 10.0f;
        [SerializeField]
        private float projectileRange= 10.0f;
        [SerializeField]
        private FieldOfView FOV;
        [SerializeField]
        private Animator auraAnimator;
        [SerializeField]
        private int rayCount = 3;
        [SerializeField]
        private float skillDamage = 10.0f;
        [SerializeField]
        private float skillAnimationSpeed = 2f;

        [SerializeField]
        private float skillSpeedCorrection = 0.7f;

        private List<GameObject> hitedObj = new List<GameObject>();

        protected override void Start()
        {
            base.Start();
            auraAnimator.SetFloat("SkillSpeed", skillAnimationSpeed);

        }
        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
            projectileRange = FOV.ViewRadius;
            auraAnimator.SetBool("SkillUse", true);
            
            SetMovePrevSkill();
        }

        private void SetMovePrevSkill()
        {
            owner.Info.SpeedDown(skillSpeedCorrection);
            owner.CanDirectionChange = false;
        }
        private void SetMoveAfterSkill()
        {
            Debug.Log("애니메이션 돌리기");
            auraAnimator.SetBool("SkillUse", false);
            owner.Info.SpeedUp(skillSpeedCorrection);
            owner.CanDirectionChange = true;
        }

        [PunRPC]
        private void CreateSkillProjectile(string prefabName,Vector3 trans,Quaternion rot)
        {
            GameObject skill = PhotonNetwork.Instantiate(prefabName, trans, rot);
        }

        private void MakeSkillProjectile()
        {
            if (!photonView.IsMine)
                return;
            if (PhotonNetwork.InRoom)
            {
                float correctionAngle = FOV.viewAngle / (rayCount - 1);
                Debug.Log(skillDirection);
                Quaternion skillRot = Quaternion.LookRotation(skillDirection,Vector3.up);
                skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y - (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                for (int i = 0; i < rayCount; ++i)
                {
                    photonView.RPC(nameof(CreateSkillProjectile), RpcTarget.MasterClient, skillPrefab.name, transform.position + transform.forward * 0.2f, skillRot);
                    skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y + correctionAngle, skillRot.eulerAngles.z);
                }

                if (!PhotonNetwork.IsMasterClient)
                    return;
                Ray ray = new Ray();
                RaycastHit rayHit;
                ray.origin = FOV.transform.position;

                ray.direction = FOV.transform.forward;
                ray.direction = Quaternion.AngleAxis(-(FOV.viewAngle / 2), Vector3.up) * ray.direction;



                for (int i = 0; i < rayCount; ++i)
                {
                    if (true == Physics.Raycast(ray, out rayHit, FOV.ViewRadius) && rayHit.transform.gameObject != owner && false == hitedObj.Contains(rayHit.transform.gameObject))
                    {
                        hitedObj.Add(rayHit.transform.gameObject);
                        Debug.Log("부딫힘");
                        //rayHit.transform.gameObject.transform.position = owner.transform.position + ((rayHit.transform.gameObject.transform.position - owner.transform.position).normalized * FOV.ViewRadius);
                        // 맞는 애가 넉백이 없을 시에 ( 플레이어가 아닐 때 ) 는 넉백 작동 안함.
                        if(null != rayHit.transform.gameObject.GetComponent<isKnockBack>())
                        {
                            rayHit.transform.gameObject.GetComponent<isKnockBack>().CallMoveKnockBack(owner.transform.position, (rayHit.transform.gameObject.transform.position - owner.transform.position).normalized, projectileSpeed, FOV.ViewRadius);
                            PlayerInfoManager.Instance.CurHpDecrease(gameObject.name, rayHit.transform.gameObject.name, skillDamage);
                        }
                    }
                    ray.direction = Quaternion.AngleAxis(correctionAngle, Vector3.up) * ray.direction;
                }
                hitedObj.Clear();
            }
            else
            {
                float correctionAngle = FOV.viewAngle / (rayCount - 1);
                Debug.Log(skillDirection);
                Quaternion skillRot = Quaternion.LookRotation(skillDirection, Vector3.up);
                skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y - (FOV.viewAngle / 2), skillRot.eulerAngles.z);
                for (int i = 0; i < rayCount; ++i)
                {
                    Instantiate(skillPrefab , transform.position + transform.forward * 0.2f, skillRot);
                    skillRot.eulerAngles = new Vector3(skillRot.eulerAngles.x, skillRot.eulerAngles.y + correctionAngle, skillRot.eulerAngles.z);
                }

                Ray ray = new Ray();
                RaycastHit[] rayHits;
                ray.origin = FOV.transform.position;

                ray.direction = FOV.transform.forward;
                ray.direction = Quaternion.AngleAxis(-(FOV.viewAngle / 2), Vector3.up) * ray.direction;

                

                for (int i = 0; i < rayCount; ++i)
                {
                    rayHits = Physics.RaycastAll(ray, FOV.ViewRadius, LayerMask.NameToLayer("Player"));

                    foreach(RaycastHit rayhit in rayHits)
                    {
                        GameObject hitted = rayhit.transform.gameObject;
                        if (hitted.tag == "Player" && false == hitedObj.Contains(hitted) && hitted != owner)
                        {
                            Debug.Log("부딫힘");
                            hitted.GetComponent<isKnockBack>().CallMoveKnockBack(owner.transform.position, (hitted.transform.position - owner.transform.position).normalized, projectileSpeed, FOV.ViewRadius);
                            hitted.gameObject.GetComponent<PlayerInfo>().Damaged(skillDamage);
                        }
                    }
                    ray.direction = Quaternion.AngleAxis(correctionAngle, Vector3.up) * ray.direction;
                }
                hitedObj.Clear();
            }
        }

        private void OnDrawGizmos()
        {
            float correctionAngle = FOV.viewAngle / (rayCount - 1);


            Ray ray = new Ray();
            RaycastHit[] rayHits;
            ray.origin = FOV.transform.position;

            ray.direction = FOV.transform.forward;
            ray.direction = Quaternion.AngleAxis(-(FOV.viewAngle / 2), Vector3.up) * ray.direction;


            for (int i = 0; i < rayCount; ++i)
            {
                rayHits = Physics.RaycastAll(ray, FOV.ViewRadius, LayerMask.NameToLayer("Player"));
                Gizmos.DrawRay(this.transform.position , ray.direction);
                foreach (RaycastHit rayhit in rayHits)
                {
                    GameObject hitted = rayhit.transform.gameObject;
                    if (hitted.tag == "Player" && hitedObj.Contains(hitted) && hitted != owner)
                    {
                        Debug.Log("부딫힘");
                        hitted.GetComponent<isKnockBack>().CallMoveKnockBack(owner.transform.position, (hitted.transform.position - owner.transform.position).normalized, projectileSpeed, FOV.ViewRadius);
                        hitted.gameObject.GetComponent<PlayerInfo>().Damaged(skillDamage);
                    }
                }
                ray.direction = Quaternion.AngleAxis(correctionAngle, Vector3.up) * ray.direction;
            }
            hitedObj.Clear();
        }

        public override void AutoSkill()
        {
            return;
        }


    }
}
