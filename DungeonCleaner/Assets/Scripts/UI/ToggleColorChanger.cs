using UnityEngine;
using UnityEngine.UI;

public class ToggleColorChanger : MonoBehaviour
{
    public Image toggleImage;
    public Color changeColor;

    private Color originColor;

    private void Awake()
    {
        originColor = toggleImage.color;
    }

    public void ChangeColor(bool isOn)
    {
        if(isOn)
        {
            toggleImage.color = changeColor;
        }
        else
        {
            toggleImage.color = originColor;
        }
    }
}