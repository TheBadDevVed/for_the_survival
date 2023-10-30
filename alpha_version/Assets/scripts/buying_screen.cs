using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class buying_screen : MonoBehaviour
{
    static_data script;
    public TextMeshProUGUI status;
    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();
    }

    // Update is called once per frame
    void Update()
    {
        status.text = "money: " + script.money.ToString() + "\n\ncotton: " + script.cotton.ToString() + "\n\ncoal: " + script.coal.ToString() + "\n\nmelons: " + script.melon.ToString() + "\n\nkfc: " + script.kfc.ToString();
    }
}
