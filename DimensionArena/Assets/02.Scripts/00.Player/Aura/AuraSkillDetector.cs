using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSkillDetector : MonoBehaviour
{
    [SerializeField]
    private Vector3 auraFront;
    [SerializeField]
    private SphereCollider detectorCollider;
    [SerializeField]
    private FieldOfView FOV;
    
    [SerializeField]
    private List<GameObject> willTakeDamageObj = new List<GameObject>();
    public List<GameObject> WillTakeDamageObj { get { return willTakeDamageObj; } }

    private void Start()
    {
        detectorCollider.radius = FOV.ViewRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ParentGround" || other.tag == "ParentObstacle" || other.tag == "Player_Detection" || other.tag == "Player_Body")
            return;
        willTakeDamageObj.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ParentGround" || other.tag == "ParentObstacle" || other.tag == "Player_Detection" || other.tag == "Player_Body")
            return;
        willTakeDamageObj.Remove(other.gameObject);
    }
}
