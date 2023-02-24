using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public void clickTest()
    {
        Debug.Log(message: "button clicked");
    }

    public void LevelSelected(string sceneName)
    {
        GameManager.instance.SwitchToScene(sceneName);
        GameManager.instance.UpdateGameState(GameState.LEVELSTARTING);
    }

}
