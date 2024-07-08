using UnityEngine;
using static FunctionLibrary;
using System;

class Graph : MonoBehaviour
{
    const float PI = (float)Math.PI;

    [SerializeField] Transform pointPrefab;
    [SerializeField] int meshIndex = 0;
    [SerializeField]  Mesh[] myMeshes;
    
    [SerializeField]
    funcName FunctionName= funcName.MultiWave;
    [SerializeField]
    uint resolution;
    [SerializeField]
    float speed;

    [SerializeField]
    float xZMin, xZMax;
    [SerializeField]
    float scaleX, scaleY, scaleZ;
    [SerializeField]
    bool modulusOfX, modulusOfZ;


    Transform[] points;

    void Awake()
    {
        //Prefab correction
        pointPrefab.GetComponent<MeshFilter>().mesh = myMeshes[meshIndex];

        //Elements used in loop.
        points = new Transform[resolution*resolution];
        Vector3 Cords = pointPrefab.localPosition;

        //Precalculation for loop iterations.
        float x, z;
        float step;    { float stop = xZMax - xZMin;    step= stop / resolution; }

        //Point spawning loop.
        x = z = xZMin;
        for (uint i = 0; i < resolution; i++)
        {
            x = xZMin;
            var c = i * resolution;
            for (uint j = 0; j < resolution; j++)
            {
                Cords.x = x; Cords.z = z;
                x += step;

                Transform point = points[c+j] = Instantiate(pointPrefab);
                point.SetParent(transform, false);
                point.localPosition = Cords;
            }
            z += step;
        }
    }

    void Update()
    {
        //Hotkeys
        {
            //Toggler
            {
                //Zeroing time speed
                if (Input.GetKeyDown(KeyCode.Alpha0)) speed = 0;

                //Changing mesh
                {
                    byte axis = 0;
                    if (Input.GetKeyDown(KeyCode.LeftBracket)) axis--;
                    if (Input.GetKeyDown(KeyCode.RightBracket)) axis++;

                    if (axis != 0)
                    {
                        meshIndex = (myMeshes.Length + axis + meshIndex) % myMeshes.Length;
                        Mesh newMesh = myMeshes[meshIndex];

                        foreach (Transform P in points)  P.GetComponent<MeshFilter>().mesh = newMesh;
                    }
                    //gameObject.GetComponent<MeshFilter>().mesh = myMesh;
                }



                //Choosing displayed function.
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        FunctionName++;
                        FunctionName = (FunctionLibrary.funcName)((int)FunctionName % FunctionLibrary.Functions.Length);
                    }
                    else
                    {
                        int stop = FunctionLibrary.Functions.Length;
                        const int maxFKeyNum = 15; //because F15 exists
                        if (stop > maxFKeyNum) stop = maxFKeyNum;

                        for (int i = 0; i < stop; i++)
                        {
                            if (Input.GetKeyDown(KeyCode.F1 + i)) FunctionName = (FunctionLibrary.funcName)i;
                        }

                    }
                }
            }

            //Using deltaTime
            {
                float tDelta = Time.deltaTime;

                //Scaling & moduling
                float scalingAxis = 0;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) scalingAxis++;
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) scalingAxis--;
                scalingAxis *= tDelta;

                if (Input.GetKey(KeyCode.X))
                {
                    scaleX += scalingAxis;
                    if (Input.GetKeyDown(KeyCode.Tab)) modulusOfX = !modulusOfX;
                }
                if (Input.GetKey(KeyCode.Y))
                {
                    scaleY += scalingAxis;
                    //:-)if (Input.GetKeyDown(KeyCode.Tab)) modulusOfY = !modulusOfY;
                }
                if (Input.GetKey(KeyCode.Z))
                {
                    scaleZ += scalingAxis;
                    if (Input.GetKeyDown(KeyCode.Tab)) modulusOfZ = !modulusOfZ;
                }

                //Changing speed
                {
                    const float speedChange = 3;
                    if (Input.GetKey(KeyCode.Equals)) speed += tDelta*speedChange;
                    if (Input.GetKey(KeyCode.Minus)) speed -= tDelta*speedChange;
                }
                
            }
        }




        //Plotting
        {
            FuncDeleg f = Functions[(uint)FunctionName];
            float t = Time.time * speed;

            for (uint i = 0; i < resolution * resolution; i++)
            {
                Vector3 Cords = points[i].localPosition;

                //Cords correction
                float x = modulusOfX ? Mathf.Abs(Cords.x) : Cords.x;
                float z = modulusOfZ ? Mathf.Abs(Cords.z) : Cords.z;
                //Function
                Cords.y = scaleY * f(x / scaleX, z / scaleZ, t);

                points[i].localPosition = Cords;
            }
        }
    }
}