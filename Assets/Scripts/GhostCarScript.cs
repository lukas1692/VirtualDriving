using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostCarScript : MonoBehaviour {

    private static int ghostStreamIndex = 0;
    private static int ghostRound = 0;

    private static Lap ghost_lap;
    private static bool ghost_enabled = false;

    [SerializeField]
    Text path;

    private string result = "";

    [SerializeField]
    public GameObject wheelShape;

    private WheelCollider[] m_Wheels;

    IEnumerator Example()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "MyFile.txt");

        path.text = "Out1: " + filePath + "\n" + result;

        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            result = www.text;
        }
        else
            result = System.IO.File.ReadAllText(filePath);

        path.text = "Out2: "+ filePath + "\n"+ result;
    }

    // Use this for initialization
    void Start () {

        //m_Wheels = GetComponentsInChildren<WheelCollider>();
        m_Wheels = transform.parent.GetComponentsInChildren<WheelCollider>();
        Debug.Log("wheels = "+m_Wheels.Length);
        for (int i = 0; i < m_Wheels.Length; ++i)
        {
            var wheel = m_Wheels[i];

            // Create wheel shapes only when needed.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (WheelCollider wheel in m_Wheels)
        {
            if (wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                // Assume that the only child of the wheelcollider is the wheel shape.
                Transform shapeTransform = wheel.transform.GetChild(0);
                shapeTransform.position = p;
                shapeTransform.rotation = q;
            }
        }
    }
    private void FixedUpdate()
    {
        //if (WheelDrive.replayGhostEventStream.Count <= ghostStreamIndex)
        //    return;

        //if (ghostRound > 0)
        //{
        //    GetComponent<MeshRenderer>().enabled = true;
        //}
        //else
        //{
        //    GetComponent<MeshRenderer>().enabled = false;
        //    return;
        //}


        //GhostEvent current = WheelDrive.replayGhostEventStream[ghostStreamIndex];

        //transform.position = current.position;
        //transform.rotation = current.rotation;

        if (ghost_lap == null)
            return;

        if (ghost_lap.timestep.Count <= ghostStreamIndex)
            ghost_enabled = false;    

        if (ghost_enabled)
        {
            //GetComponentInChildren<MeshRenderer>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            return;
        }

        TimeStep current = ghost_lap.timestep[ghostStreamIndex];

        transform.parent.position = current.getPosition();
        transform.parent.rotation = current.getRotation();

        ghostStreamIndex++;

    }

    public static void startGhost(Lap ghost)
    {
        ghost_lap = ghost;
        ghostRound++;
        ghostStreamIndex = 0;
        ghost_enabled = true;
    }
}
