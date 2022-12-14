using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public UnityEvent<AbilityDescription> AbilitySlotClicked;
    public UnityEvent OnCooldownFinished;

    [SerializeField]
    private AbilityDescription _ability;

    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _icon;

    [Header("Ability cooldown")]
    [SerializeField]
    private Timer _timer;
    [SerializeField]
    private GameObject _coolDownVisualizer;

    public AbilityDescription Ability => _ability;

    private void Awake()
    {
        AbilityDescription newAbilityDescription = ScriptableObject.CreateInstance<AbilityDescription>();
        newAbilityDescription.Initialize(_ability.Name, _ability.TargetingReticle, _ability.Icon, _ability.CoolDown);

        _ability = newAbilityDescription;

        UpdateSlot();
    }

    private void UpdateSlot()
    {
        _icon.sprite = _ability.Icon;
        _timer.Set(_ability.CoolDown);
        UpdateCDVisualizer(1f);
    }

    private void OnEnable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void Update()
    {
        _timer.Update();

        if (_timer.IsRunning)
        {
            ConvertTimerProgressToCDVisualizer();
        }
    }

    private void UpdateCDVisualizer(float scaleX)
    {
        _coolDownVisualizer.transform.localScale = new Vector3(scaleX * 3, scaleX * 3, scaleX * 3);

        if (scaleX >= 1f)
        {
            OnCooldownFinished.Invoke();
        }
    }

    private void ConvertTimerProgressToCDVisualizer()
    {
        UpdateCDVisualizer(_timer.Progress);
    }

    public void OnButtonClicked()
    {
        AbilitySlotClicked.Invoke(_ability);
    }

    public void StartCooldownTimer()
    {
        if (!_timer.IsRunning)
        {
            _timer.Start();
        }
    }

    public void SetAbility(AbilityDescription ability)
    {
        _ability = ability;
    }
}
