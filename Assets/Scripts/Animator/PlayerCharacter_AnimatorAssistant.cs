using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    public class PlayerCharacter_AnimatorAssistant : AnimatorAssistant
    {
        CinemachineVirtualCamera _cinemachineVirtualCamera;
        CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

        [SerializeField] private NoiseSettings noiseSettings;

        private void Start()
        {
            _cinemachineVirtualCamera = Helper.GetActiveVirtualCamera().VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

            _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _cinemachineBasicMultiChannelPerlin.m_NoiseProfile = noiseSettings;
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        }

        public void SetIdleActionValueToZeroEvent()
        {
            AnimatorHelper.SetAnimatorIntParameter(Animator, "IdleActionValue", 0);
        }

        public void ResetControllerTimeSpentInIdleEvent()
        {
            transform.root.GetComponent<CharacterController>().ResetTimeSpentInIdleValue();
        }

        public void SetCameraShakeOnInteraction(float shakeIntensity)
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeIntensity;
        }

        public void ResetCameraShakeOnInteraction()
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }
}