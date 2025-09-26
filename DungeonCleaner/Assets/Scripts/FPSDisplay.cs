using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private float fps = 0.0f;
    private float msec = 0.0f;

    private float timer = 0f;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        timer += Time.unscaledDeltaTime;
        if (timer >= 1f)
        {
            msec = deltaTime * 1000.0f;
            fps = 1.0f / deltaTime;

            timer = 0f;
        }
    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = 60;
        style.normal.textColor = Color.red;

        string text = string.Format("{0:0.} fps ({1:0.0} ms)", fps, msec);
        GUI.Label(rect, text, style);
    }
}
