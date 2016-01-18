using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Capable of animating / interpolating Position, Rotation and Scale.
/// Intended to be used for Transform components.
/// </summary>
public class TransformAnimator {

    private Vector3 sourcePosition;
    private Vector3 targetPosition;
    private Vector3 currentPosition;
    public Vector3 position { get { return currentPosition; } }

    private Quaternion sourceRotation;
    private Quaternion targetRotation;
    private Quaternion currentRotation;
    public Quaternion rotation { get { return currentRotation; } }

    private Vector3 sourceScale;
    private Vector3 targetScale;
    private Vector3 currentScale;
    public Vector3 scale { get { return currentScale; } }

    public bool invertAfterFinish = false;
    public bool restartAfterFinish = false;
    public bool movingTowardsTarget = false;
    public bool reachedTarget = false;
    public bool movingBackToSource = false;
    public bool running = false;

    public bool animatePosition = true;
    public bool animateRotation = true;
    public bool animateScale = false;

    public float waitAfterFinishDuration = 0f;
    private float currentFinishWaitTime;

    public float animationDuration = 0.5f;
    private float currentAnimationTime = 0f;

    public TransformAnimator(Transform source, Transform target, float duration) {
        Init(source, target, duration);
    }

    public TransformAnimator(Vector3 sourcePosition, Vector3 targetPosition, float duration) {
        Init(sourcePosition, targetPosition, duration);
    }


    public TransformAnimator() {}

    public void Init(Vector3 sourcePosition, Vector3 targetPosition, float duration) {
        Init(sourcePosition, targetPosition, Quaternion.identity, Quaternion.identity, Vector3.one, Vector3.one, duration);
        animateRotation = false;
        animateScale = false;
    }

    public void Init(Transform source, Transform target, float duration) {
        Init(source.position, target.position, source.rotation, target.rotation, source.localScale, target.localScale, duration);
        animateScale = true;
    }

    public void Init(Vector3 sourcePosition, Vector3 targetPosition, 
        Quaternion sourceRotation, Quaternion targetRotation, 
        Vector3 sourceScale, Vector3 targetScale, float duration) 
    {
        this.sourcePosition = sourcePosition;
        this.targetPosition = targetPosition;
        this.sourceRotation = sourceRotation;
        this.targetRotation = targetRotation;
        this.sourceScale = sourceScale;
        this.targetScale = targetScale;
        this.animationDuration = duration;
        Start();
    }
    
    public void Start() {
        if (!running) {
            currentFinishWaitTime = waitAfterFinishDuration;
            currentAnimationTime = 0;
            currentPosition = sourcePosition;
            currentRotation = sourceRotation;
            currentScale = sourceScale;
            movingTowardsTarget = true;
            reachedTarget = false;
            movingBackToSource = false;
            running = true;
        }
    }

    public void InvertedStart() {
        if (!running) {
            currentFinishWaitTime = waitAfterFinishDuration;
            currentAnimationTime = animationDuration;
            movingBackToSource = true;
            movingTowardsTarget = false;
            reachedTarget = false;
            running = true;
        }
    }

    /// <summary>
    /// Sets all transform values to source values, running state to false,
    /// moving states to false and animation time to 0.
    /// </summary>
    public void Reset() {
        running = false;
        movingBackToSource = false;
        movingTowardsTarget = false;
        currentAnimationTime = 0f;
        currentFinishWaitTime = waitAfterFinishDuration;
        currentPosition = sourcePosition;
        currentRotation = sourceRotation;
        currentScale = sourceScale;
    }

    /// <summary>
    /// Updates the animated values with deltaTime and returns the running state after update.
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns>True if animation is still running after the update.</returns>
    public bool Tick(float deltaTime) {
        if (running) {
            // update animation progress
            Vector3 distanceToTarget = targetPosition - sourcePosition;
            Vector3 totalScaleDiff = targetScale - sourceScale;
            
            // interpolate from source to target
            if (movingTowardsTarget) {
                if (currentAnimationTime >= animationDuration) {
                    movingTowardsTarget = false;
                    reachedTarget = true;
                }
                else {
                    currentAnimationTime += deltaTime;
                    if (currentAnimationTime > animationDuration) {
                        currentAnimationTime = animationDuration;
                    }

                }

            }
            // wait on target
            if (reachedTarget) {
                if (invertAfterFinish) {
                    if (currentFinishWaitTime <= 0) {
                        reachedTarget = false;
                        movingBackToSource = true;
                    }
                    else {
                        currentFinishWaitTime -= deltaTime;
                    }
                }
                else {
                    running = false;
                }
            }
            // interpolate from target to source
            if (movingBackToSource) {
                if (currentAnimationTime <= 0) {
                    running = false;
                    if (restartAfterFinish) {
                        Start();
                    }
                }
                else {
                    currentAnimationTime -= deltaTime;
                    if (currentAnimationTime < 0) {
                        currentAnimationTime = 0;
                    }
                }
            }

            // update transform values
            float animationProgress = currentAnimationTime / animationDuration;
            if (animatePosition) {
                currentPosition = sourcePosition + (distanceToTarget * animationProgress);
            }
            if (animateRotation) {
                currentRotation = Quaternion.Lerp(sourceRotation, targetRotation, animationProgress);
            }
            if (animateScale) {
                currentScale = sourceScale + (totalScaleDiff * animationProgress);
            }
        }
        return running;
    }


    /// <summary>
    /// Sets the values of the given transform to initial target values.
    /// </summary>
    /// <param name="transform"></param>
    public void SetToTargetValues(Transform transform) {
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        transform.localScale = targetScale;
    }

    /// <summary>
    /// Sets the values of the given transform to initial source values.
    /// </summary>
    /// <param name="transform"></param>
    public void SetToSourceValues(Transform transform) {
        transform.position = sourcePosition;
        transform.rotation = sourceRotation;
        transform.localScale = sourceScale;
    }
}
