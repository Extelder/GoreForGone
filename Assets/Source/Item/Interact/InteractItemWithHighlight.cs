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

    private ConfigurableJoint joint;

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

    public void StartGrab()
    {
        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = null;
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = 0.2f;
        joint.linearLimit = limit;

        JointDrive drive = new JointDrive();
        drive.positionSpring = 400f;     
        drive.positionDamper = 80f;
        drive.maximumForce = 1500f;    

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;


        Observable.EveryFixedUpdate().Subscribe(_ =>
        {
            Vector3 grabPos = PlayerCharacter.Instance.PlayerInteract.GrabPoint.position;

            joint.connectedAnchor = grabPos;

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