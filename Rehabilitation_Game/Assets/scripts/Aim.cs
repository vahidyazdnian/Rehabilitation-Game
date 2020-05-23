using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float initial_x= 12.602f;
    [SerializeField] float initial_y= 6.246f;
    void Start()
    {
        transform.position = new Vector2(initial_x, initial_y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
