using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Keeps track of the items that the user needs to find.
/// </summary>
public class TaskManager : MonoBehaviour {
    
    /// <summary> The items that the user needs to collect. </summary>
    [Tooltip("The items that the user needs to collect.")]
    public Item[] items;
    /// <summary> The item currently being tracked. </summary>
    [HideInInspector]
    public Item currentItem;
    /// <summary> The index of the item currently being tracked. </summary>
    [HideInInspector]
    public int currentIndex = 0;
    /// <summary> The waypoint closest to the current item. </summary>
    private Waypoint targetWaypoint;
    /// <summary> The waypoint closest to the user. </summary>
    private Waypoint currentWaypoint;
    /// <summary> The current path being taken. </summary>
    private List<Waypoint> currentPath;
    /// <summary> The path navigator. </summary>
    private Navigator navigator;

    /// <summary> The material of non-goal waypoints. </summary>
    [SerializeField]
    [Tooltip("The material of non-goal waypoints.")]
    private Material waypointMaterial;
    /// <summary> The material of goal waypoints. </summary>
    [SerializeField]
    [Tooltip("The material of goal waypoints.")]
    private Material goalMaterial;

    /// <summary> The user of the application. </summary>
    private GameObject user;
    /// <summary> The line between the user and the next waypoint. </summary>
    private LineRenderer userLine;



    /// <summary> The singleton instance of the object. </summary>
    public static TaskManager instance {
        get;
        private set;
    }

    /// <summary>
    /// Initializes the singleton instance of the object.
    /// </summary>
    private void Awake() {
        instance = this;
    }

    /// <summary>
    /// Finds the navigator.
    /// </summary>
    private void Start() {
        navigator = GetComponent<Navigator>();
        user = FindObjectOfType<Camera>().gameObject;
        if (items != null && items.Length > 0) {
            currentItem = items[0];
            userLine = Instantiate(navigator.lineRenderer) as LineRenderer;
            userLine.name = "User Line";
        }
    }

    /// <summary>
    /// Constantly draws a line between the user and the next waypoint.
    /// </summary>
    private void Update() {
        if (userLine != null) {
            if (items.Length > currentIndex) {
                if (currentPath == null) {
                    FindTargetWaypoint();
                    currentPath = navigator.DrawShortestPath(navigator.GetClosestWaypoint(user.transform.position), targetWaypoint);
                }
                userLine.SetPosition(1, user.transform.position + Vector3.down * 0.1f);
                userLine.SetPosition(0, currentPath[0].transform.position);
            } else {
                userLine.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Checks if the scanned item matches the item being looked for.
    /// </summary>
    /// <returns>Whether the scanned item matches the item being looked for.</returns>
    /// <param name="skuID">The ID of the item to check.</param>
    public void CheckItem(long skuID) {
        if (currentItem != null && currentItem.skuID == skuID) {
            ItemList.instance.CheckItem();
            if (!IncrementCurrentItem()) {

            }
        } else {
            
        }
    }

    /// <summary>
    /// Changes the current item to the next item in the list.
    /// </summary>
    /// <returns>Whether there are any more items to look for.</returns>
    private bool IncrementCurrentItem() {
        currentItem.collected = true;
        if (items.Length > currentIndex) {
            if (items.Length > ++currentIndex) {
                currentItem = items[currentIndex];
            } else {
                currentItem = null;
            }
            currentPath = null;
            FindTargetWaypoint();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Draws a path between the player and the current item.
    /// </summary>
    /// <param name="currentPosition">The waypoint closest to the player.</param>
    public void DrawPathToItem(Waypoint currentPosition) {
        if (targetWaypoint == null) {
            FindTargetWaypoint();
        }
        currentPath = navigator.DrawShortestPath(currentPosition, targetWaypoint);
    }

    /// <summary>
    /// Finds the waypoint closest to the target item.
    /// </summary>
    private void FindTargetWaypoint() {
        if (targetWaypoint != null) {
            targetWaypoint.GetComponent<Renderer>().material = waypointMaterial;
        }
        if (currentItem != null) {
            targetWaypoint = navigator.GetClosestWaypoint(currentItem.transform.position);
            targetWaypoint.GetComponent<Renderer>().material = goalMaterial;
        }
    }

    /// <summary>
    /// Changes to the next nearest waypoint.
    /// </summary>
    /// <param name="waypoint">The waypoint that the user left.</param>
    public void LeaveWaypoint(Waypoint waypoint) {
        navigator.RemoveFirstLine();
        if (currentPath != null && currentPath.Count > 1) {
            currentPath.RemoveAt(0);
        }
    }
}