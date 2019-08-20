using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class FlexibleExtensions
{
    public static void AddEventTrigger (this EventTrigger eventTrigger, UnityAction<BaseEventData> action,
        EventTriggerType triggerType)
    {
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent ();
        trigger.AddListener (action);

        EventTrigger.Entry entry = new EventTrigger.Entry { callback = trigger, eventID = triggerType };
        eventTrigger.triggers.Add (entry);
    }
}

[RequireComponent (typeof (EventTrigger))]
public class FlexiblePanel : MonoBehaviour
{
    [Tooltip ("Object used to resize panel, must be child of panel and anchored to bottom right, must have event trigger component")]
    [SerializeField] private bool enableResize;
    [SerializeField] private EventTrigger resizeEventTrigger;
    [SerializeField] private Vector2 MinimumDimmensions = new Vector2 (50, 50);
    [SerializeField] private Vector2 MaximumDimmensions = new Vector2 (800, 800);

    private RectTransform targetPanel;
    private EventTrigger dragEventTrigger;

    void Start ()
    {
        targetPanel = GetComponent<RectTransform> ();
        dragEventTrigger = GetComponent<EventTrigger> ();
        dragEventTrigger.AddEventTrigger (OnDragPanel, EventTriggerType.Drag);
        resizeEventTrigger.AddEventTrigger (OnDragResize, EventTriggerType.Drag);
    }

    void OnDragPanel (BaseEventData data)
    {
        PointerEventData ped = (PointerEventData) data;
        targetPanel.transform.Translate (ped.delta);
    }

    void OnDragResize (BaseEventData data)
    {
        PointerEventData ped = (PointerEventData) data;

        if (enableResize)
        {
            targetPanel.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, Mathf.Clamp (targetPanel.rect.width + ped.delta.x, MinimumDimmensions.x, MaximumDimmensions.x));
            targetPanel.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Mathf.Clamp (targetPanel.rect.height - ped.delta.y, MinimumDimmensions.y, MaximumDimmensions.y));
            Debug.Log ("X: " + Mathf.Clamp (targetPanel.rect.width - ped.delta.x, MinimumDimmensions.x, MaximumDimmensions.x));

            /*       targetPanel.SetInsetAndSizeFromParentEdge ((RectTransform.Edge) horizontalEdge,
                      Screen.width - targetPanel.position.x - targetPanel.pivot.x * targetPanel.rect.width,
                      Mathf.Clamp (targetPanel.rect.width - ped.delta.x, MinimumDimmensions.x, MaximumDimmensions.x));

                  targetPanel.SetInsetAndSizeFromParentEdge ((RectTransform.Edge) verticalEdge,
                      Screen.height - targetPanel.position.y - targetPanel.pivot.y * targetPanel.rect.height,
                      Mathf.Clamp (targetPanel.rect.height - ped.delta.y, MinimumDimmensions.y, MaximumDimmensions.y)); */

            /*       targetPanel.SetInsetAndSizeFromParentEdge ((RectTransform.Edge) horizontalEdge,
                      targetPanel.position.x - targetPanel.pivot.x * targetPanel.rect.width,
                      Mathf.Clamp (targetPanel.rect.width + ped.delta.x, MinimumDimmensions.x, MaximumDimmensions.x));

                  targetPanel.SetInsetAndSizeFromParentEdge ((RectTransform.Edge) verticalEdge,
                      targetPanel.position.y - targetPanel.pivot.y * targetPanel.rect.height,
                      Mathf.Clamp (targetPanel.rect.height + ped.delta.y, MinimumDimmensions.y, MaximumDimmensions.y)); */

        }
    }
}