using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory")]
    public List<string> inv = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //adding a gun to inventory
    public void AddToInv(string gunName)
    {
        inv.Add(gunName);
    }
}
