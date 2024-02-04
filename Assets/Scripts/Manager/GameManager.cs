using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    private bool _isMouseLock = true;

    private void Update()
    {
#if UNITY_STANDALONE
        //UpdateMouseLock();
#endif
    }

    private void UpdateMouseLock()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            _isMouseLock = !_isMouseLock;
        }

        if (_isMouseLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}