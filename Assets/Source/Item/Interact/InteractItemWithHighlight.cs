using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class InteractItemWithHighlight : InteractItem, IGrabbable
{
    [SerializeField] private float _breakDistance;

    [SerializeField] private MeshRenderer _meshRenderer;

    private Material[] _defaultMaterials;

    private bool _detected;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void Interact()
    {
        StartGrab();
        //Item.Interact();
    }

    public override void Detected()
    {
        if (_detected)
            return;

        _detected = true;

        _defaultMaterials = _meshRenderer.materials;

        Material[] mats = new Material[_defaultMaterials.Length + 1];
        for (int i = 0; i < _defaultMaterials.Length; i++)
        {
            mats[i] = _defaultMaterials[i];
        }

        mats[mats.Length - 1] = PlayerCharacter.Instance.InteractMaterial;

        _meshRenderer.materials = mats;
    }

    public override void Lost()
    {
        if (!_detected)
            return;
        _detected = false;
        _meshRenderer.materials = _defaultMaterials;
    }

    private ConfigurableJoint joint;
    private Rigidbody grabRb;

    public void StartGrab()
    {
        GameObject grabTarget = new GameObject("GrabTarget");
        grabTarget.transform.position = PlayerCharacter.Instance.PlayerInteract.GrabPoint.position;

        grabRb = grabTarget.AddComponent<Rigidbody>();
        grabRb.isKinematic = true;
        grabRb.interpolation = RigidbodyInterpolation.Interpolate;

        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = grabRb;

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;

        joint.xMotion = ConfigurableJointMotion.Free;
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;

        joint.linearLimit = new SoftJointLimit
        {
            limit = 0.05f
        };

        JointDrive drive = new JointDrive
        {
            positionSpring = 150f,
            positionDamper = 30f,
            maximumForce = Mathf.Infinity
        };

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;

        Observable.EveryFixedUpdate().Subscribe(_ =>
        {
            Vector3 grabPos = PlayerCharacter.Instance.PlayerInteract.GrabPoint.position;
            grabRb.MovePosition(grabPos);

            float delta = Vector3.Distance(transform.position, grabPos);
            if (delta > _breakDistance)
                StopGrab();
        }).AddTo(_disposable);
    }


    private void OnDisable()
    {
        _disposable?.Clear();
    }

    public void StopGrab()
    {
        _disposable.Clear();
        Destroy(joint);
    }
}