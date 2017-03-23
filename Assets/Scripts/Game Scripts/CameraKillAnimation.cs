using UnityEngine;
using System.Collections;

/* This script handles the camera animation on a successful kill.
 * It uses iTween to change the position and rotation of the camera rig,
 * and to change the camera's field of view to zoom in and out.*/
public class CameraKillAnimation : MonoBehaviour
{
	[SerializeField] private float _time = 0.2f;				//how long it takes for the camera to be positioned and zoomed
	[SerializeField] private float _offset = 3.5f;				//to center the action
	[SerializeField] private float _rotationX = -18f;			//to center the action
	[SerializeField] private float _zoomedFieldOfView = 20f;	//desired zoomed in value

	private Transform _killer;		
	private Transform _victim;
	private float _destinationX = 0f;
	private float _destinationY = 0f;
	private float _destinationZ = 0f;
	private Vector3 _destinationPos;	//to set the destination of the camera rig
	private Camera _cam;

	void Awake()
	{
		_cam = GetComponentInChildren<Camera> ();
	}

	//called by the KillManager script on a successful kill.
	public void ZoomIn(Transform killer, Transform victim)
	{
		_killer = killer;
		_victim = victim;
		SetDestination ();

		//Define the iTween events for the animation of the camera.
		//They all ignore TimeScale in order not to be affected by the stop motion effect in the KillFeedback script.
		//CAMERA MOVEMENT
		iTween.MoveTo (gameObject, iTween.Hash ("position", _destinationPos, "time", _time, "easetype", iTween.EaseType.easeInOutCubic, "ignoretimescale", true));
		//CAMERA ROTATION
		iTween.RotateTo (gameObject, iTween.Hash("x", _rotationX, "time", _time, "easetype", iTween.EaseType.easeInOutCubic, "ignoretimescale", true));
		//CAMERA ZOOM IN
		iTween.ValueTo (gameObject, iTween.Hash("from", 60, "to", _zoomedFieldOfView, "time", _time, "onupdate", "FieldOfView", "easetype", iTween.EaseType.easeInOutCubic, "ignoretimescale", true));
	}

	//called by the KillManager script when the feedback is finished.
	public void ZoomOut()
	{
		iTween.MoveTo (gameObject, iTween.Hash ("position", Vector3.zero, "time", _time, "easetype", iTween.EaseType.easeInOutCubic));
		iTween.RotateTo (gameObject, iTween.Hash("x", 0, "time", _time, "easetype", iTween.EaseType.easeInOutCubic));
		iTween.ValueTo (gameObject, iTween.Hash("from", _zoomedFieldOfView, "to", 60, "time", _time, "onupdate", "FieldOfView", "easetype", iTween.EaseType.easeInOutCubic));
	}

	void SetDestination()
	{
		//Reset the positions
		_destinationX = 0f;
		_destinationZ = 0f;

		//Find the average of the x positions of the two players
		_destinationX += _killer.position.x;
		_destinationX += _victim.position.x;
		_destinationX /= 2;

		//Find the average of the z positions of the two players
		_destinationZ += _killer.position.z;
		_destinationZ += _victim.position.z;
		_destinationZ /= 2;
		_destinationZ += _offset;	//add the offset

		_destinationPos.Set (_destinationX, _destinationY, _destinationZ);
	}

	void FieldOfView (float newFoV)
	{
		_cam.fieldOfView = newFoV;
	}
}