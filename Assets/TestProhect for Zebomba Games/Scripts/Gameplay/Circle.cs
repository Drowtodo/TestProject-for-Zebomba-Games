using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer))]
public class Circle : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Material _material;
    private Pendulum _pendulum;
    [SerializeField, Tooltip("������� ����������� ��� ������������ Circle")]
    private ParticleSystem Particles;
    /// <summary>
    /// ���� Circle
    /// </summary>
    public Color Color { get { return _sr.material.color; } }

    /// <summary>
    /// ������������� Circle, ������ ���������� �� ������� ������������� ������� Circle
    /// </summary>
    /// <param name="color">���� Circle</param>
    public void Init(Color color)
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        if(_sr == null) _sr = GetComponent<SpriteRenderer>();
        _material = _sr.material;
        _material.color = color;
    }

    /// <summary>
    /// ����������� Circle �� ��������� � ��������
    /// </summary>
    public void Free()
    {
        transform.SetParent(null);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _pendulum = null;
    }

    /// <summary>
    /// Circle ������������ �� ���������
    /// </summary>
    /// <param name="pendulum">�������, � �������� ������������� Circle</param>
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
            GameplayController.FreePendulum();//����������� Circle �� ����� �� ����
        }
    }

    /// <summary>
    /// ���������� �������� ����������� Circle � ������������� "��������"
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
    /// ���������� Circle � ���������
    /// </summary>
    /// <param name="container">Transform ����������</param>
    public void ReturnToContainer(Transform container)
    {
        _material.SetFloat("_AlphaCliping", 0);
        _rb.bodyType = RigidbodyType2D.Kinematic;
        transform.SetParent(container);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
        
    }
}
