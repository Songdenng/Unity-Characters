using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCharacter : MonoBehaviour
{
    #region Variables
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float dashDistance = 5f;

    private Rigidbody rigidbody;    // rigidbody를 컨트롤 할 수 있도록, 받아올 수 있는 변수

    private Vector3 inputDirection = Vector3.zero;      // 사용자의 입력에 대한 방향성을 계산하기 위한 변수 

    private bool isGrounded = false;    // 땅에 있을 때에만 점프를 가능하게 하기 위해 땅에 붙어있는지 확인 하는 변수(2)
    public LayerMask groundLayerMask;   // 땅에 떨어져 있는 거리 확인 변수를 위한 layerMask(2)
    public float groundCheckDistance = 0.3f;    // 땅에 떨어져 있는 거리 확인 변수(2)

    #endregion Variables
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();  // 시작하면서 rigidbody를 GetComponent로 가져옴 
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundStatus();    // 시작하자마자 땅인지 확인(2)

        //  Process user inputs
        inputDirection = Vector3.zero;  // 입력값 받기 전 초기화
        // 좌우에 대한 입력값은 GetAxis("Horizontal"), GetAxis("Vertical") 로 받아 옴
        inputDirection.x = Input.GetAxis("Horizontal"); 
        inputDirection.z = Input.GetAxis("Vertical");
        if (inputDirection != Vector3.zero) // 입력이 진행되고 있다면
        {
            transform.forward = inputDirection;     // 현재 캐릭터 방향을 입력한 방향으로 설정
        }

        // Process jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // jump에 대한 공식
            Vector3 jumpValocity = Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            // 이 공식을 rigidbody에 AddFroce로 진행. 물리엔진으로 인해 입력된 jumpValocity가 입력됨
            rigidbody.AddForce(jumpValocity, ForceMode.VelocityChange); // 힘을 가하는 플래그는 가속도를 변경하는 것으로 설정
        }

        // Process dash input
        if (Input.GetButtonDown("Dash"))
        {
            // dash에 대한 공식
            Vector3 dashVelocity = Vector3.Scale(transform.forward,
                dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / - Time.deltaTime),
                0,
                (Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime)));
            // 이 공식을 rigidbody에 AddFroce로 진행. 물리엔진으로 인해 입력된 dashVelocity가 입력됨
            rigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
        }

    }

    // 입력에 대한 이동처리
    private void FixedUpdate()      // 물리엔진을 사용하기 때문에. 게임의 프레임과 상관없이 고정적으로 호출이 되는 업데이트 함수
    {
        rigidbody.MovePosition(rigidbody.position + inputDirection * speed * Time.fixedDeltaTime);
    }

    #region Helper Methos
    void CheckGroundStatus()        // (2)
    {
        RaycastHit hitInfo;     // RaycastHit = 충돌된 오브젝트의 정보를 가져옴

#if UNITY_EDITOR
        Debug.DrawLine(transform.position + (Vector3.up * 0.01f),
            transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif

        // 발에서 살짝 위 부터 시작
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
