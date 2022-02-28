using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject definitive;
    [HideInInspector]
    public GameObject passive;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetDefinitive(GameObject definitiveChoosed)
    {
        definitive = definitiveChoosed;
    }
    public void SetPassive(GameObject passiveChoosed)
    {
        passive = passiveChoosed;
    }
}
