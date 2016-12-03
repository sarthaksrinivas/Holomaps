using UnityEngine;

/// <summary>
/// An item that the user is looking for.
/// </summary>
public class Item : MonoBehaviour {

    /// <summary> The human-readable name of the item. </summary>
    public string itemName;

    /// <summary> Whether the item has already been collected by the user. </summary>
    public bool collected;

    /// <summary> The ID of the item. </summary>
    public long skuID;

    /// <summary> The index of the item in the items list. </summary>
    public int listIndex;

    /// <summary>
    /// Marks when the item is collected.
    /// </summary>
    /// <param name="collider">The user.</param>
    private void OnTriggerEnter(Collider collider) {
        TaskManager.instance.CheckItem(skuID);
    }
}