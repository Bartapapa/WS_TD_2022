using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField]
    private float _cameraSpeed = 25f;

    [SerializeField]
    private float _rotationSpeed = 30;

    [SerializeField]
    private bool _canRotate = false;

    [SerializeField]
    private GameObject _cameraPivot;

    [SerializeField]
    private List<GameObject> _movementLimits = new List<GameObject>();

    [SerializeField]
    private List<float> _boxColliderSize = new List<float>();   
    
    [SerializeField]
    private List<float> _CameraYAxis = new List<float>();
    
    [SerializeField]
    private List<float> _cameraPos = new List<float>();

    private CinemachineConfiner _confiner;

    private BoxCollider _boxCollider;

	private GameManager _gameManager;

    private GameObject _movementLimit;

    private quaternion _rotationPos;

	private void Awake()
	{
        _gameManager = GameManager.Instance;
        
		_confiner = GetComponent<CinemachineConfiner>();
        _boxCollider = _confiner.m_BoundingVolume.GetComponent<BoxCollider>();
        _rotationPos = transform.rotation;
	}

	private void OnEnable()
	{
        _gameManager.GamePhaseChangeEvent_UE.RemoveListener(UpdateConfiner);
        _gameManager.GamePhaseChangeEvent_UE.AddListener(UpdateConfiner);
        _boxCollider.transform.localScale = new Vector3(_boxColliderSize[0], 1, _boxColliderSize[0]);
        _movementLimit = _movementLimits[0];
	}

	private void OnDisable()
	{
		_gameManager.GamePhaseChangeEvent_UE.RemoveListener(UpdateConfiner);
	}

    void LateUpdate()
    {
        if (_canRotate == true) 
        { 
            transform.RotateAround(_cameraPivot.transform.position, Vector3.up, -_rotationSpeed * Input.GetAxis("CameraRotation") * Time.deltaTime);
        }
			transform.localPosition = new Vector3
            (Mathf.Clamp(transform.localPosition.x + _cameraSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,
                -_movementLimit.transform.position.x, _movementLimit.transform.position.x), 
            
            transform.position.y,
			
            Mathf.Clamp(transform.localPosition.z + _cameraSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 
                -_movementLimit.transform.position.z, _movementLimit.transform.position.z));

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = _rotationPos;
        }

	}

    private void UpdateConfiner(GameManager.GamePhase fromPhase, GameManager.GamePhase toPhase)
    {
        if (toPhase == GameManager.GamePhase.Phase1)
        {
			_boxCollider.transform.localScale = new Vector3(_boxColliderSize[0], 1, _boxColliderSize[0]);            
            transform.position = new Vector3 (transform.position.x, _CameraYAxis[0], transform.position.z);
            _movementLimit = _movementLimits[0];
            _cameraPivot.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _cameraPos[0]);
        }
        if (toPhase == GameManager.GamePhase.Phase2)
        {
			_boxCollider.transform.localScale = new Vector3(_boxColliderSize[1], 1, _boxColliderSize[1]);
			transform.position = new Vector3 (transform.position.x, _CameraYAxis[1], transform.position.z);
            _movementLimit = _movementLimits[1];
            _cameraPivot.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _cameraPos[1]);
        }
        if (toPhase == GameManager.GamePhase.Phase3)
        {
            _boxCollider.transform.localScale = new Vector3(_boxColliderSize[2], 1, _boxColliderSize[2]);
            transform.position = new Vector3 (transform.position.x, _CameraYAxis[2], transform.position.z);
            _movementLimit = _movementLimits[2];
            _cameraPivot.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _cameraPos[2]);
        }
        if (toPhase == GameManager.GamePhase.Phase4)
        {
            _boxCollider.transform.localScale = new Vector3(_boxColliderSize[3], 1, _boxColliderSize[3]);
            transform.position = new Vector3 (transform.position.x, _CameraYAxis[3], transform.position.z);
            _movementLimit = _movementLimits[3];
            _cameraPivot.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _cameraPos[3]);
        }
    }
}
