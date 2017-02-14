using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// When activated the gameObject will move towards a destination. The gameObject will stop moving once the destination is reached or it is deactivated.
/// For use on single destination interactables.
/// </summary>
public class SmartItweenObject : TriggerObject
{

    //Public Variables
    //--------------------------------------------------------------------
    public EAnimationType AnimationType = EAnimationType.PlayOnce;
    public EActionType ActionToPerform = EActionType.Movement;
    public bool ActiveFromStart = false;

    [Range(0, 50)]
    public float Speed = 3;
    public Vector3 DestinationOffset;
    public EaseType ToDestinationEaseType = EaseType.linear;

    [HideInInspector]
    public bool ResetToOriginalPos = false;
    [HideInInspector]
    public float ResetSpeed = 3;
    [HideInInspector]
    public Vector3 OriginalPosOffset;
    [HideInInspector]
    public EaseType ToOriginEaseType = EaseType.linear;
    [HideInInspector]
    public bool CallEventOnFinish = false;
    [HideInInspector]
    public UnityEvent OnPlayOnceFinished;


    public enum EaseType
    {
        easeInQuad,
        easeOutQuad,
        easeInOutQuad,
        easeInCubic,
        easeOutCubic,
        easeInOutCubic,
        easeInQuart,
        easeOutQuart,
        easeInOutQuart,
        easeInQuint,
        easeOutQuint,
        easeInOutQuint,
        easeInSine,
        easeOutSine,
        easeInOutSine,
        easeInExpo,
        easeOutExpo,
        easeInOutExpo,
        easeInCirc,
        easeOutCirc,
        easeInOutCirc,
        linear,
        spring,
        easeInBounce,
        easeOutBounce,
        easeInOutBounce,
        easeInBack,
        easeOutBack,
        easeInOutBack,
        easeInElastic,
        easeOutElastic,
        easeInOutElastic,
    }
    public enum EActionType
    {
        Movement,
        Rotation
    }
    public enum EAnimationType
    {
        PlayOnce,
        PingPong
    }
    //--------------------------------------------------------------------

    //Private Variables
    //--------------------------------------------------------------------              
    private float _elapsedTime;
    private Vector3 _originalPos;
    private Vector3 _destination;
    private Vector3 _currentDestination;
    //--------------------------------------------------------------------


    //EventCalls
    //--------------------------------------------------------------------
    void Start()
    {
        switch (ActionToPerform)
        {
            case EActionType.Movement:
                _originalPos = transform.position + OriginalPosOffset;
                _destination = transform.position + DestinationOffset;
                break;

            case EActionType.Rotation:
                _originalPos = transform.rotation.eulerAngles;
                _destination = (transform.rotation * Quaternion.Euler(DestinationOffset)).eulerAngles;
                break;

        }

        if (ActiveFromStart)
        {            
            Activate();
        }
    }
    //--------------------------------------------------------------------

    //Private Functions
    //--------------------------------------------------------------------
    private void MoveToDestination(Vector3 destination, float speed, string easeType)
    {
        _currentDestination = destination;
        switch (ActionToPerform)
        {
            case EActionType.Movement:
                if (AnimationType == EAnimationType.PlayOnce)
                {
                    iTween.MoveTo(gameObject, new Hashtable { { "position", destination }, { "speed", speed }, { "easeType", easeType }, { "onComplete", "PlayOnceFinished" } });
                }
                else
                    iTween.MoveTo(gameObject, new Hashtable { { "position", destination }, { "speed", speed }, { "easeType", easeType }, { "onComplete", "SwitchDestination" } });
                break;

            case EActionType.Rotation:
                if (AnimationType == EAnimationType.PlayOnce)
                {
                    iTween.RotateTo(gameObject, new Hashtable { { "rotation", destination }, { "speed", speed * 10 }, { "easeType", easeType } });
                    if (OnPlayOnceFinished != null)
                    {
                        OnPlayOnceFinished.Invoke();
                    }
                }
                else
                    iTween.RotateTo(gameObject, new Hashtable { { "rotation", destination }, { "speed", speed * 10 }, { "easeType", easeType }, { "onComplete", "SwitchDestination" } });
                break;

        }

    }

    private void SwitchDestination()
    {
        Debug.Log("Switch Destination");
        MoveToDestination(_currentDestination == _destination ? _originalPos : _destination, Speed,
            ToDestinationEaseType.ToString());
    }

    private void PlayOnceFinished()
    {
        if (OnPlayOnceFinished != null)
        {
            OnPlayOnceFinished.Invoke();
        }
    }
    //--------------------------------------------------------------------

    //Public Functions
    //--------------------------------------------------------------------
    public override void Activate()
    {
        base.Activate();        
        Vector3 destinationBeforeDeactivation = _currentDestination == Vector3.zero ? _destination : _currentDestination;

        if (transform.position != _destination)
        {
            MoveToDestination(AnimationType == EAnimationType.PingPong ? destinationBeforeDeactivation : _destination,
                Speed, ToDestinationEaseType.ToString());
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        iTween.Stop(gameObject);
        if (ResetToOriginalPos && AnimationType != EAnimationType.PingPong)
        {
            MoveToDestination(_originalPos, ResetSpeed, ToOriginEaseType.ToString());
        }
    }

    //--------------------------------------------------------------------

    //Gizmos and Editor 
    //--------------------------------------------------------------------

    //Show the destination using gizmos
    void OnDrawGizmosSelected()
    {
        switch (ActionToPerform)
        {
            case EActionType.Movement:
                Vector3 currentDestination = _destination == Vector3.zero ? transform.position + DestinationOffset : _destination;
                drawMoveToGizmos(currentDestination, Color.green);

                if (ResetToOriginalPos)
                {
                    Vector3 returnDestination = _originalPos == Vector3.zero ? transform.position + OriginalPosOffset : _originalPos;
                    drawMoveToGizmos(returnDestination, Color.red);
                }
                break;

            case EActionType.Rotation:
                Vector3 currentRotation = _destination == Vector3.zero ? (transform.rotation * Quaternion.Euler(DestinationOffset)).eulerAngles : _destination;
                drawRotateToGizmos(currentRotation, Color.green);

                if (ResetToOriginalPos)
                {
                    Vector3 resetRotation = _originalPos == Vector3.zero ? (transform.rotation * Quaternion.Euler(OriginalPosOffset)).eulerAngles : _originalPos;
                    drawRotateToGizmos(resetRotation, Color.red);
                }
                break;

        }
    }

    private void drawMoveToGizmos(Vector3 destination, Color color)
    {
        if (GetComponent<MeshFilter>())
        {
            Gizmos.color = color;
            Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, -1, destination, transform.rotation, transform.lossyScale);
            Gizmos.DrawLine(transform.position, destination);
        }
    }

    private void drawRotateToGizmos(Vector3 rotationTarget, Color color)
    {
        if (GetComponent<MeshFilter>())
        {
            Gizmos.color = color;
            Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, -1, transform.position, Quaternion.Euler(rotationTarget), transform.lossyScale);
        }
    }

    //--------------------------------------------------------------------
}