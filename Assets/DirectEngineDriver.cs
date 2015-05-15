﻿using UnityEngine;
using System.Collections;

public class DirectEngineDriver : MonoBehaviour {


	public float	maxLifetime = 0.5f,
					maxForce = 50f;

	protected ParticleSystem sys;
	protected string axis;
    protected Rigidbody target;

    public Vector3 offset = Vector3.zero;
    private Vector3 force = Vector3.zero;


	// Use this for initialization
	protected void Start ()
	{
		sys = this.GetComponent<ParticleSystem>();
        target = this.GetComponentInParent<Rigidbody>();
	}


	protected virtual string FetchAxis(Controller controller)
	{
		return controller.ctrlAxisAccelerate;
	}


	private string FetchAxis0()
	{
        Controller ctrl = GetComponentInParent<Controller>();
        if (ctrl != null)
            return FetchAxis(ctrl);
        return null;
	}


	protected virtual float Filter(float f)
	{
		return Mathf.Max(f,0f); //thruster logic
	}

	// Update is called once per frame
	protected void Update()
	{


		//bool enabled = Input.GetKey(key);

		if (axis == null || axis.Length == 0)
			axis = FetchAxis0();

		
		{
			float f = 0f;
			if (axis != null && axis.Length > 0)
			{ 
				//Debug.Log(axis);
				f = Filter(Input.GetAxis(axis));
			}
			if (sys != null)
			{
				sys.enableEmission = f != 0f;
				sys.startLifetime = f * maxLifetime;
			}

			{
				force = new Vector3(0,0,maxForce * f * Time.fixedDeltaTime);
			}
		}
	}


    private Vector3 world, dbgOffset;
    protected void FixedUpdate()
    {
        if (target != null)
        {

            dbgOffset = target.transform.TransformVector(offset);
            world = transform.position + dbgOffset;



            target.AddForceAtPosition(transform.TransformVector(force), world);
        }

    }
}