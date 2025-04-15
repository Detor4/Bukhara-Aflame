using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonoTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource monoAudioSource;
    private void OnTriggerEnter(Collider other)
    {
        Mono monoScript = other.GetComponent<Mono>();
        if (monoScript != null)
        {
            MonoManager.Instance.AddMono();
            monoScript.Action();
            monoAudioSource.Play();
        }
    }
}
