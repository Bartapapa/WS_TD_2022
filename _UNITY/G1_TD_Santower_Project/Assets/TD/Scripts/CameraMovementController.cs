using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField]
    private float _cameraSpeed = 25f;

    [SerializeField]
    private List<GameObject> _movementLimits = new List<GameObject>();

    [SerializeField]
    private List<float> _boxColliderSize = new List<float>();   
    
    [SerializeField]
    private List<float> _CameraYAxis = new List<float>();

    private CinemachineConfiner _confiner;

    private BoxCollider _boxCollider;

	private GameManager _gameManager;

    private GameObject _movementLimit;


	private void Awake()
	{
        _gameManager = GameManager.Instance;
        
		_confiner = GetComponent<CinemachineConfiner>();
        _boxCollider = _confiner.m_BoundingVolume.GetComponent<BoxCollider>();
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
		transform.position = new Vector3
            (Mathf.Clamp(transform.position.x + _cameraSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,
                -_movementLimit.transform.position.x, _movementLimit.transform.position.x), 
            
            transform.position.y,
			
            Mathf.Clamp(transform.position.z + _cameraSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 
                -_movementLimit.transform.position.z, _movementLimit.transform.position.z));

	}

    private void UpdateConfiner(GameManager.GamePhase fromPhase, GameManager.GamePhase toPhase)
    {
        if (toPhase == GameManager.GamePhase.Phase1)
        {
			_boxCollider.transform.localScale = new Vector3(_boxColliderSize[0], 1, _boxColliderSize[0]);            
            transform.position = new Vector3 (transform.position.x, _CameraYAxis[0], transform.position.z);
            _movementLimit = _movementLimits[0];
        }
        if (toPhase == GameManager.GamePhase.Phase2)
        {
			_boxCollider.transform.localScale = new Vector3(_boxColliderSize[1], 1, _boxColliderSize[1]);
			transform.position = new Vector3 (transform.position.x, _CameraYAxis[1], transform.position.z);
            _movementLimit = _movementLimits[1];
        }
        if (toPhase == GameManager.GamePhase.Phase3)
        {
            _boxCollider.transform.localScale = new Vector3(_boxColliderSize[2], 1, _boxColliderSize[2]);
            transform.position = new Vector3 (transform.position.x, _CameraYAxis[2], transform.position.z);
            _movementLimit = _movementLimits[2];
        }
        if (toPhase == GameManager.GamePhase.Phase4)
        {
            _boxCollider.transform.localScale = new Vector3(_boxColliderSize[3], 1, _boxColliderSize[3]);
            transform.position = new Vector3 (transform.position.x, _CameraYAxis[3], transform.position.z);
            _movementLimit = _movementLimits[3];
        }
    }
}
