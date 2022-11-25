using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField]
    private GameObject _movementLimits;

    [SerializeField]
    private float _cameraSpeed = 5f;

    void LateUpdate()
    { 
		transform.position = new Vector3
            (Mathf.Clamp(transform.position.x + _cameraSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,
                -_movementLimits.transform.position.x, _movementLimits.transform.position.x), 
            
            transform.position.y,
			
            Mathf.Clamp(transform.position.z + _cameraSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 
                -_movementLimits.transform.position.z, _movementLimits.transform.position.z));

	}
}
