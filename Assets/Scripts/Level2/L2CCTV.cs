using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2CCTV : MonoBehaviour
{
    [SerializeField] private Vector3 aimDirection;

    private L2Player player;
    [SerializeField] private Transform pfFieldOfView;
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 50f;

    private FieldOfView fieldOfView;

    private enum State
    {
        Surveilling,
        Alert,
        Busy
    }

    private State state;
    private Vector3 lastMoveDir;

    public Vector3 positionToMoveTo;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<L2Player>();
        state = State.Surveilling;
        lastMoveDir = aimDirection;

        fieldOfView = Instantiate(pfFieldOfView, null).GetComponent<FieldOfView>();
        fieldOfView.transform.parent = transform;
        fieldOfView.transform.localPosition = new Vector3(0, 0, -5);
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);

        StartCoroutine(LerpPosition(positionToMoveTo, 25));
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.Surveilling:
                FindTargetPlayer();
                break;
            case State.Alert:
                Alert();
                break;
            case State.Busy:
                break;
        }

        if (fieldOfView != null)
        {
            fieldOfView.SetOrigin(transform.position);
            fieldOfView.SetAimDirection(GetAimDir());
        }

        Debug.DrawLine(transform.position, transform.position + GetAimDir() * 10f);
    }

    IEnumerator LerpPosition(Vector3 target, float duration)
    {
        float time = 0;
        Vector3 startPosition = lastMoveDir;
        while (time < duration)
        {
            lastMoveDir = Vector3.Lerp(startPosition, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        lastMoveDir = target;
    }

    private void FindTargetPlayer()
    {
        if (Vector3.Distance(GetPosition(), player.GetPosition()) < viewDistance)
        {
            // Player inside viewDistance
            Vector3 dirToPlayer = (player.GetPosition() - GetPosition()).normalized;
            if (Vector3.Angle(GetAimDir(), dirToPlayer) < fov / 2f)
            {
                // Player inside Field of View
                RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), dirToPlayer, viewDistance);
                if (raycastHit2D.collider != null)
                {
                    // Hit something
                    if (raycastHit2D.collider.gameObject.GetComponent<L2Player>() != null)
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

        Vector3 targetPosition = player.GetPosition();
        Vector3 dirToTarget = (targetPosition - GetPosition()).normalized;
        lastMoveDir = dirToTarget;

        FindObjectOfType<GameManager>().EndGame();

        //Alert other guards
        //gameover
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetAimDir()
    {
        return lastMoveDir;
    }

}
