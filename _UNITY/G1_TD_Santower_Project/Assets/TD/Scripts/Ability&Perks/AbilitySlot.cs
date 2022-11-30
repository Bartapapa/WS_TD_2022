using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public UnityEvent AbilitySlotClicked;

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
        _coolDownVisualizer.transform.localScale = new Vector3(scaleX, 1, 1);

        if (scaleX >= 1f)
        {
            _coolDownVisualizer.GetComponent<Image>().color = Color.green;
        }
        else
        {
            _coolDownVisualizer.GetComponent<Image>().color = Color.red;
        }
    }

    private void ConvertTimerProgressToCDVisualizer()
    {
        UpdateCDVisualizer(_timer.Progress);
    }

    private void OnButtonClicked()
    {
        //TODO Only initialize cooldown afterwards.

        if (!_timer.IsRunning)
        {
            _timer.Start();

            AbilitySlotClicked.Invoke();
        }
        //What do when clicked?
        //Change PlayerPickerController's state.
    }
}
