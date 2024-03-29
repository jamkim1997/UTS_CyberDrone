using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour
{
    [SerializeField] private Vector3 aimDirection;
    [SerializeField] private Player player;
    [SerializeField] private Transform pfFieldOfView;
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 50f;
    [SerializeField] private float speed;
    public LayerMask layerMask;
    private CameraAnim cameraAnim;
    private FieldOfView fieldOfView;
    public bool isBusy;

    private enum State
    {
        Surveilling,
        Alert,
        Busy
    }

    private State state;
    private Vector3 lastMoveDir;
    public Vector3 positionToMoveTo;

    private void Awake()
    {
        cameraAnim = GetComponent<CameraAnim>();
        player = FindObjectOfType<Player>();
    }

    void Start()
    {
        state = State.Surveilling;
        lastMoveDir = aimDirection;

        fieldOfView = Instantiate(pfFieldOfView, null).GetComponent<FieldOfView>();
        fieldOfView.transform.parent = transform;
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);

        StartCoroutine(LerpPosition(positionToMoveTo, speed));
    }

    void Update()
    {
        switch (state){
            default:
            case State.Surveilling:
                FindTargetPlayer();
                break;
            case State.Busy:
                break;
        }
        if(!isBusy)
        {
            if (fieldOfView != null)
            {
                fieldOfView.SetOrigin(transform.position);
                fieldOfView.SetAimDirection(GetAimDir());
                cameraAnim.UpdateSprite(GetAimDir().x);
            }
        }
        

        Debug.DrawLine(transform.position, transform.position + GetAimDir() * 10f);
    }

    IEnumerator LerpPosition(Vector3 target, float speed)
    {
        while (true)
        {
        float time = Mathf.PingPong(Time.time * speed, 1);
        lastMoveDir = Vector3.Lerp(aimDirection, target, time);
        yield return null;
        }
    }

    private void FindTargetPlayer()
    {
        if (Vector2.Distance(GetPosition(), player.GetPosition()) < viewDistance)
        {
            // Player inside viewDistance
            Vector2 dirToPlayer = (player.GetPosition() - GetPosition()).normalized;
            if (Vector2.Angle(GetAimDir(), dirToPlayer) < fov / 2f)
            {
                // Player inside Field of View
                RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), dirToPlayer, viewDistance, layerMask);
                print(raycastHit2D.collider.name);
                if (raycastHit2D.collider != null)
                {
                    // Hit something
                    if (raycastHit2D.collider.gameObject.GetComponent<Player>() != null)
                    {
                        // Hit Player
                        Alert();
                    }
                    else
                    {
                        // Hit something else
                    }
                }
            }
        }
    }

    private void Alert()
    {
        state = State.Busy;
        player.enabled = false;
        Vector3 targetPosition = player.GetPosition();
        Vector3 dirToTarget = (targetPosition - GetPosition()).normalized;
        lastMoveDir = dirToTarget;

        FindObjectOfType<GameManager>().EndGame();

        Material material = Instantiate(fieldOfView.GetComponent<MeshRenderer>().material);
        fieldOfView.GetComponent<MeshRenderer>().material = material;
        material.SetColor("_FaceColor", Color.red);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetAimDir()
    {
        return lastMoveDir;
    }


    // Function for Level 5 Switch Off Camera Mechanic
    public void changeViewDistance(float Distance)
    {
        viewDistance = Distance;
        fieldOfView.SetViewDistance(viewDistance);
        //viewDistance = 10;
    }
}
