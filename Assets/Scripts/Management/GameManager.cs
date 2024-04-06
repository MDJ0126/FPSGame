using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    [RuntimeInitializeOnLoadMethod]
    private static void Load()
    {
        if (!IsLive)
        {
            if (Application.isPlaying)
            {
                GameObject go = new GameObject(nameof(GameManager));
                go.AddComponent<GameManager>();
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Options.initialize();
    }
}