using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Item Info")]
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemSprite;

    [TextArea(3, 10)]
    [SerializeField] private string itemDescription;

    // Public Properties
    public string ItemName => itemName;
    public Sprite ItemSprite => itemSprite;
    public string ItemDescription => itemDescription;
}
