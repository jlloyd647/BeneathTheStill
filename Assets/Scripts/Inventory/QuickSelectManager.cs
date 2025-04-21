using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickSelectManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image[] itemSlots; // 0 = Top, 1 = Middle, 2 = Bottom
    [SerializeField] private TextMeshProUGUI[] itemLabels;
    [SerializeField] private GameObject quickSelectUI;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Timing")]
    [SerializeField] private float showDelay = 0.5f;
    [SerializeField] private float fadeDuration = 0.3f;

    private List<InventoryItem> itemOptions = new();
    private int currentIndex = 0;
    private string stationID;

    public delegate void ItemSelectedCallback(InventoryItem selectedItem);
    private ItemSelectedCallback onItemSelected;

    public void Show(List<InventoryItem> items, string stationId, InventoryItem lastUsed = null, ItemSelectedCallback callback = null)
    {
        itemOptions = items;
        this.stationID = stationId;
        this.onItemSelected = callback;

        if (lastUsed != null && itemOptions.Contains(lastUsed))
            currentIndex = itemOptions.IndexOf(lastUsed);
        else
            currentIndex = 0;

        quickSelectUI.SetActive(true);
        UpdateDisplay();

        StopAllCoroutines();
        StartCoroutine(FadeInAfterDelay());
    }

    public void Hide()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0f;
        quickSelectUI.SetActive(false);
        onItemSelected = null;
    }

    private IEnumerator FadeInAfterDelay()
    {
        canvasGroup.alpha = 0f;

        yield return new WaitForSeconds(showDelay);

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(time / fadeDuration);
            yield return null;
        }
    }

    private void Update()
    {
        if (!quickSelectUI.activeSelf) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
            Cycle(-1);
        else if (scroll < 0f)
            Cycle(1);

        // TODO: This needs to be moved out of this section
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     onItemSelected?.Invoke(GetCurrentItem());
        //     Hide();
        // }
    }

    private void Cycle(int direction)
    {
        currentIndex = (currentIndex + direction + itemOptions.Count) % itemOptions.Count;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (itemOptions.Count == 0) return;

        int top = (currentIndex - 1 + itemOptions.Count) % itemOptions.Count;
        int bottom = (currentIndex + 1) % itemOptions.Count;

        itemLabels[0].text = itemOptions[top].itemName;
        itemLabels[1].text = itemOptions[currentIndex].itemName;
        itemLabels[2].text = itemOptions[bottom].itemName;

        itemSlots[0].sprite = itemOptions[top].icon;
        itemSlots[1].sprite = itemOptions[currentIndex].icon;
        itemSlots[2].sprite = itemOptions[bottom].icon;

        itemSlots[0].transform.localScale = Vector3.one;
        itemSlots[1].transform.localScale = Vector3.one * 1.3f;
        itemSlots[2].transform.localScale = Vector3.one;
    }

    public InventoryItem GetCurrentItem() => itemOptions[currentIndex];
}
