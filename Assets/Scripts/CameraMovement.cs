using UnityEngine;
using static UnityEngine.Mathf;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float movementSensitiveness, rotationSensitiveness;

    System.Collections.IEnumerator delayedRotationCorrection()
    {
        yield return new WaitForSeconds(1);
        transform.LookAt(target);
    }

    private void Start()
    {
        this.StartCoroutine(delayedRotationCorrection());
    }


    // Update is called once per frame
    void Update()
    {
        //transformComponent.LookAt(target);
        float t= Time.deltaTime;

        //Rotation
        Vector3 rot;
        {
            if (Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.Keypad5))
            {
                transform.LookAt(target);
                rot = transform.localEulerAngles;
            }
            else
            {
                float baseRotation = rotationSensitiveness * t;

                rot = transform.localEulerAngles;
                rot.y += Input.GetAxis("Y Rotation") * baseRotation;
                rot.x -= Input.GetAxis("X Rotation") * baseRotation;
                rot.z -= Input.GetAxis("Z Rotation") * baseRotation;

                transform.localEulerAngles = rot;
            }
        }

        //Location
        {
            float baseMove = movementSensitiveness * t;
            Vector3 loc = transform.localPosition;

            //x,z  move
            {
                float rotYInRad = Deg2Rad * rot.y;
                float Vx = Sin(rotYInRad)*baseMove, Vz= Cos(rotYInRad)*baseMove;

                //Zooming effect
                //..to be coded

                //keys w,s
                {
                    if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
                    {
                        loc.x -= Vx;
                        loc.z -= Vz;
                    }
                    else if (!Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W))
                    {
                        loc.x += Vx;
                        loc.z += Vz;
                    }
                }

                //keys a,d
                {
                    if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) //Counter clockwise
                    {
                        loc.x -= Vz;
                        loc.z += Vx;
                    }
                    else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) //Clockwise
                    {
                        loc.x += Vz;
                        loc.z -= Vx;
                    }
                }
            }

            //y move;  keys lAlt,space;
            {
                if (Input.GetKey(KeyCode.LeftAlt)) loc.y -= baseMove;
                if (Input.GetKey(KeyCode.Space)) loc.y += baseMove;
            }

            transform.localPosition = loc;
        }
        
    }
}
