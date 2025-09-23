using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TestSpriteAtlas : MonoBehaviour
{
    public SpriteAtlas atlas;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            image.sprite = atlas.GetSprite("4_Play_UI_Matching");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            image.sprite = atlas.GetSprite("8_Pass");
        }
    }
}
