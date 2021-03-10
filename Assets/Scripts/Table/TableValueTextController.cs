using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableValueTextController : MonoBehaviour
{
    [SerializeField] private TableController tableController;
    [SerializeField] private Text text;

    [SerializeField]
    private void Start()
    {
        tableController.OnChangeValue += value => UpdateText(value);
    }

    private void UpdateText(int value)
    {
        text.text = value.ToString();
    }
}
