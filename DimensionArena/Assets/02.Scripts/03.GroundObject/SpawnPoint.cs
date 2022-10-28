using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    bool isRegistered;
    public void SetRegisterOn() { isRegistered = true; }
    public bool GetRegisterState() { return isRegistered; }
}
