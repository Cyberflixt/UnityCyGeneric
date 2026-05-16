using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFootstepsDistance : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    public float stepSize = 2f;
    private float height = 2;

    private float distance = 0;
    private Vector3 position;

    void Start()
    {
        position = transform.position;
    }
    void Update()
    {
        Vector3 oldPos = position;
        position = transform.position;

        // Is on the ground?
        if (characterController.isGrounded){
            // Add moved distance
            float add = (transform.position - oldPos).magnitude;
            distance += add;

            // Bug: unity gave a funny value?
            if (distance > stepSize * 2){
                distance = stepSize * 1.5f;
            }

            // Moved distance is a full step?
            if (distance > stepSize){
                distance -= stepSize;
                
                // Get material
                string material = "Concrete"; // Fallback

                // Raycast down
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit)){
                    SoundCollider colliderAudio = hit.collider.transform.GetComponent<SoundCollider>();
                    if (colliderAudio){
                        material = colliderAudio.material.ToString();
                    }
                    
                    Sounds.PlayAudio(SoundsType.SFX, "footsteps"+material, transform.position - Vector3.down*(height/2f), .5f);
                }
                
            }
        }
    }
}
