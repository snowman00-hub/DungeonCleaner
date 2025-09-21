using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHomeManager : MonoBehaviour
{
    public int selectStageIndex = 1;

    public void StartStage()
    {
        SceneManager.LoadScene(selectStageIndex);
    }
}
