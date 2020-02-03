using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{

    [SerializeField] Train train;
    [SerializeField] Transform follow;

    void Update()
    {
        Vector3 direction = follow.transform.position - this.transform.position;
        direction.y = 0;

        transform.Translate(direction.normalized * Time.deltaTime * train.Speed, Space.World);

        Vector3 trainPosition = follow.transform.position;
        trainPosition.y = transform.position.y;

        transform.LookAt(trainPosition);
    }
}
