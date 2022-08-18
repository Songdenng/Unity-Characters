using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class ControllerCharacter : MonoBehaviour
{
    #region Variables
    private CharacterController characterController;    // CharacterController 를 컨트롤 할 수 있도록, 받아올 수 있는 변수
    private NavMeshAgent agent;
    private Camera camera;  // Click & Move에서 Camera picking이 필요

    private bool isGrounded = false;

    public LayerMask groundLayerMask;   // 땅에 떨어져 있는 거리 확인 변수를 위한 layerMask
    public float groundCheckDistance = 0.3f;    // 땅에 떨어져 있는 거리 확인 변수

    private Vector3 calcVelocity; // 저항력 등 계산

    #endregion Variables
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();  // 시작하면서 CharacterController 를 GetComponent로 가져옴 
        agent = GetComponent<NavMeshAgent>();

        // agent 의 이동시스템을 사용하지 않음 (Cotroller 로 구현하기 때문에 필수)
        agent.updatePosition = false;
        agent.updateRotation = true;

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Process mouse left button input
        if (Input.GetMouseButtonDown(0))
        {
            // Make rey from sceen to world
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // Check hit from ray
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
            {
                Debug.Log("We hit " + hit.collider.name + " " + hit.point);

                // Move our character to what we hit
                agent.SetDestination(hit.point);  // agent 가 destination으로 이동
            }

            // remainingDistance = 현재 agent 위치와 원하는 위치값의 거리가 어느정도 남았는지 수치적으로 갖고옴
            if (agent.remainingDistance > agent.stoppingDistance)   // 아직 가야할 거리가 남아있으면
            {
                characterController.Move(agent.velocity * Time.deltaTime);
            }
            else
            {
                characterController.Move(Vector3.zero);
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }
}
