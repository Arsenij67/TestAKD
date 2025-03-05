using Zenject;
using UnityEngine;
using System;
public class MotionControl : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private float rotateSpeedX = 100f; // Скорость вращения игрока
    [SerializeField] private float rotateSpeedY = 100f; // Скорость вращения камеры по вертикали
    [SerializeField] private float rotateSpeedXMobile = 10f; // Скорость вращения игрока на мобильных устройствах
    [SerializeField] private float rotateSpeedYMobile = 10f; // Скорость вращения камеры по вертикали на мобильных устройствах
    [SerializeField] private float minVerticalAngle = -40f; // Минимальный угол наклона камеры вверх
    [SerializeField] private float maxVerticalAngle = 40f; // Максимальный угол наклона камеры вниз

    private float currentVerticalAngle = 0f; // Текущий угол наклона камеры по вертикали
    private Vector3 _previousMousePosition; // Предыдущее положение мыши

    [Inject] private DynamicJoystick variableJoystickMove; // Джойстик для управления движением
    [Inject] private CharacterController charController; // Контроллер управления персонажем

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward * variableJoystickMove.Vertical + Vector3.right * variableJoystickMove.Horizontal);
        charController.Move(direction * speed * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        if (ShouldRotatePlayer())
        {
            print(999);
            Vector2 rotationInput = GetRotationInput();
            RotateHorizontally(rotationInput.x);
            RotateVertically(rotationInput.y);
        }
    }

    private bool ShouldRotatePlayer()
    {

        short countTouches = Convert.ToInt16(Input.GetMouseButton(0) || Input.GetMouseButton(1));
#if UNITY_ANDROID
        countTouches = (short) Input.touchCount;
#endif

        return (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && variableJoystickMove.Direction == Vector2.zero)
                || (variableJoystickMove.Direction != Vector2.zero && countTouches>1);
    }

    private Vector2 GetRotationInput()
    {
#if UNITY_EDITOR
        return GetMouseRotationInput();
#elif UNITY_ANDROID
        return GetTouchRotationInput();
#endif
    }

    private Vector2 GetMouseRotationInput()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - _previousMousePosition;
            _previousMousePosition = currentMousePosition;

            // Плавное изменение значения с использованием Lerp
            float smoothHorizontal = Mathf.Lerp(0, mouseDelta.x * rotateSpeedX * Time.fixedDeltaTime, 0.1f);
            float smoothVertical = Mathf.Lerp(0, mouseDelta.y * rotateSpeedY * Time.fixedDeltaTime, 0.1f);

            return new Vector2(smoothHorizontal, smoothVertical);
        }
        _previousMousePosition = Input.mousePosition;
        return Vector2.zero;
    }

    private Vector2 GetTouchRotationInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                return new Vector2(
                    touch.deltaPosition.x * rotateSpeedXMobile * Time.fixedDeltaTime,
                    touch.deltaPosition.y * rotateSpeedYMobile * Time.fixedDeltaTime
                );
            }
        }
        return Vector2.zero;
    }

    private void RotateHorizontally(float horizontalInput)
    {
        transform.Rotate(Vector3.up * horizontalInput);
    }

    private void RotateVertically(float verticalInput)
    {
        currentVerticalAngle -= verticalInput; // Инвертируем для естественного поворота
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);
        Camera.main.transform.localRotation = Quaternion.Euler(currentVerticalAngle, 0, 0);
    }
}