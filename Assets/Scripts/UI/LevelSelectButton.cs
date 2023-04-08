using UnityEngine;

namespace Detection
{
    public class LevelSelectButton : MonoBehaviour
    {
        public void clickTest()
        {
            Debug.Log(message: "button clicked");
        }

        public void LevelSelected(string sceneName)
        {
            GameManager.instance.UpdateGameState(GameState.LEVELENDED);
            GameManager.instance.SwitchToScene(sceneName, true);
        }
    }
}
