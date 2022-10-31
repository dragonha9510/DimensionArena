using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public int speed = 150;

    private bool canMoveUp = true;
    private bool canMoveDown = true;

    private float maxZoomInDistance = 0.8f;
    private float maxZoomOutDistance = 4;

    // Update is called once per frame
    void Update()
    {
        // always focus on the target
		transform.LookAt(target.transform.position + new Vector3(0.15f,0.2f,0.0f));

        // move around the target
        Vector3 newPos = new Vector3();
        float movement = Time.deltaTime * speed;

        bool upKey = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (upKey && canMoveUp)
        {
            newPos = transform.right;
        }

        bool downKey = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if (downKey && canMoveDown)
        {
            newPos = -transform.right;
        }

        bool leftKey = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);

        if (leftKey)
        {
            newPos = transform.up;
        }

        bool rightKey = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (rightKey)
        {
            newPos = -transform.up;
        }

        transform.RotateAround(target.position, newPos, movement);


        // set limit for vertical rotation
        Vector3 eulerAngles = transform.eulerAngles;
        float xAngle = transform.eulerAngles.x;

        if (xAngle <= 90 && xAngle > 70)
        {
            this.canMoveUp = false;
            this.canMoveDown = true;
        } else if (xAngle >= 350 && xAngle < 360)
        {
            this.canMoveUp = true;
            this.canMoveDown = false;
        } else
        {
            this.canMoveUp = true;
            this.canMoveDown = true;
        }

        transform.eulerAngles = eulerAngles;

        // zoom in and out of the target
        Vector3 zoomPos = new Vector3();

        float scrollDirection = Input.mouseScrollDelta.y;
 
        zoomPos.z = scrollDirection / 10;
        
        float distance = Vector3.Distance(transform.position, target.position);

        if ((distance > maxZoomInDistance && distance < maxZoomOutDistance) ||
            (distance <= maxZoomInDistance && scrollDirection < 0) ||
            (distance >= maxZoomOutDistance && scrollDirection > 0))
        {
            transform.Translate(zoomPos);
        }
    }
}