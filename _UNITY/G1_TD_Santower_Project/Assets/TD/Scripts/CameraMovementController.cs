using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField]
    private GameObject _movementLimits;

    [SerializeField]
    private float _cameraSpeed = 25f;

    [SerializeField]
    private Vector3 _boxColliderSizeLevel1;   
    [SerializeField]
    private Vector3 _boxColliderSizeLevel2;    
    [SerializeField]
    private Vector3 _boxColliderSizeLevel3;    
    [SerializeField]
    private Vector3 _boxColliderSizeLevel4;

    private CinemachineConfiner _confiner;

    private BoxCollider _boxCollider;

	private GameManager _gameManager;


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
        _boxCollider.transform.localScale = _boxColliderSizeLevel1;
	}

	private void OnDisable()
	{
		_gameManager.GamePhaseChangeEvent_UE.RemoveListener(UpdateConfiner);
	}

	void LateUpdate()
    { 
		transform.position = new Vector3
            (Mathf.Clamp(transform.position.x + _cameraSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,
                -_movementLimits.transform.position.x, _movementLimits.transform.position.x), 
            
            transform.position.y,
			
            Mathf.Clamp(transform.position.z + _cameraSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 
                -_movementLimits.transform.position.z, _movementLimits.transform.position.z));

	}

    private void UpdateConfiner(GameManager.GamePhase fromPhase, GameManager.GamePhase toPhase)
    {
        if (toPhase == GameManager.GamePhase.Phase1)
        {
            _boxCollider.transform.localScale = _boxColliderSizeLevel1;
        }
        if (toPhase == GameManager.GamePhase.Phase2)
        {
            _boxCollider.transform.localScale = _boxColliderSizeLevel2;
        }
        if (toPhase == GameManager.GamePhase.Phase3)
        {
            _boxCollider.transform.localScale = _boxColliderSizeLevel3;
        }
        if (toPhase == GameManager.GamePhase.Phase4)
        {
            _boxCollider.transform.localScale = _boxColliderSizeLevel4;
        }
    }
}
