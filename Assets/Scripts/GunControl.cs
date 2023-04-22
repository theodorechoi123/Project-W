using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public Guns gun;
    public InventorySystem inventory;
    public float pickupRange;
    public Transform player;


    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if(distanceToPlayer.magnitude <= pickupRange && Input.GetKeyDown(KeyCode.F))
        {
            inventory.AddToInv(gun.name);
            Destroy(this.gameObject);
        }
    }
}
