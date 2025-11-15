using System;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    public Action<float> MoveActionEvent;
    public Action JumpActionEvent;

    [SerializeField] private HoldButton LeftUIBtn;
    [SerializeField] private HoldButton RightUIBtn;
    [SerializeField] private Button JumpUIBtn;

    [SerializeField] private KeyCode LeftAction = KeyCode.A;
    [SerializeField] private KeyCode RightAction = KeyCode.D;
    [SerializeField] private KeyCode JumpAction = KeyCode.Space;

    private void OnEnable()
    {
        JumpUIBtn.onClick.AddListener(() => JumpActionEvent?.Invoke());
    }

    private void OnDisable()
    {
        JumpUIBtn.onClick.RemoveListener(() => JumpActionEvent?.Invoke());
    }

    private void Update()
    {
        if (Input.GetKey(LeftAction) || LeftUIBtn.IsHeld)
            MoveActionEvent?.Invoke(-1f);

        if (Input.GetKey(RightAction) || RightUIBtn.IsHeld)
            MoveActionEvent?.Invoke(1f);

        if (Input.GetKeyDown(JumpAction))
            JumpActionEvent?.Invoke();
    }
}