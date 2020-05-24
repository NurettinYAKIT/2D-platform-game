using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 2; 
    public bool hasRightBuddy = false;
    public bool hasLeftBuddy = false;
    public bool reverseSclae = false; //used if the element is not tilable
    private float spriteWidth = 0f; //width of our element

    private Camera cam;
    private Transform myTransform;

    void Awake() {
        cam = Camera.main;
        myTransform = transform;    
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasLeftBuddy==false||hasRightBuddy==false){
            float camHorizantalExtend = cam.orthographicSize * Screen.width/Screen.height;
            // calculate the x position where the camera can see the edge of the sprite.
            float edgeVisiblePositionRight = (myTransform.position.x+spriteWidth/2) - camHorizantalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x-spriteWidth/2) + camHorizantalExtend;

            if(cam.transform.position.x >= edgeVisiblePositionRight-offsetX && hasRightBuddy ==false){
                MakeNewBuddy(1);
                hasRightBuddy = true;
            }else if(cam.transform.position.x <= edgeVisiblePositionLeft+offsetX && hasLeftBuddy ==false){
                MakeNewBuddy(-1);
                hasLeftBuddy =true;
            }
        }
    }

    void MakeNewBuddy(int rightOrLeft){
        Vector3 newPosition = new Vector3(myTransform.position.x +spriteWidth*rightOrLeft,myTransform.position.y,myTransform.position.z);

        //Create new buddy
        Transform newBuddy = (Transform)Instantiate(myTransform, newPosition, myTransform.rotation);

        if(reverseSclae==true){
            newBuddy.localScale = new Vector3(newBuddy.localScale.x*-1,newBuddy.localScale.y,newBuddy.localScale.z);
        }
        newBuddy.parent = myTransform.parent;
        
        if(rightOrLeft>0){
            newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;
        }else{
            newBuddy.GetComponent<Tiling>().hasRightBuddy = true;
        }
    }
}
