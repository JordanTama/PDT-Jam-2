using UnityEditor;

public abstract class CardsContextMenu : ContextMenu
{
    private const string Directory = "Cards/";

    [MenuItem(BaseDirectory + Directory + "Card")]
    private static void CreateCard() => CreateScriptableObject<Card>();
}
