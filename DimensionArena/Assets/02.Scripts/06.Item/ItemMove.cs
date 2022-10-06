using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField]
    float boingPower = 1.0f;
    [SerializeField]
    float decreasePower = 0.4f;
    [SerializeField]
    float rotation = 100.0f;
    [SerializeField]
    float decreaseRotation = 0.9f;
    [SerializeField]
    float followSpeed = 1.0f;
    [SerializeField]
    GameObject particle;

    Vector3 randBoing;
    private int ColliderCount = 0;
    private int StopCount = 3;

    private Transform trans;
    public Transform Trans { get {return this.transform; } }
    // Start is called before the first frame update
    void Start()
    {
        trans = this.transform;
        randBoing = new Vector3(Random.Range(-0.2f,0.2f), 1, Random.Range(-0.2f, 0.2f));
        
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(randBoing.normalized * boingPower, ForceMode.Impulse);
    }


    private void FixedUpdate()
    {
        this.transform.Rotate(Vector3.up * rotation * Time.deltaTime,Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.tag == "Ground")
        {
            if(ColliderCount <= StopCount)
            {
                boingPower *= decreasePower;
                rigidBody.AddForce(randBoing.normalized * boingPower, ForceMode.Impulse);
                ++ColliderCount;
                rotation *= decreaseRotation;
            }
            if (ColliderCount == StopCount)
            {
                rigidBody.freezeRotation = true;
                particle.SetActive(true);
            }
        }
    }
}
