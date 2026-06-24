using UnityEngine;

public enum KitchenItemType
{
    BurgerPattyRaw,
    BurgerPattyCooked,
    BurgerPattyBurnt
}

public class KitchenItem : MonoBehaviour
{
    public string ItemName;
    public KitchenItemType ItemType;
}