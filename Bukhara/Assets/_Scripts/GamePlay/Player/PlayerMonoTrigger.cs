using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonoTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Mono monoScript = other.GetComponent<Mono>();
        if (monoScript != null)
        {
            MonoManager.Instance.AddMono();
            monoScript.Action();
        }
    }
}
