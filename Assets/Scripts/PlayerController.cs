using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotateSpeed = 50f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] Transform itemHoldTransform;
    [SerializeField] Animator animator;
    Vector3 moveInput;
    [SerializeField] AnimatorOverrideController holdItemAnimatorController;
    [SerializeField] RuntimeAnimatorController normalAnimatorController;
    [SerializeField] SkinnedMeshRenderer hat;
    [SerializeField] SkinnedMeshRenderer body;

    private bool canControl;

    public Item HoldItem { get; private set; }

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        moveInput = Vector3.zero;
        HoldItem = null;
        animator.runtimeAnimatorController = normalAnimatorController;
        canControl = true;
    }

    public void GiveItem(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        if (item)
        {
            HoldItem = item;
            itemObject.transform.position = itemHoldTransform.position;
            itemObject.transform.SetParent(itemHoldTransform);
            animator.runtimeAnimatorController = holdItemAnimatorController;
            
        }

    }

    Vector3 moveDirection;
    private void Update()
    {
        if (!canControl || !GameManager.Instance.IsGameActive || GameManager.Instance.IsGameFinished)
        {
            animator.SetBool("isMoving", false);
            moveDirection = Vector3.zero;
            return;
        }


        //Normalized 
        moveDirection = new Vector3((moveInput).normalized.x, 0, (moveInput).normalized.z);

        //Rotation
        //Vector3 look = moveDirection;
        //look.y = transform.position.y;
        //transform.LookAt(transform.position + look);

        if (moveInput.sqrMagnitude > 0)
        {
            Quaternion rot = Quaternion.LookRotation(moveDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * rotateSpeed);
            transform.rotation = targetRotation;

            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        

        //Movement
        //transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

    }

    private void FixedUpdate()
    {
        
        rb.velocity = (moveDirection * moveSpeed);
    }

    public void InteractInput(bool pressed)
    {
        if (pressed)
        {

            Collider[] sphere = Physics.OverlapSphere(transform.position + transform.forward, 1, interactableLayer);
            if(sphere.Length>0)
            {
                for(int i = 0; i <sphere.Length; i++)
                {
                    Interactable interactable = sphere[i].gameObject.GetComponent<Interactable>();
                    if (interactable)
                    {
                        bool canInteract = interactable.Interact(this);

                        if (canInteract)
                        {
                            StartCoroutine(WaitRoutine());
                            //Animate
                            animator.SetTrigger("hit");
                        }
                    }
                }
                
            }
        }

        //string not = pressed ? "" : "NOT";
        //Debug.Log("Player is" + not + " interacting");
    }

    public void SetMaterial(Material mat)
    {
        hat.material = mat;
        body.material = mat;
    }

    SawInteractable saw;
    public void SetPlayerOnArrowsMechanic(SawInteractable saw)
    {
        this.saw = saw;
        canControl = false;
    }

    public void FinishArrowsMechanic()
    {
        this.saw = null;
        canControl = true;
    }

    public void ConsumeItem()
    {
        Destroy(HoldItem.gameObject);
        HoldItem = null;

        animator.runtimeAnimatorController = normalAnimatorController;
    }

    public GameObject RemoveItem()
    {
        GameObject item = HoldItem.gameObject;

        HoldItem = null;
        animator.runtimeAnimatorController = normalAnimatorController;

        return item;
    }

    public void MoveInput(string key, bool pressed)
    { 
        //Normal movement
        if (saw == null)
        {
            switch (key)
            {
                case "up":
                    moveInput.z = pressed ? 1f : 0f;
                    break;
                case "down":
                    moveInput.z = pressed ? -1f : 0f;
                    break;
                case "right":
                    moveInput.x = pressed ? 1f : 0f;
                    break;
                case "left":
                    moveInput.x = pressed ? -1f : 0f;
                    break;
            }
        }
        else //Special Arrows Mechanic
        {
            if (pressed)
                saw.SendArrowKey(key);
        }

        
    }

    IEnumerator WaitRoutine()
    {
        canControl = false;
        yield return new WaitForSeconds(0.1f);
        canControl = true;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, 1);
    }
}
