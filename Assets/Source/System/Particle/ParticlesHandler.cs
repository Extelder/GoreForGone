using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesHandler : MonoBehaviour
{
    [field: SerializeField] public GameObject BloodParticle { get; private set; }
    [field: SerializeField] public GameObject ObjectHitParticle { get; private set; }
}
