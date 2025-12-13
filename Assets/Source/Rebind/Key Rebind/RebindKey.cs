using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindKey : MonoBehaviour, IPointerEnterHandler
{
    public bool Selected { get; private set; }


    public void StartWaitingRebind()
    {
        if (inputActionReference != null)
        {
            InputManager.LoadBindingOverride(actionName);
            GetBindingInfo();
            UpdateUI();
        }

        DoRebind();

        Selected = true;
        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCanceled += UpdateUI;
        Debug.Log("StartBind");
    }

    public void StopWaitingRebind()
    {
        Selected = false;
        Debug.Log("StopBind");

        _rebindingOperation?.Cancel();

        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCanceled -= UpdateUI;
    }

    [SerializeField] private InputActionReference inputActionReference; //this is on the SO

    [SerializeField] private bool excludeMouse = true;
    [Range(0, 10)] [SerializeField] private int selectedBinding;
    [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions;

    [Header("Binding Info - DO NOT EDIT")] [SerializeField]
    private InputBinding inputBinding;

    private int bindingIndex;

    private string actionName;

    [SerializeField] private Text rebindText;

    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

    private void OnEnable()
    {
        if (inputActionReference != null)
        {
            InputManager.LoadBindingOverride(actionName);
            GetBindingInfo();
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        Selected = false;
        StopWaitingRebind();
        _rebindingOperation?.Cancel();

        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCanceled -= UpdateUI;
    }

    private void OnValidate()
    {
        if (inputActionReference == null)
            return;

        GetBindingInfo();
        UpdateUI();
    }

    private void GetBindingInfo()
    {
        if (inputActionReference.action != null)
            actionName = inputActionReference.action.name;

        if (inputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    private void UpdateUI()
    {
        if (rebindText != null)
        {
            if (Application.isPlaying)
            {
                rebindText.text = InputManager.GetBindingName(actionName, bindingIndex);
            }
            else
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
        }
    }

    private void DoRebind()
    {
        InputManager.StartRebind(actionName, bindingIndex, rebindText, excludeMouse, out _rebindingOperation);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Selected = true;
    }
}