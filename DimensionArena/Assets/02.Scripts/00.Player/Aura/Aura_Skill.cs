using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;

namespace PlayerSpace
{
    public class Aura_Skill : Player_Skill
    {
        protected override void Start()
        {
            base.Start();
        }
        public override void ActSkill(Vector3 attackdirection, float magnitude)
        {
            //Parabola rotation, distance velocity radianAngle�� ����ȭ���� �ʴ´�. �̸� �������ְ� ������ �Ķ���͸� ���� �� ����ұ�?
            if (!PhotonNetwork.OfflineMode)
            {
            }
            else
            {
            }
        }
    }
}
