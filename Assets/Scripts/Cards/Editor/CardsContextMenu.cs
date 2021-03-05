using UnityEditor;

namespace Cards.Editor
{
    public abstract class CardsContextMenu : ContextMenu
    {
        private const string Directory = BaseDirectory + "Cards/";

        [MenuItem(Directory + "Card")]
        private static void NewCardType() => CreateScriptableObject<CardType>();
    }
}