using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaskTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MaskedBox>() != null)
        {
            gameObject.tag = "PlayerMask";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<MaskedBox>() != null)
        {
            gameObject.tag = "Player";
        }
    }
}