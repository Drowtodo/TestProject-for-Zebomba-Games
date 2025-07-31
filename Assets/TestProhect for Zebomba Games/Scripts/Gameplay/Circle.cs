using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer))]
public class Circle : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Material _material;
    private Pendulum _pendulum;
    [SerializeField, Tooltip("Частицы запускаемые при исчезновении Circle")]
    private ParticleSystem Particles;
    /// <summary>
    /// Цвет Circle
    /// </summary>
    public Color Color { get { return _sr.material.color; } }

    /// <summary>
    /// Инициализация Circle, должна вызываться до первого изпользования объекта Circle
    /// </summary>
    /// <param name="color">Цвет Circle</param>
    public void Init(Color color)
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        if(_sr == null) _sr = GetComponent<SpriteRenderer>();
        _material = _sr.material;
        _material.color = color;
    }

    /// <summary>
    /// Освобождает Circle от крепления к маятнику
    /// </summary>
    public void Free()
    {
        transform.SetParent(null);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _pendulum = null;
    }

    /// <summary>
    /// Circle закрепляется за маятником
    /// </summary>
    /// <param name="pendulum">Маятник, к которому прикрепляется Circle</param>
    public void Grab(Pendulum pendulum)
    {
        _rb.bodyType = RigidbodyType2D.Kinematic;
        transform.SetParent(pendulum.CircleAnchor);
        transform.localPosition = Vector3.zero;
        _pendulum = pendulum;
    }

    private void OnMouseDown()
    {
        if (_pendulum != null)
        {
            GameplayController.FreePendulum();//Освобождаем Circle по клику на нему
        }
    }

    /// <summary>
    /// Асинхронно проводим ичезновение Circle с проигрыванием "анимации"
    /// </summary>
    /// <returns></returns>
    public async UniTask ReleaseParticles()
    {
        Particles.Play();
        for(float i = 0.01f; i<1; i+=0.02f)
        {
            _material.SetFloat("_AlphaCliping", i);
            await UniTask.Yield();
        }
        Particles.Stop();
        await UniTask.Yield();
    }

    /// <summary>
    /// Возвращаем Circle в контейнер
    /// </summary>
    /// <param name="container">Transform контейнера</param>
    public void ReturnToContainer(Transform container)
    {
        _material.SetFloat("_AlphaCliping", 0);
        _rb.bodyType = RigidbodyType2D.Kinematic;
        transform.SetParent(container);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
        
    }
}
