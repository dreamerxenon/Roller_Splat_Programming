using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
   public Rigidbody rb;
   public float speed = 15;
   public bool isTravelling;
   private Vector3 travelDirection;
   private Vector3 nextCollisionPosition;

  public int minSwipeRecognition = 500;
  private Vector2 swipePosCurrentFrame;
  private Vector2 swipePosLastFrame;
  private Vector2 currentSwipe;

  public Color solveColor;
  
  public AudioSource audioSource;
  public AudioClip soundEffect;
  private void Start()
  {
    solveColor = Random.ColorHSV(0.5f, 1);
    GetComponent<MeshRenderer>().material.color = solveColor;
    audioSource.clip =  soundEffect;
  }

   private void FixedUpdate()
   {
    if (isTravelling)
    {
    rb.velocity = speed * travelDirection;
    audioSource.Play();
    }

    Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
    int i = 0;
    while (i < hitColliders.Length)
    {
        GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
        if (ground && !ground.isColour)
        {
            ground.ChangeColor(solveColor);
        }
        i++;
    }
    if (nextCollisionPosition != Vector3.zero)
    {
        if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
        {
            isTravelling = false;
            travelDirection = nextCollisionPosition = Vector3.zero;
        }
    }
   
    if (isTravelling)
    return;
    
    if (Input.GetMouseButton(0))
    {
        swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (swipePosLastFrame != Vector2.zero)
        {
            currentSwipe = swipePosCurrentFrame - swipePosLastFrame;
        
        if (currentSwipe.sqrMagnitude < minSwipeRecognition)
        {
            return;
        }
        currentSwipe.Normalize();

        if(currentSwipe.x > -0.5f && currentSwipe.x < 0.5)
        {
           setDestination(currentSwipe.y  > 0? Vector3.forward: Vector3.back);
        }
        if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5)
        {
           setDestination(currentSwipe.x  > 0? Vector3.right: Vector3.left);

        }
        }
        swipePosLastFrame = swipePosCurrentFrame;
    }
    if (Input.GetMouseButtonUp(0))
    {
        swipePosLastFrame = swipePosCurrentFrame = Vector2.zero;
    }
   }

   private void setDestination(Vector3 direction)
   {
    travelDirection = direction;
    RaycastHit hit;
    if (Physics.Raycast(transform.position, direction,out hit, 100f))
    {
        nextCollisionPosition = hit.point;
    }
    isTravelling = true;
   }
}
