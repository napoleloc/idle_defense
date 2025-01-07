using System;
using EncosyTower.Modules.Collections;
using UnityEngine;

namespace Module.Core.KCC
{
    /// <summary>
    /// The system that manages the simulation of KinematicCharacterMotor and PhysicsMover
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class KinematicCharacterSystem : MonoBehaviour
    {
        private static KinematicCharacterSystem s_instance;

        public static FasterList<KinematicCharacterComponent> CharacterComponents = new FasterList<KinematicCharacterComponent>();
        public static FasterList<PhysicsMover> PhysicsMovers = new FasterList<PhysicsMover>();

        private static float s_lastCustomInterpolationStartTime = -1f;
        private static float s_lastCustomInterpolationDeltaTime = -1f;

        public static KCCSettings Settings;

        /// <summary>
        /// Creates a KinematicCharacterSystem instance if there isn't already one
        /// </summary>
        public static void EnsureCreation()
        {
            if (s_instance == null)
            {
                GameObject systemGameObject = new GameObject("KinematicCharacterSystem");
                s_instance = systemGameObject.AddComponent<KinematicCharacterSystem>();

                systemGameObject.hideFlags = HideFlags.NotEditable;
                s_instance.hideFlags = HideFlags.NotEditable;

                Settings = ScriptableObject.CreateInstance<KCCSettings>();

                GameObject.DontDestroyOnLoad(systemGameObject);
            }
        }

        /// <summary>
        /// Gets the KinematicCharacterSystem instance if any
        /// </summary>
        /// <returns></returns>
        public static KinematicCharacterSystem GetInstance()
        {
            return s_instance;
        }

        /// <summary>
        /// Sets the maximum capacity of the character motors list, to prevent allocations when adding characters
        /// </summary>
        /// <param name="capacity"></param>
        public static void SetCharacterMotorsCapacity(int capacity)
        {
            if (capacity < CharacterComponents.Count)
            {
                capacity = CharacterComponents.Count;
            }
            CharacterComponents.IncreaseCapacityTo(capacity);
        }

        /// <summary>
        /// Registers a KinematicCharacterMotor into the system
        /// </summary>
        public static void RegisterCharacterMotor(KinematicCharacterComponent component)
        {
            CharacterComponents.Add(component);
        }

        /// <summary>
        /// Unregisters a KinematicCharacterMotor from the system
        /// </summary>
        public static void UnregisterCharacterMotor(KinematicCharacterComponent component)
        {
            CharacterComponents.Remove(component);
        }

        /// <summary>
        /// Sets the maximum capacity of the physics movers list, to prevent allocations when adding movers
        /// </summary>
        /// <param name="capacity"></param>
        public static void SetPhysicsMoversCapacity(int capacity)
        {
            if (capacity < PhysicsMovers.Count)
            {
                capacity = PhysicsMovers.Count;
            }
            PhysicsMovers.IncreaseCapacityTo(capacity);
        }

        /// <summary>
        /// Registers a PhysicsMover into the system
        /// </summary>
        public static void RegisterPhysicsMover(PhysicsMover mover)
        {
            PhysicsMovers.Add(mover);

            mover.Rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        /// <summary>
        /// Unregisters a PhysicsMover from the system
        /// </summary>
        public static void UnregisterPhysicsMover(PhysicsMover mover)
        {
            PhysicsMovers.Remove(mover);
        }

        // This is to prevent duplicating the singleton gameobject on script recompiles
        private void OnDisable()
        {
            Destroy(this.gameObject);
        }

        private void Awake()
        {
            s_instance = this;
        }

        private void FixedUpdate()
        {
            if (Settings.autoSimulation)
            {
                float deltaTime = Time.deltaTime;

                if (Settings.interpolate)
                {
                    PreSimulationInterpolationUpdate(deltaTime);
                }

                Simulate(deltaTime, CharacterComponents.AsSpan(), PhysicsMovers.AsSpan());

                if (Settings.interpolate)
                {
                    PostSimulationInterpolationUpdate(deltaTime);
                }
            }
        }

        private void LateUpdate()
        {
            if (Settings.interpolate)
            {
                CustomInterpolationUpdate();
            }
        }

        /// <summary>
        /// Remembers the point to interpolate from for KinematicCharacterComponents and PhysicsMovers
        /// </summary>
        public static void PreSimulationInterpolationUpdate(float deltaTime)
        {
            // Save pre-simulation poses and place transform at transient pose
            for (int i = 0; i < CharacterComponents.Count; i++)
            {
                KinematicCharacterComponent motor = CharacterComponents[i];

                motor.initialTickPosition = motor.TransientPosition;
                motor.initialTickRotation = motor.TransientRotation;

                motor.Transform.SetPositionAndRotation(motor.TransientPosition, motor.TransientRotation);
            }

            for (int i = 0; i < PhysicsMovers.Count; i++)
            {
                PhysicsMover mover = PhysicsMovers[i];

                mover.InitialTickPosition = mover.TransientPosition;
                mover.InitialTickRotation = mover.TransientRotation;

                mover.Transform.SetPositionAndRotation(mover.TransientPosition, mover.TransientRotation);
                mover.Rigidbody.position = mover.TransientPosition;
                mover.Rigidbody.rotation = mover.TransientRotation;
            }
        }

        /// <summary>
        /// Ticks characters and/or movers
        /// </summary>
        public static void Simulate(float deltaTime, Span<KinematicCharacterComponent> motors, Span<PhysicsMover> movers)
        {
            int characterComponentsLenght = motors.Length;
            int physicsMoversLength = movers.Length;

#pragma warning disable 0162
            // Update PhysicsMover velocities
            for (int i = 0; i < physicsMoversLength; i++)
            {
                movers[i].VelocityUpdate(deltaTime);
            }

            // Character controller update phase 1
            for (int i = 0; i < characterComponentsLenght; i++)
            {
                motors[i].UpdatePhase1(deltaTime);
            }

            // Simulate PhysicsMover displacement
            for (int i = 0; i < physicsMoversLength; i++)
            {
                PhysicsMover mover = movers[i];

                mover.Transform.SetPositionAndRotation(mover.TransientPosition, mover.TransientRotation);
                mover.Rigidbody.position = mover.TransientPosition;
                mover.Rigidbody.rotation = mover.TransientRotation;
            }

            // Character controller update phase 2 and move
            for (int i = 0; i < characterComponentsLenght; i++)
            {
                KinematicCharacterComponent motor = motors[i];

                motor.UpdatePhase2(deltaTime);

                motor.Transform.SetPositionAndRotation(motor.TransientPosition, motor.TransientRotation);
            }
#pragma warning restore 0162
        }

        /// <summary>
        /// Initiates the interpolation for KinematicCharacterMotors and PhysicsMovers
        /// </summary>
        public static void PostSimulationInterpolationUpdate(float deltaTime)
        {
            s_lastCustomInterpolationStartTime = Time.time;
            s_lastCustomInterpolationDeltaTime = deltaTime;

            // Return interpolated roots to their initial poses
            for (int i = 0; i < CharacterComponents.Count; i++)
            {
                KinematicCharacterComponent motor = CharacterComponents[i];

                motor.Transform.SetPositionAndRotation(motor.initialTickPosition, motor.initialTickRotation);
            }

            for (int i = 0; i < PhysicsMovers.Count; i++)
            {
                PhysicsMover mover = PhysicsMovers[i];

                if (mover.MoveWithPhysics)
                {
                    mover.Rigidbody.position = mover.InitialTickPosition;
                    mover.Rigidbody.rotation = mover.InitialTickRotation;

                    mover.Rigidbody.MovePosition(mover.TransientPosition);
                    mover.Rigidbody.MoveRotation(mover.TransientRotation);
                }
                else
                {
                    mover.Rigidbody.position = (mover.TransientPosition);
                    mover.Rigidbody.rotation = (mover.TransientRotation);
                }
            }
        }

        /// <summary>
        /// Handles per-frame interpolation
        /// </summary>
        private static void CustomInterpolationUpdate()
        {
            float interpolationFactor = Mathf.Clamp01((Time.time - s_lastCustomInterpolationStartTime) / s_lastCustomInterpolationDeltaTime);

            // Handle characters interpolation
            for (int i = 0; i < CharacterComponents.Count; i++)
            {
                KinematicCharacterComponent motor = CharacterComponents[i];

                motor.Transform.SetPositionAndRotation(
                    Vector3.Lerp(motor.initialTickPosition, motor.TransientPosition, interpolationFactor),
                    Quaternion.Slerp(motor.initialTickRotation, motor.TransientRotation, interpolationFactor));
            }

            // Handle PhysicsMovers interpolation
            for (int i = 0; i < PhysicsMovers.Count; i++)
            {
                PhysicsMover mover = PhysicsMovers[i];

                mover.Transform.SetPositionAndRotation(
                    Vector3.Lerp(mover.InitialTickPosition, mover.TransientPosition, interpolationFactor),
                    Quaternion.Slerp(mover.InitialTickRotation, mover.TransientRotation, interpolationFactor));

                Vector3 newPos = mover.Transform.position;
                Quaternion newRot = mover.Transform.rotation;
                mover.PositionDeltaFromInterpolation = newPos - mover.LatestInterpolationPosition;
                mover.RotationDeltaFromInterpolation = Quaternion.Inverse(mover.LatestInterpolationRotation) * newRot;
                mover.LatestInterpolationPosition = newPos;
                mover.LatestInterpolationRotation = newRot;
            }
        }
    }
}
