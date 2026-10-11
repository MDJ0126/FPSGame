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
                Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Options.initialize();
    }
}