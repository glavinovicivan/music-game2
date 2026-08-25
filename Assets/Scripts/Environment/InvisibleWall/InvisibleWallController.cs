using System.Collections;
using UnityEngine;

public class InvisibleWallController : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_staticAudioSource;
    [SerializeField]
    private AudioClip m_collisionAudioClip;
    [SerializeField]
    private AudioClip m_loopingAudioClip;
    [SerializeField]
    private float m_staticVolume = 0.6f;
    [SerializeField]
    private float m_fadeOutTime = 1.0f;

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.TryGetComponent(out PlayerController _playerController))
        {
            StopAllCoroutines();

            m_staticAudioSource.volume = m_staticVolume;
            m_staticAudioSource.clip = m_collisionAudioClip;
            m_staticAudioSource.loop = false;
            m_staticAudioSource.Play();
            
            StartCoroutine(PlayLoopingPartWithDelay(m_collisionAudioClip.length));
        }
    }

    private void OnCollisionStay(Collision _collision)
    {
        if (_collision.gameObject.TryGetComponent(out PlayerController _playerController))
        {
            m_staticAudioSource.transform.position = _collision.GetContact(0).point;
        }
    }

    private void OnCollisionExit(Collision _collision)
    {
        if (_collision.gameObject.TryGetComponent(out PlayerController _playerController))
        {
            StartCoroutine(FadeOut(m_fadeOutTime));
        }
    }

    private IEnumerator PlayLoopingPartWithDelay(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        m_staticAudioSource.loop = true;
        m_staticAudioSource.clip = m_loopingAudioClip;
        m_staticAudioSource.Play();
    }

    private IEnumerator FadeOut(float _fadeOutTime)
    {
        float timeFromBeginning = 0;
        while (timeFromBeginning < _fadeOutTime)
        {
            yield return null;
            m_staticAudioSource.volume = Mathf.Lerp(m_staticVolume, 0, timeFromBeginning / _fadeOutTime);
            timeFromBeginning += Time.deltaTime;
        }
    }
}
