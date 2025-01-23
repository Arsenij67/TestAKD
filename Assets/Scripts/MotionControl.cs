using Zenject;
using UnityEngine;


public class MotionControl : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private float rotateSpeedX = 100f; // Скорость вращения игрока
    [SerializeField] private float rotateSpeedY = 100f; // Скорость вращения камеры по вертикали
    [SerializeField] private float rotateSpeedXMobile = 10f; // Скорость вращения игрока
    [SerializeField] private float rotateSpeedYMobile = 10f; // Скорость вращения камеры по вертикали
    [SerializeField] private float minVerticalAngle = -40f; // Минимальный угол наклона камеры вверх
    [SerializeField] private float maxVerticalAngle = 40f; // Максимальный угол наклона камеры вниз
    private float currentVerticalAngle = 0f; // Текущий угол наклона камеры по вертикали
    [Inject]
    private DynamicJoystick variableJoystickMove;// внедрение джойстика
    [Inject]
    private CharacterController charController;// контроллер управления героя для движения

    public void FixedUpdate()
    {
        //движение
        MovePlayer();
       
        //поворот
        RotatePlayer();

    }

    private void MovePlayer()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward * variableJoystickMove.Vertical + Vector3.right * variableJoystickMove.Horizontal);
        charController.Move(direction * speed * Time.fixedDeltaTime);
    }

    private void RotatePlayer()
    {
        Vector2 mousePos = Vector2.zero;

        // Если джойстик не используется (Direction == Vector2.zero), обрабатываем ввод мыши/касания
        if (variableJoystickMove.Direction.Equals(Vector2.zero))
        {
#if UNITY_EDITOR
            // В редакторе Unity используем ввод с мыши
            if (Input.GetMouseButton(0))
            {
                mousePos = new Vector2(Input.GetAxis("Mouse X") * rotateSpeedX * Time.fixedDeltaTime, Input.GetAxis("Mouse Y") * rotateSpeedY * Time.fixedDeltaTime);
            }
#elif UNITY_ANDROID
            // На Android используем ввод касания
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Обрабатываем перемещение пальца по экрану
                if (touch.phase == TouchPhase.Moved)
                {
                    mousePos = new Vector2(touch.deltaPosition.x * rotateSpeedXMobile * Time.fixedDeltaTime, touch.deltaPosition.y * rotateSpeedYMobile * Time.fixedDeltaTime);
                }
            }
#endif

            // Вращаем игрока по горизонтали
            transform.Rotate(Vector3.up * mousePos.x);

            // Вращаем камеру по вертикали, ограничивая угол
            currentVerticalAngle -= mousePos.y;
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

            // Применяем вращение к камере
            Camera.main.transform.localRotation = Quaternion.Euler(currentVerticalAngle, 0, 0);
        }
    }
}











