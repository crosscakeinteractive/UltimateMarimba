using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Axis_button_input : MonoBehaviour
{

     float sensitivity=2.5f;
     float gravity=5f;
     float deadzone=0.001f;
     float limit = 1f;
    RectTransform rect1;
    RectTransform rect2;
    public GameObject reject_superposed_ip;
    Camera camera;
    public New_Touch_button jump;
    public New_Touch_button attack;
    
    Animator anim;
    Animator anim2;

    public float Axis;
    public bool left;
    public bool right;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        rect1 = this.GetComponent<RectTransform>();
        rect2 = reject_superposed_ip.GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
        anim2 = reject_superposed_ip.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
       
        int touchcount = Input.touchCount;
        if (touchcount > 0)
        {
            Touch[] touches = Input.touches;
            for (int i = 0; i < touchcount; i++)
            {               
                Touch touch = Input.GetTouch(i);
                if(jump.rt_id || attack.rt_id)
                {
                    touch = Input.GetTouch(0);
                }
                left = RectTransformUtility.RectangleContainsScreenPoint(rect1, touch.position, camera);
                right = RectTransformUtility.RectangleContainsScreenPoint(rect2, touch.position, camera);
    
            }
            

        }
        else
        {
            left = false;
            right = false;
        }
        anim.SetBool("button_anim", left);
        anim2.SetBool("button_anim", right);
        movement();      
    }
    void movement()
    {
        if (left && !right)
        {
            Axis = Mathf.MoveTowards(Axis, -limit, sensitivity * Time.deltaTime);
        }
        else if (right && !left)
        {
            Axis = Mathf.MoveTowards(Axis, limit, sensitivity * Time.deltaTime);
        }
        if (!right && !left)
        {
            Axis = Mathf.MoveTowards(Axis, 0f, gravity * Time.deltaTime);
        }
        else if (left && right || (Axis <= deadzone && Axis > 0) || (-Axis >= -deadzone && -Axis < 0))
        {
            Axis = 0f;
        }
    }
}
