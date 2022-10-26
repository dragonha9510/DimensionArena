using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    [System.Serializable]
    public class PlayerAtkInfo
    {
    
        public PlayerAtkInfo(float range, int maxMagazine, float reloadTime)
        {
            this.range = range;
            this.maxMagazine = maxMagazine;
            this.reloadTime = reloadTime;

            //Inverse
            shotCost = (float)(1.0f / maxMagazine);
            inverseReloadTime = 1.0f / (reloadTime * maxMagazine);
        }


        [Header("PlayerAttackInfo")]
        [SerializeField] private float range;
        [SerializeField] private int maxMagazine;
        [SerializeField] private float reloadTime;

        private float curCost;
        private float shotCost;
        private float inverseReloadTime;

        public int MaxMagazine => maxMagazine;
        public float Range => range;
        public float ReloadTime => reloadTime;
        public float CurCost => curCost;
        public float ShotCost => shotCost;
        public float InverseReloadTime => inverseReloadTime;


        public void AddCost(float amount)
        {
            curCost += amount;
            curCost = Mathf.Min(curCost, 1);
        }

        public void SubCost(float amount)
        {
            curCost -= amount;
            curCost = Mathf.Max(curCost, 0);
        }
    }
}