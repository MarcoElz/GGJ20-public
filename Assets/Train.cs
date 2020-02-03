using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    Transform wayPoints;
    int index = 0;
    float distanceLimit = 0.25f;
    [SerializeField]float speed = 1f;

    public float Speed { get { return speed; }  }

    void Start()
    {
        wayPoints = GameManager.Instance.GetBrokenPartsParent();// FindObjectOfType<WaypointsParent>().transform;
    }
    void Update()
    {
        if(index < wayPoints.childCount)
        {
            Transform wayPoint = wayPoints.GetChild(index);

            Vector3 myposition = transform.position;
            myposition.y = 0f;
            Vector3 wayPosition = wayPoint.position;
            wayPosition.y = 0f;

            float distance = Vector3.Distance(myposition, wayPosition);

            if (distance < distanceLimit)
                index++;

            Vector3 direction = wayPoint.transform.position - this.transform.position;
            direction.y = 0;

            transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);
            wayPosition.y = transform.position.y;
            transform.LookAt(wayPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            FixInteraction interaction = other.GetComponent<FixInteraction>();

            if (interaction != null)
            {
                if (interaction.IsBroken)
                {
                    //GameOver
                    GameManager.Instance.GameOver();
                }
            }
        }
    }
}
