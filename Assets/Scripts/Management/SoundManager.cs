using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum eSoundEffect
{
    [Description("Sounds/gun_fire")]
    gun_fire,
    [Description("Sounds/m4a1_fire")]
    m4a1_fire,
    [Description("Sounds/ak47_fire")]
    ak47_fire,
}

public class SoundManager : SingletonBehaviour<SoundManager>
{
    /// <summary>
    /// 사운드 풀 관리하는 클래스
    /// </summary>
    private class AudioChannel
    {
        private Transform _parent = null;
        private List<AudioSource> _audioSources = new();
        private int poolCount = 20;

        public AudioChannel(Transform parent)
        {
            _parent = parent;
            CreatePool();
        }

        private void CreatePool()
        {
            for (int i = 0; i < poolCount; i++)
            {
                var go = new GameObject("AudioSource");
                go.transform.SetParent(_parent);
                _audioSources.Add(go.AddComponent<AudioSource>());
            }
        }

        public AudioSource GetAudioSource()
        {
            AudioSource audioSource = _audioSources.Find(source => !source.gameObject.activeSelf);
            if (!audioSource)
            {
                audioSource = _audioSources[0];
            }
            _audioSources.Remove(audioSource);
            _audioSources.Add(audioSource);
            return audioSource;
        }
    }

    private AudioChannel _effectChannel;
    private Dictionary<AudioSource, Coroutine> _playCoroutines = new();

    private void Awake()
    {
        _effectChannel = new AudioChannel(this.transform);
    }

    /// <summary>
    /// 사운드 재생
    /// </summary>
    /// <param name="worldPos"></param>
    /// <param name="sound"></param>
    public void PlayEffectSound(Vector3 worldPos, eSoundEffect sound, float volume = 1f)
    {
        AudioSource audioSource = _effectChannel.GetAudioSource();
        if (audioSource)
        {
            audioSource.clip = Resources.Load<AudioClip>(sound.ToDescription());
            audioSource.volume = volume;
            if (_playCoroutines.ContainsKey(audioSource))
            {
                StopCoroutine(_playCoroutines[audioSource]);
                _playCoroutines[audioSource] = null;
            }
            else
            {
                _playCoroutines.Add(audioSource, null);
            }
            _playCoroutines[audioSource] = StartCoroutine(PlayCo(audioSource));
        }
    }

    /// <summary>
    /// 사운드 일정 시간 뒤에 종료되면 게임 오브젝트 비활성화 처리
    /// </summary>
    /// <param name="audioSource"></param>
    /// <returns></returns>
    private IEnumerator PlayCo(AudioSource audioSource)
    {
        audioSource.gameObject.SetActive(true);
        audioSource.Stop();
        audioSource.Play();
        yield return YieldInstructionCache.WaitForSeconds(audioSource.clip.length);
        audioSource.gameObject.SetActive(false);
    }
}