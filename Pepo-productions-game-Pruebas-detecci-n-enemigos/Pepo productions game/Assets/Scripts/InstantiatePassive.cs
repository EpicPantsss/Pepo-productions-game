using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePassive : MonoBehaviour
{
    private GameObject passive;
    void Start()
    {
        passive = GameObject.Find("GameManager").GetComponent<GameManager>().passive;
        if (passive.name == "Velocista")
        {
            Instantiate(passive, transform);
        }
    }
}
