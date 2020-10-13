using UnityEngine;

public class LightButton : MonoBehaviour
{

    [System.NonSerialized]
    public int row;
    [System.NonSerialized]
    public int col;

    UniLightsMain main;

    void Start()
    {
        main = GameObject.FindGameObjectWithTag("GameController").GetComponent<UniLightsMain>();
    }

    public void OnClick()
    {
        main.SwitchLights(row, col, false);
    }

}