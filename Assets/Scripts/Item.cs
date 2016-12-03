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
}