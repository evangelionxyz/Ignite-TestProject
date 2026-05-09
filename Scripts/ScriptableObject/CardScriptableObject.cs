using Ignite;

namespace TestProject;

[CreateAssetMenu(FileName = "CardScriptableObject", MenuName = "Card/Config")]
public class CardScriptableObject : ScriptableObject
{
    public enum CardType
    {
        Default = 0,
        Type_A,
        Type_B,
    }

    [SerializeField] public string name;
    [SerializeField] public int id;
    // [SerializeField] public AssetHandle mesh;
    [SerializeField] public CardType type;
}
