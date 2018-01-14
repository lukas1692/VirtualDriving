using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public enum DriveType
{
    RearWheelDrive,
    FrontWheelDrive,
    AllWheelDrive
}

public struct GhostEvent
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;

    public float angle;
    public float torque;
    public float laptime;
};

public class WheelDrive : MonoBehaviour
{
    public Text speedText;

    public List<Image> speedStrip;

    [Tooltip("Maximum steering angle of the wheels")]
	public float maxAngle = 30f;
	[Tooltip("Maximum torque applied to the driving wheels")]
	public float maxTorque = 300f;
	[Tooltip("Maximum brake torque applied to the driving wheels")]
	public float brakeTorque = 30000f;
	[Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
	public GameObject wheelShape;

	[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
	public float criticalSpeed = 5f;
	[Tooltip("Simulation sub-steps when the speed is above critical.")]
	public int stepsBelow = 5;
	[Tooltip("Simulation sub-steps when the speed is below critical.")]
	public int stepsAbove = 1;

	[Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
	public DriveType driveType;

    private WheelCollider[] m_Wheels;

    public static List<GhostEvent> replayCarEventStream = new List<GhostEvent>();
    public static List<GhostEvent> replayGhostEventStream = new List<GhostEvent>();

    Rigidbody rigid;

    // Find all the WheelColliders down in the hierarchy.
    void Start()
	{
        rigid = GetComponent<Rigidbody>();

        m_Wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < m_Wheels.Length; ++i) 
		{
			var wheel = m_Wheels [i];

			// Create wheel shapes only when needed.
			if (wheelShape != null)
			{
				var ws = Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}
	}

	// This is a really simple approach to updating wheels.
	// We simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero.
	// This helps us to figure our which wheels are front ones and which are rear.
	void Update()
	{
		m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

		float angle = maxAngle * Input.GetAxis("Horizontal");
		float torque = maxTorque * Input.GetAxis("Vertical");

		float handBrake = Input.GetKey(KeyCode.X) ? brakeTorque : 0;

		foreach (WheelCollider wheel in m_Wheels)
		{
			// A simple car where front wheels steer while rear ones drive.
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;

			if (wheel.transform.localPosition.z < 0)
			{
				wheel.brakeTorque = handBrake;
			}

			if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
			{
				wheel.motorTorque = torque;
			}

			if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
			{
				wheel.motorTorque = torque;
			}

			// Update visual wheels if any.
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// Assume that the only child of the wheelcollider is the wheel shape.
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}
	}
    private void FixedUpdate()
    {
        Vector3 position = rigid.position;
        Quaternion rotation = rigid.rotation;
        Vector3 velocity = rigid.velocity;
       

        GhostEvent ghost = new GhostEvent();
        ghost.position = rigid.position;
        ghost.rotation = rigid.rotation;
        ghost.laptime = LapTimeController.getCurrentTime();

        //Debug.Log(ghost.position + ", " + ghost.laptime);

        replayCarEventStream.Add(ghost);

        speedText.text = string.Format("{0:0}", rigid.velocity.magnitude * 3.6);
        
        setSpeedStrip(rigid.velocity.magnitude * 3.6);
    }

    public static void startNewGhost()
    {
        replayGhostEventStream = new List<GhostEvent>(replayCarEventStream);
        replayCarEventStream = new List<GhostEvent>();

        GhostCarScript.startGhost();
    }

    public static void clearGhostHistory()
    {
        replayCarEventStream = new List<GhostEvent>();
    }

    public void setSpeedStrip(double speed)
    {
        if (speed < 1)
            return;

        int nr = Convert.ToUInt16(Mathf.Lerp(0, speedStrip.Count, (float)(speed/130)));

        for (int i = 0; i < speedStrip.Count; i++)
        {
            if (i < nr)
                speedStrip[i].GetComponent<Image>().color = new Color(255, 0, 0);
            else
                speedStrip[i].GetComponent<Image>().color = new Color(255, 255, 255);
        }
            
    }
}
