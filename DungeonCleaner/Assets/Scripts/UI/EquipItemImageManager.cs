using UnityEngine;
using UnityEngine.U2D;

public class EquipItemImageManager : MonoBehaviour
{
    public static EquipItemImageManager Instance { get; private set; }

    [SerializeField]
    private SpriteAtlas atlas;

    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetSprite(string name)
    {
        return atlas.GetSprite(name);
    }
}
