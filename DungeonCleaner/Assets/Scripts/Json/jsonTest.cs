using UnityEngine;

public class jsonTest : MonoBehaviour
{
    private void Update()
    {
        // unity remote할땐 키입력 안받음
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("asdf");
            SaveLoadManager.Save();            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveLoadManager.Load();
        }
    }
}
