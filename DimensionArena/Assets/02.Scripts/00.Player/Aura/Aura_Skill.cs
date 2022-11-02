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
            //Parabola rotation, distance velocity radianAngle이 동기화되지 않는다. 이를 전달해주고 싶은데 파라미터를 여러 개 써야할까?
            if (!PhotonNetwork.OfflineMode)
            {
            }
            else
            {
            }
        }
    }
}
