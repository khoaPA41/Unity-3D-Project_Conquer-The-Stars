using System;
using System.Buffers.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchSwipeController : MonoBehaviour
{
    [Header("Swipe Settings")]
    [Tooltip("Minimum swipe distance, as a percentage of screen height. " + "Percentage instead of raw pixels so behavior stays consistent across Android resolutions/DPI.")]

    [SerializeField, Range(0.03f, 0.3f)]
    private float minSwipeDistancePercent = 0.08f;

    [Tooltip("How much larger |deltaY| must be than |deltaX| for a swipe to count as vertical.")]
    [SerializeField, Range(1f, 3f)]
    private float verticalDominanceRatio = 1.2f;


    private readonly Dictionary<int, Vector2> activeTouchStarts = new();

    private readonly HashSet<int> firedTouchs = new();


    public event Action DodgeAction = delegate { };
    public event Action ParryAction = delegate { };

    public event Action AttackAction = delegate { };

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += HandleFingerDown;
        Touch.onFingerMove += HandleFingerMove;
        Touch.onFingerUp += HandleFingerUp;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= HandleFingerDown;
        Touch.onFingerMove -= HandleFingerMove;
        Touch.onFingerUp -= HandleFingerUp;
        EnhancedTouchSupport.Disable();

        activeTouchStarts.Clear();
        firedTouchs.Clear();
    }

    private void HandleFingerDown(Finger finger)
    {
        activeTouchStarts[finger.index] = finger.screenPosition; // Get start pos of finger
        AttackAction?.Invoke();
    }
    private void HandleFingerMove(Finger finger)
    {
        if (firedTouchs.Contains(finger.index)) return; // if already active
        if (!activeTouchStarts.TryGetValue(finger.index, out Vector2 startPos)) return; // if don't have value

        var delta = finger.screenPosition - startPos;
        var minDistance = Screen.height * minSwipeDistancePercent;
        if (delta.magnitude < minDistance) return; // Not far enough
        if (Mathf.Abs(delta.y) < Mathf.Abs(delta.x) * verticalDominanceRatio) return; // Not vertical enough

        // Qualified -> Active
        firedTouchs.Add(finger.index);

        bool isRightSide = startPos.x > Screen.height * .5f;
        if (isRightSide)
        {
            Debug.Log("Dodge");
            DodgeAction?.Invoke();
        }
        else
        {
            Debug.Log("Parry");
            ParryAction?.Invoke();
        }
    }
    private void HandleFingerUp(Finger finger)
    {
        activeTouchStarts.Remove(finger.index);
        firedTouchs.Remove(finger.index);
    }
}
