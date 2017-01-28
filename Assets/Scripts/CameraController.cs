using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;
    private float speed = .25f;
    private Vector3 prevPos;
    private Vector3 cameraPos;

    // Use this for initialization
    void Start () {
		offset = transform.position - player.transform.position;
        cameraPos = Vector3.zero;
    }

    void Update()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 forward = player.transform.forward * 3.0f;

        Vector3 needPos = player.transform.position - forward;


        needPos.y += 1.0f;
        if (prevPos.y > needPos.y)
            needPos.y = prevPos.y;
        else
            prevPos.y = needPos.y;

        transform.position = Vector3.SmoothDamp(transform.position, needPos,
                                                ref velocity, 0.1f);
        cameraPos = player.transform.position;
        //cameraPos.y = 75;
        transform.LookAt(cameraPos);

        Vector3 v = player.transform.rotation.eulerAngles;
        Quaternion rotation = Quaternion.Euler(player.transform.position.x, transform.rotation.y, v.z);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * speed);
        
        //transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * speed);
        /*
        Vector3 direction = player.transform.rotation * player.transform.forward;
        float playerAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        direction = transform.rotation * transform.parent.forward;
        float cameraAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; ;

        // difference in orientations
        float rotationDiff = Mathf.DeltaAngle(cameraAngle, playerAngle);

        // rotate around target by time-sensitive difference between these angles
        transform.RotateAround(player.transform.position, Vector3.up, rotationDiff * Time.deltaTime);
        */
    }

    // Update is called once per frame
    void LateUpdate () {
		//transform.position = player.transform.position + offset;
	}
}
