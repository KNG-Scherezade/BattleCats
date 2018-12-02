/*
Origonal Author: ECHibiki

Moves the ball across the terrain

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationNav : StationAbstract {

    public static float torqueOffset = 0.0f;

    private GameObject yarn_object;
	private StationGeneric generic_station;
	private Rigidbody2D yarn_rigidbody;

	public float yarn_mass, linear_drag, angular_drag, gravity_effect;
	public RigidbodyType2D body_type;
	public bool simulated;

	public float accel_torque_magnitude_applied;
	public float brake_torque_magnitude_applied;
	public float accel_force_magnitude_aplied;
	public float brake_force_magnitude_aplied;
	public float max_angular_velocity;
	public Vector2 max_linear_velocity;

    public float dragScale; // for momentum

	void Start(){
		//handled by generic_station
		yarn_object = GameObject.FindGameObjectWithTag("YarnPhysics") as GameObject;
		generic_station = gameObject.AddComponent (typeof(StationGeneric)) as StationGeneric;
		generic_station.startRoutine (this);

		yarn_rigidbody = yarn_object.GetComponent<Rigidbody2D> ();

		yarn_rigidbody.bodyType = body_type;
		yarn_rigidbody.simulated = simulated;
		yarn_rigidbody.mass = yarn_mass;
		yarn_rigidbody.drag = linear_drag;
		yarn_rigidbody.angularDrag = angular_drag;
		yarn_rigidbody.gravityScale = gravity_effect;

	}

	public override bool actionPressed(){
		//handled by generic_station
		return generic_station.actionPressed ();
	}

	public override void fire1Pressed()
	{      
	}

	public override void fire2Pressed()
	{
	}

	public override void fire3Pressed()
	{      
	}

	public override bool checkDistance (Vector2 caller_location, StationAbstract station){
		return generic_station.checkDistance (caller_location, station);
	}

    public override bool IsOccupied(Vector2 caller_location, StationAbstract station)
    {
        return generic_station.IsOccupied(caller_location, station);
    }

    public override void directionPressed(Vector3 dir_vec){
		//make it a torque magn
		float torque = -dir_vec.x;

        // upgrade torque
        torque *= (1 + torqueOffset);
        //momentum: when yarn is going up, apply a "down" drag force

		if (yarn_rigidbody.velocity.y > 0) {
			yarn_rigidbody.AddForce (Vector2.down * dragScale);
		}
		if (Mathf.Abs (dir_vec.x) > 0) {
			if (Mathf.Sign (-yarn_rigidbody.velocity.x) != Mathf.Sign (torque)) {
				yarn_rigidbody.AddTorque (Mathf.Sign(torque) * brake_torque_magnitude_applied);
				yarn_rigidbody.AddForce (new Vector2(-Mathf.Sign(yarn_rigidbody.velocity.x) * brake_force_magnitude_aplied, 0 ));
//				Debug.Log ("Braking");
			} else {
				yarn_rigidbody.AddTorque (Mathf.Sign(torque) * accel_torque_magnitude_applied);
				yarn_rigidbody.AddForce (new Vector2(Mathf.Sign(yarn_rigidbody.velocity.x)* accel_force_magnitude_aplied, 0));
			}
		}
		//Magnitude caps
		if (Mathf.Abs(yarn_rigidbody.angularVelocity) > max_angular_velocity)
			yarn_rigidbody.angularVelocity = Mathf.Sign(yarn_rigidbody.angularVelocity) * max_angular_velocity;
		if (Mathf.Abs(yarn_rigidbody.velocity.x) > max_linear_velocity.x)
			yarn_rigidbody.velocity = new Vector2(Mathf.Sign(yarn_rigidbody.velocity.x) * max_linear_velocity.x, yarn_rigidbody.velocity.y);
		if (Mathf.Abs(yarn_rigidbody.velocity.y) > max_linear_velocity.y)
			yarn_rigidbody.velocity = new Vector2(yarn_rigidbody.velocity.x, Mathf.Sign(yarn_rigidbody.velocity.y) *  max_linear_velocity.y);
//		Debug.Log(yarn_rigidbody.velocity);
//		Debug.Log (max_linear_velocity.x);
	}

	protected override void enterStation(){	
		//handled by generic_station	
	}

	protected override void exitStation(){
		//handled by generic_station
	}

    public void UpgradeTorque(float offset)
    {
        torqueOffset += offset;
    }
}