using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideSprites : MonoBehaviour
{
    void Start()
    {
        try//used for hiding any sprites during gameplay
        {
            
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        catch (System.Exception ex)
        {
           
            Debug.LogWarning("Error: " + ex.Message);
            
        }
    }

   
}
