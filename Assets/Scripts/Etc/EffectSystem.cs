using UnityEngine;

public class EffectSystem : MonoBehaviour
{
	#region Inspector

	public float duration = 1f;

    #endregion

    private float _time = 0f;

    private void OnEnable()
    {
        _time = 0f;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= duration)
        {
            this.gameObject.SetActive(false);
        }
    }
}