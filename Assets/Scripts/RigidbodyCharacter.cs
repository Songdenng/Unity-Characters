using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCharacter : MonoBehaviour
{
    #region Variables
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float dashDistance = 5f;

    private Rigidbody rigidbody;    // rigidbody�� ��Ʈ�� �� �� �ֵ���, �޾ƿ� �� �ִ� ����

    private Vector3 inputDirection = Vector3.zero;      // ������� �Է¿� ���� ���⼺�� ����ϱ� ���� ���� 

    private bool isGrounded = false;    // ���� ���� ������ ������ �����ϰ� �ϱ� ���� ���� �پ��ִ��� Ȯ�� �ϴ� ����(2)
    public LayerMask groundLayerMask;   // ���� ������ �ִ� �Ÿ� Ȯ�� ������ ���� layerMask(2)
    public float groundCheckDistance = 0.3f;    // ���� ������ �ִ� �Ÿ� Ȯ�� ����(2)

    #endregion Variables
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();  // �����ϸ鼭 rigidbody�� GetComponent�� ������ 
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundStatus();    // �������ڸ��� ������ Ȯ��(2)

        //  Process user inputs
        inputDirection = Vector3.zero;  // �Է°� �ޱ� �� �ʱ�ȭ
        // �¿쿡 ���� �Է°��� GetAxis("Horizontal"), GetAxis("Vertical") �� �޾� ��
        inputDirection.x = Input.GetAxis("Horizontal"); 
        inputDirection.z = Input.GetAxis("Vertical");
        if (inputDirection != Vector3.zero) // �Է��� ����ǰ� �ִٸ�
        {
            transform.forward = inputDirection;     // ���� ĳ���� ������ �Է��� �������� ����
        }

        // Process jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // jump�� ���� ����
            Vector3 jumpValocity = Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            // �� ������ rigidbody�� AddFroce�� ����. ������������ ���� �Էµ� jumpValocity�� �Էµ�
            rigidbody.AddForce(jumpValocity, ForceMode.VelocityChange); // ���� ���ϴ� �÷��״� ���ӵ��� �����ϴ� ������ ����
        }

        // Process dash input
        if (Input.GetButtonDown("Dash"))
        {
            // dash�� ���� ����
            Vector3 dashVelocity = Vector3.Scale(transform.forward,
                dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / - Time.deltaTime),
                0,
                (Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime)));
            // �� ������ rigidbody�� AddFroce�� ����. ������������ ���� �Էµ� dashVelocity�� �Էµ�
            rigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
        }

    }

    // �Է¿� ���� �̵�ó��
    private void FixedUpdate()      // ���������� ����ϱ� ������. ������ �����Ӱ� ������� ���������� ȣ���� �Ǵ� ������Ʈ �Լ�
    {
        rigidbody.MovePosition(rigidbody.position + inputDirection * speed * Time.fixedDeltaTime);
    }

    #region Helper Methos
    void CheckGroundStatus()        // (2)
    {
        RaycastHit hitInfo;     // RaycastHit = �浹�� ������Ʈ�� ������ ������

#if UNITY_EDITOR
        Debug.DrawLine(transform.position + (Vector3.up * 0.01f),
            transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif

        // �߿��� ��¦ �� ���� ����
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f),
            Vector3.down, out hitInfo, groundCheckDistance, groundLayerMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    #endregion Helper Mehtos
}
