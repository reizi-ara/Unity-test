using UnityEngine;
using UnityEngine.UI;

public class SizeDropdown : MonoBehaviour
{

    [SerializeField]
    UniLightsMain main;

    Dropdown dropDown;

    void Start()
    {
        dropDown = GetComponent<Dropdown>();

        OnValueChanged();
    }

    public void OnValueChanged()
    {
        main.ClearLights();
        main.CreateLights(dropDown.value + 4);
    }
}