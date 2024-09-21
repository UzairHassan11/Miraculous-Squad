using System.Collections;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Transforms")]

    public VariableJoystick joystickControl;

    public float velocity;
    public float turnSpeed = 10;
    public float gravityForce = 10;
    public float scaleMultipleOnHit = 1.1f;
    [Header("Other Variables")]
    public bool isControlActive;
    bool move, canRotate = true;

    Rigidbody rb;
    //public Animator animator;

    public float angle;
    public float groundDistance;
    float deadzone = 0.01f;

    Vector2 input;
    Quaternion targetRotation;
    Quaternion lastRotation;

    Transform cam;

    bool animating, boostMode;

    public SkinnedMeshRenderer skin;

    public AnimationController animationController;

    [HideInInspector]
    public Transform mTransform;

    public bool isGrounded;

    #endregion

    #region Unity-Methods
    private void Awake()
    {
        mTransform = transform;
    }

    private void Start()
    {
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        lastRotation = transform.rotation;
        //animator = GetComponentInChildren<Animator>();
        isControlActive = false;
        move = true;
        originalScale = mTransform.localScale.x;
        //ResetFirstTimeFall();
    }

    private void FixedUpdate()
    {
        //if (!GameManager.instance.isRunning)
        //    return;

        //Debug.Log("IsGrounded " + IsGrounded());
        if(!IsGrounded() && !animating)
        {
            GetGrounded();
            animationController.SetBool("Falling", true);
            isGrounded = false;
            return;
        }
        else
        {
            animationController.SetBool("Falling", false);
            isGrounded = true;
            if (fallingFirstTime)
            {
                fallingFirstTime = false;
                EnemyIndicator.instance.SetVisibility(true);
                if(SaveData.Instance.Haptic)
                    MMVibrationManager.Haptic(HapticTypes.SoftImpact);
                if(!GameManager.instance.levelManager.ShowStartDialogue())
                   GameManager.instance.uiManager.ShowGameplayPanel(true);
            }
        }

        if (isControlActive)
        {
            HandleAnimationSpeed();

            if (move)
            {
                // Move();
                Move2();
                //PlayRun();
            }
            else
            {
                IdleState(false);
            }

            GetInput();

            if (input.magnitude > deadzone)
            {
                CalculateDirection();
                if(canRotate)
                    Rotate();
            }
        }
    }

    #endregion

    #region Other-Methods
    public void ResetFirstTimeFall()
    {
        fallingFirstTime = true;
        PlayerManager.instance.healthController.SetHealthManually(0);
        PlayerManager.instance.enemiesTrigger.shootingRangeVisualiser.SetToMinScale();
        GameManager.instance.uiManager.ShowGameplayPanel(false);
        EnemyIndicator.instance.SetVisibility(false);
    }

    [SerializeField]
    bool fallingFirstTime;
    public void ResetAll()
    {
        if(dead)
           animationController.SetTrigger("Alive");
        //joystickControl.SetVisibility(true);
        dead = false;
        CanRotate(true);
    }

    public GameObject speedParticles;
    public void IncreaseSpeed(bool state)
    {
        boostMode = state;
        velocity = state ? 4.5f : 2.7f;
        //animator.SetFloat("speed", state ? 1.5f : 1);
        animationController.SetFloat("speed", state ? 1.5f : 1);
        speedParticles.SetActive(state);
    }

    public void PlaceMeAt(Transform t)
    {
        mTransform.SetPositionAndRotation(t.position, t.rotation);
        ResetFirstTimeFall();
    }

    public void PlaceMeAfterRevive()
    {
        Vector3 pos = mTransform.position;
        rb.velocity = Vector3.zero;
        mTransform.position = new Vector3(pos.x, pos.y + 5, pos.z);
        ResetFirstTimeFall();
    }

    public void ChangeMyMaterial(Material mat)
    {
        skin.material = mat;
    }

    public void CanRotate(bool state)
    {
        canRotate = state;
    }

    public void Animating(bool state)
    {
        rb.velocity = Vector3.zero;

        rb.angularVelocity = Vector3.zero;

        Move(!state);

        CanRotate(!state);

        GetComponent<CapsuleCollider>().enabled = !state;

        animating = state;
        //angle = 0;
    }

    public void Move(bool state)
    {
        move = state;
    }

    void IdleState(bool stopSmoothly)
    {
        input = Vector2.zero;
        transform.rotation = lastRotation;
        if (!stopSmoothly)
        {
            rb.velocity = Vector3.zero;

            PlayIdle();
        }
        else
        {
            StartCoroutine(StopSmoothly(rb.velocity, 0.5f, 0.05f));
        }
    }

    IEnumerator StopSmoothly(Vector3 startVel, float speedVal ,float duration)
    {
        float counter = 0f;

        while(counter < duration)
        {
            rb.velocity = Vector3.Lerp(startVel, Vector3.zero, counter / duration);
            counter += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector3.zero;
        PlayIdle();
    }

    void GetInput()
    {
        input.x = joystickControl.Direction.x;
        input.y = joystickControl.Direction.y;
    }

    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        lastRotation = transform.rotation;
    }

    void Move()
    {
        rb.velocity = transform.forward * velocity;
    }

    public float changeableVelocity = 0;
    // move forward amount with joystick 
    void Move2()
    {
        changeableVelocity = Mathf.Lerp(0, velocity,  Mathf.Abs(joystickControl.Horizontal) + Mathf.Abs(joystickControl.Vertical));
        rb.velocity = transform.forward * changeableVelocity;
    }

    public void EndControls()
    {
        //animator.SetFloat("speed", 0);
        animationController.SetFloat("speed", 0);
        isControlActive = false;
        move = false;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.Sleep();
    }

    public void EnableControls(bool state)
    {
        isControlActive = state;
        Move(state);
        if (!isControlActive)
            EndControls();
    }

    void GetGrounded()
    {
        //print("getting grounded");
        rb.AddForce(-transform.up * gravityForce, ForceMode.VelocityChange);
        //rb.velocity = -transform.up * 10;
    }

    bool IsGrounded()
    {
        //Debug.DrawRay(transform.position, -Vector3.up * (GetComponent<Collider>().bounds.extents.y + 0.12f));
        return Physics.Raycast(transform.position, -Vector3.up, groundDistance);
    }
    #endregion

    #region Animations

    void HandleAnimationSpeed()
    {
        if (!boostMode)
        {
            //animator.SetFloat("speed", Mathf.Abs(variableJoystick.Horizontal) + Mathf.Abs(variableJoystick.Vertical));
            animationController.SetFloat("speed", Mathf.Abs(joystickControl.Horizontal) + Mathf.Abs(joystickControl.Vertical));
        }
    }
    
    public void PlayRun()
    {
        //animator.SetBool("canRun", true);
        animationController.SetBool("canRun", true);
    }

    public void PlayIdle()
    {
       //animator.SetBool("canRun", false);
        animationController.SetBool("canRun", false);
    }

    public void PlayWinAnim()
    {
        //animator.SetTrigger("Win");
        animationController.SetTrigger("Win");
    }

    public void PlayDieAnim()
    {
        //animator.SetTrigger("Fail");
        animationController.SetTrigger("Die");
    }

    bool dead;
    public void DieAndFail()
    {
        //animator.SetTrigger("Fail");
        // print("failed");
        animationController.SetTrigger("Die");
        move = false;
        canRotate = false;
        dead = true;
        joystickControl.SetVisibility();
        //GameManager.instance.LevelFailed();
    }

    public void PlayShoot()
    {
        animationController.SetTrigger("Shoot");
    }
    #endregion

    float originalScale;
    Tween scaleTween;
    public void PlayDamageAnimation()
    {
        //mTransform.MultiplyLocalScale(scaleMultipleOnHit);
        //mTransform.DOScale(originalScale, .1f).SetEase(Ease.Linear).;
        if (scaleTween != null)
            if (scaleTween.IsPlaying())
            {
                scaleTween.Kill(true);
                //print("was playing it scale animation");
            }

        scaleTween = mTransform.DOScale(.1f, .1f).SetRelative(true).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    public void LookTowards(Transform target)
    {
        mTransform.LookTowards(target);
        StartCoroutine(ResetRotaion());
    }
    IEnumerator ResetRotaion()
    {
        canRotate = false;
        yield return new WaitForSeconds(.1f);
        canRotate = true;
    }

    public void TurnMe(bool state)
    {
        gameObject.SetActive(state);
    }
}