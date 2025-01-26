using System.Collections;
using UnityEngine;

/// <summary>
/// Defines the behaviour of the monster that attacks the bubble.
/// </summary>
public class Monster_Bubble_Attacker : SimpleMonster {

    private GameObject _bubble;
    private BubbleTargetsGenerator bubbleTargetsGenerator;
    private Animator animator;

    [SerializeField]
    private GameObject _bubbleHitParticle;

    private bool _isAttachedToTheBubble = false;
    private int indexInBubble = 0;

    private bool _isLeavingTheGround = true;
    private Vector3 _targetPosition;

    private float _timeElapsedBeforeReOrientate = 1;
    private readonly float _timeThresholdToReOrientate = 1;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        _bubble = GameObject.Find("Bubble");
        bubbleTargetsGenerator = _bubble.GetComponent<BubbleTargetsGenerator>();
        animator = GetComponent<Animator>();

        StartCoroutine(LeaveTheGround());
    }

    private void FixedUpdate() {

        _timeElapsedBeforeReOrientate += Time.deltaTime;

        if (_isLeavingTheGround) {
            // DO NOTHING.
            return;
        }

        if (!_isAttachedToTheBubble) {
            Move();
            return;
        }

        if (_isAttachedToTheBubble &&
            Vector3.Distance(transform.position, bubbleTargetsGenerator.bubbleTargetPoints[indexInBubble].transform.position) > 0.1f) {
            transform.position = bubbleTargetsGenerator.bubbleTargetPoints[indexInBubble].transform.position;
        }
    }

    private void Move() {

        GetClosestPoint();

        // Only recalculate the orientation if the postion changed in order to avoid flickering.
        if (_timeElapsedBeforeReOrientate > _timeThresholdToReOrientate) {
            _timeElapsedBeforeReOrientate = 0;
            transform.LookAt(_targetPosition, Vector3.back);
        }

        DoBasicMove();
    }

    private Vector3 GetClosestPoint() {
        float closestDistance = Mathf.Infinity;
        int index = 0;

        Vector3 newTargetPosition = Vector3.zero;

        // 1) Identify the closest part of the bubble.
        bubbleTargetsGenerator.bubbleTargetPoints.ForEach(target => {
            float currentMinDistance = Vector3.Distance(transform.position, target.transform.position);
            if (closestDistance > currentMinDistance) {
                closestDistance = currentMinDistance;
                newTargetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                indexInBubble = index;
            }
            index++;
        });

        _targetPosition = newTargetPosition;

        return _targetPosition;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Bubble") {
            _isAttachedToTheBubble = true;
            animator.Play("Bite");
            _mainSource.clip = _inBubbleClip;
            _mainSource.Play();
            _bubbleHitParticle.GetComponent<ParticleSystem>().Play();
        }
    }

    private IEnumerator LeaveTheGround() {
        float spawnAnimationElapsedTime = 0;
        float animationDuration = 3;
        Vector3 scaleBeforeReduction = transform.localScale;
        transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(1f);

        GetClosestPoint();

        // Only recalculate the orientation if the postion changed in order to avoid flickering.
        if (_timeElapsedBeforeReOrientate > _timeThresholdToReOrientate) {
            _timeElapsedBeforeReOrientate = 0;
            transform.LookAt(_targetPosition, Vector3.back);
        }

        while (spawnAnimationElapsedTime < animationDuration - 0.1) {
            transform.localScale = spawnAnimationElapsedTime / animationDuration * scaleBeforeReduction;
            yield return new WaitForSeconds(0.01f);
            spawnAnimationElapsedTime += Time.deltaTime;
        }

        transform.localScale = transform.localScale;
        _isLeavingTheGround = false;
    }
}
