using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{

    private GameObject yarn_object;
    private Rigidbody2D yarn_rigidbody;
    private Camera this_camera;

    public Vector2 origonal_position;
    private float origonal_zoom;

    public float zoom_to_velocity_ratio;
    //	public float zoom_velocity_activation_threshold;
    public float max_zoom_level;
    public float zoom_growth_decay;
    public float zoom_unzoom_steps;

    public Vector2 pan_to_velocity_ratio;
    public Vector2 max_pan_level;
    public Vector2 pan_snap;
    public Vector2 min_activation_vel;

    private Vector2 previous_velocity_zoom;
    private Vector2 previous_velocity_pan;

    public Vector2 cam_trail_velocity;
    private Vector2 cam_location_to_reach;

    // Use this for initialization
    void Start()
    {
        yarn_object = GameObject.FindGameObjectWithTag("YarnPhysics") as GameObject;
        yarn_rigidbody = yarn_object.GetComponent<Rigidbody2D>() as Rigidbody2D;
        this_camera = this.GetComponent<Camera>();
        origonal_position = new Vector2(
            this.gameObject.transform.localPosition.x,
            this.gameObject.transform.localPosition.y);
        origonal_zoom = this_camera.orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 yarn_vel = yarn_rigidbody.velocity;
        //zoom
        if (this_camera.orthographicSize + zoom_unzoom_steps <= origonal_zoom + yarn_vel.magnitude * zoom_to_velocity_ratio)
        {
			this_camera.orthographicSize = origonal_zoom + yarn_vel.magnitude * zoom_to_velocity_ratio;
            if (this_camera.orthographicSize > max_zoom_level)
            {
				this_camera.orthographicSize = max_zoom_level;
            }
        }
        else if (this_camera.orthographicSize - zoom_unzoom_steps > origonal_zoom + yarn_vel.magnitude * zoom_to_velocity_ratio)
        {
			if (this_camera.orthographicSize > origonal_zoom) {
				this_camera.orthographicSize -= zoom_growth_decay;
			}
        }

        //Panx
        if (Mathf.Abs(yarn_vel.x) > min_activation_vel.x)
        {
            float vel_x_pan = origonal_position.x + yarn_vel.x * pan_to_velocity_ratio.x;
            if (Mathf.Abs(vel_x_pan) < Mathf.Abs(max_pan_level.x + origonal_position.x))
            {
                cam_location_to_reach = new Vector3(
                    vel_x_pan,
                    origonal_position.y);
            }
            else
            {
                cam_location_to_reach = new Vector3(
					max_pan_level.x * Mathf.Sign(yarn_vel.x) + origonal_position.x,
                    origonal_position.y);
            }
        }
        else
        {
            cam_location_to_reach = new Vector3(
                origonal_position.x,
                origonal_position.y);
        }
        //Pany
        if (Mathf.Abs(yarn_vel.y) > min_activation_vel.y)
        {
            float vel_y_pan = origonal_position.y + yarn_vel.y * pan_to_velocity_ratio.y;
            if (Mathf.Abs(vel_y_pan) < Mathf.Abs(max_pan_level.y + origonal_position.y))
            {
                cam_location_to_reach = new Vector3(
                    cam_location_to_reach.x,
                    vel_y_pan);
            }
            else
            {
                cam_location_to_reach = new Vector3(
                    cam_location_to_reach.x,
					max_pan_level.y * Mathf.Sign(yarn_vel.y) + origonal_position.y);
            }
        }
        else
        {
            cam_location_to_reach = new Vector3(
                cam_location_to_reach.x,
                origonal_position.y);
        }

        //grow to the pan

        GameObject.FindGameObjectWithTag("CameraDestination").transform.localPosition = cam_location_to_reach;

        float x_dest = transform.localPosition.x + Mathf.Sign(cam_location_to_reach.x - transform.localPosition.x) * cam_trail_velocity.x;
        //		Debug.Log (Mathf.Abs (x_dest - cam_location_to_reach.x));
        if (Mathf.Abs(x_dest - cam_location_to_reach.x) > pan_snap.x)
        {
            transform.localPosition = new Vector2(
                x_dest,
                transform.localPosition.y);
        }
        else
        {
            transform.localPosition = new Vector2(
                cam_location_to_reach.x,
                transform.localPosition.y);
        }

        float y_dest = transform.localPosition.y + Mathf.Sign(cam_location_to_reach.y - transform.localPosition.y) * cam_trail_velocity.y;
        //		Debug.Log (Mathf.Abs (y_dest - cam_location_to_reach.y));
        if (Mathf.Abs(y_dest - cam_location_to_reach.y) > pan_snap.y)
        {
            transform.localPosition = new Vector2(
                transform.localPosition.x,
                y_dest);
        }
        else
        {
            transform.localPosition = new Vector2(
                transform.localPosition.x,
                cam_location_to_reach.y);
        }

    }
}
