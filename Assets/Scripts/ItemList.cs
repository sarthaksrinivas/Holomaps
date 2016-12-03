using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Lists the items that need to be collected in the UI.
/// </summary>
public class ItemList : MonoBehaviour {

    /// <summary> The items that need to be collected. </summary>
    private Item[] items;
    /// <summary> The prefab for item text entries. </summary>
    [SerializeField]
    [Tooltip("The prefab for item text entries.")]
    private GameObject itemText;
    /// <summary> The items in the list UI </summary>
    private GameObject[] itemTextEntries;

    /// <summary> The amount of time that text color takes to change. </summary>
    private const int COLOR_TIME = 20;

    /// <summary> The singleton instance of the object. </summary>
    public static ItemList instance {
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
    /// Initializes the object.
    /// </summary>
    private void Start() {
        items = TaskManager.instance.items;
        itemTextEntries = new GameObject[items.Length];
        for (int i = 0; i < items.Length; i++) {
            items[i].listIndex = i;
            itemTextEntries[i] = Instantiate(itemText, transform);
            Vector3 textPosition = itemTextEntries[i].transform.localPosition;
            textPosition.z = 0;
            textPosition.y -= i * 15;
            itemTextEntries[i].transform.localPosition = textPosition;
            itemTextEntries[i].name = "Item Text";
            itemTextEntries[i].GetComponent<Text>().text = items[i].itemName;
        }
        HighlightItem(0);
    }

    /// <summary>
    /// Checks off an object.
    /// </summary>
    public void CheckItem() {
        int index = TaskManager.instance.currentIndex;
        itemTextEntries[index].transform.FindChild("Check").gameObject.SetActive(true);
        StartCoroutine(ChangeColor(itemTextEntries[index], itemText.GetComponent<Text>().color));
        if (index < itemTextEntries.Length - 1) {
            HighlightItem(index + 1);
        }
    }

    /// <summary>
    /// Highlights an item entry.
    /// </summary>
    /// <param name="index">The index of the item to highlight.</param>
    private void HighlightItem(int index) {
        StartCoroutine(ChangeColor(itemTextEntries[index], Color.yellow));
    }

    /// <summary>
    /// Gradually changes the color of a text entry.
    /// </summary>
    /// <param name="gameObject">The object to change the text color of.</param>
    /// <param name="color">The color to change the object to.</param>
    private IEnumerator ChangeColor(GameObject gameObject, Color color) {
        int timer = 0;
        Text text = gameObject.GetComponent<Text>();
        Color initColor = text.color;
        while (timer++ < COLOR_TIME) {
            text.color = Color.Lerp(initColor, color, (float)timer / COLOR_TIME);
            yield return null;
        }
        text.color = color;
    }
}