using Cysharp.Threading.Tasks;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [Range(0f, 180f),Tooltip("Угол отклонения маятника")]
    public float angle = 45;
    [Range(0f, 360f), Tooltip("Местоположение центральной линии, отсносительно которой расчитывается отклонение")]
    public float offsetAngle = 0f;
    [Tooltip("Скорость движения мятника")]
    public float speed = 1.0f;
    [Tooltip("Transform места для крепления Circle")]
    public Transform CircleAnchor;
    /// <summary>
    /// Текущий закрепленный Circle
    /// </summary>
    public Circle CurrentCircle { get; private set; }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Cos(Time.realtimeSinceStartup * speed) + offsetAngle);//Раскачивание маятника
    }

    /// <summary>
    /// Освобождаем маятник
    /// </summary>
    public void Free()
    {
        if(CurrentCircle != null)
        {
            CurrentCircle.Free();
            CurrentCircle = null;
        }
    }

    /// <summary>
    /// Прикрепляем Circle к маятнику
    /// </summary>
    /// <param name="circle">Прикепляемый Circle</param>
    /// <returns></returns>
    public bool Add(Circle circle)
    {
        if(CurrentCircle == null)
        {
            circle.Grab(this);
            CurrentCircle = circle;
            return true;
        }
        return false;
    }

}
