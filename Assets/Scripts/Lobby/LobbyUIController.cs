using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    private void Update()
    {
        bool isPress = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        isPress = Input.GetMouseButtonDown(0);
#else
        Touch touch = Input.GetTouch(0);
        isPress = touch.phase == TouchPhase.Ended;
#endif
        if (isPress)
        {
            SceneController.Instance.LoadScene(eScene.GameRoom);
        }
    }
}