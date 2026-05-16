using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PreUpdate0
{
    public class MovingPlatformRigidbody : MonoBehaviour
    {
        [SerializeField] private Transform transformA;
        [SerializeField] private Transform transformB;
        [SerializeField] private float duration = 3;
        [SerializeField] private float delay = 0;
        [SerializeField] private EasingStyle easingStyle;
        [SerializeField] private EasingDirection easingDirection = EasingDirection.InOut;

        private Func<float, float> easingFunction;
        private Rigidbody rb;

        [NonSerialized] public Vector3 position;
        [NonSerialized] public Quaternion rotation;

        // Start is called before the first frame update
        void Start()
        {
            easingFunction = Eases.GetFunc(easingStyle, easingDirection);
            rb = GetComponent<Rigidbody>();
            ManualUpdate();
        }

        // Update is called once per frame
        public void ManualUpdate()
        {
            float t = Math.Abs((Time.time + delay) % (duration * 2) / duration - 1);

            float b = easingFunction(t);

            position = Vector3.Lerp(transformA.position, transformB.position, b);
            rotation = Quaternion.Lerp(transformA.rotation, transformB.rotation, b);

            rb.MovePosition(position);
            rb.MoveRotation(rotation);
        }
    }
}
