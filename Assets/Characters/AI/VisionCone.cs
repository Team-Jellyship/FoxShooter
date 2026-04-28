using System;
using System.Collections.Generic;
using FoxShooter.Game;
using FoxShooter.Scripts;
using Unity.Behavior;
using UnityEngine;

namespace FoxShooter.Characters.AI
{
    [Serializable]
    public struct PerceptionEvent
    {
        public GameObject seenObject;
    }

    public class VisionCone : MonoBehaviour
    {
        /**
         * The distance at which an enemy is no longer visible
         */
        [SerializeField] [Min(0.0f)] private float coneRadius = 1.0f;

        /**
         * Half-arc length of the vision cone, in degrees - a full circle is 180 degrees, etc.
         */
        [SerializeField] [Range(0.0f, 180.0f)] private float coneHalfAngle = 30.0f;

        /**
         * The radius of the minimum visibility circle. Anything inside this radius will be detected, as long as
         * it passes the visibility raycast.
         */
        [SerializeField] [Min(0.0f)] private float minimumVisibilityRadius = 0.5f;

        /**
         * Contact filter to use for visibility raycast
         */
        [SerializeField] private ContactFilter2D visibilityContactFilter;

        /**
         * Agent to write visibility changed events to directly
         */
        [SerializeField] private BehaviorGraphAgent agent;

        /**
         * Length of time, in seconds, it takes for an enemy to be forgotten after it leaves the visible area,
         * or is otherwise obscured
         */
        [SerializeField] [Range(0.0f, 100.0f)] private float lostSightTime;
        
        private BlackboardVariable<SpottedEnemy> _seenEnemyEventChannel;
        private BlackboardVariable<SpottedEnemy> _lostSightEventChannel;

        private readonly HashSet<PerceptionSource> _seenCharacters = new();

        private TimerHandle _loseSightTimer;


#if UNITY_EDITOR
        private Mesh _coneMesh;
        private float _previousConeHalfAngle;
#endif

        private void Start()
        {
            agent.GetVariable("SeenEnemy", out _seenEnemyEventChannel);
            agent.GetVariable("LostSight", out _lostSightEventChannel);
        }

        private void FixedUpdate()
        {
            foreach (var perceptionSource in PerceptionSubsystem.instance.GetSources())
            {
                if (CanDetect(perceptionSource.transform.position))
                {
                    if (!_seenCharacters.Add(perceptionSource))
                    {
                        continue;
                    }
                    _seenEnemyEventChannel.Value.SendEventMessage(perceptionSource.gameObject, perceptionSource.transform);
                    // _loseSightTimer.Pause();
                    continue;
                }
                
                if (_seenCharacters.Remove(perceptionSource))
                {
                    // _lostSightEventChannel.Value.SendEventMessage(perceptionSource.gameObject, perceptionSource.transform);
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Compare our cached cone half angle values
            if (_previousConeHalfAngle.Equals(coneHalfAngle))
            {
                return;
            }

            _previousConeHalfAngle = coneHalfAngle;
            RegenerateConeMesh();
        }

        private void RegenerateConeMesh()
        {
            // Lazily initialize mesh
            if (!_coneMesh)
            {
                _coneMesh = new Mesh();
            }


            var coneHalfAngleRads = coneHalfAngle * Mathf.Deg2Rad;

            // Clear arrays in a set order, so we don't get errors for having out of index vertices or too many normals
            _coneMesh.triangles = new int[] { };
            _coneMesh.normals = new Vector3[] { };
            _coneMesh.vertices = new Vector3[] { };

            // Insert first two vertices, origin and 'top'
            List<Vector3> vertices = new() { Vector3.zero, new Vector3(Mathf.Cos(-coneHalfAngleRads), 0.0f, Mathf.Sin(-coneHalfAngleRads)) };
            List<Vector3> normals = new() { Vector3.forward, Vector3.forward };
            List<int> triangles = new();

            // 32 Vertices for a full circle
            const float angleInterval = Mathf.PI / 16.0f;
            var i = 1;
            for (var angle = -coneHalfAngleRads; angle < coneHalfAngleRads; angle += angleInterval)
            {
                vertices.Add(new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle)));
                normals.Add(Vector3.forward);
                triangles.AddRange(new[] { i + 1, i, 0 });
                ++i;
            }

            vertices.Add(new Vector3(Mathf.Cos(coneHalfAngleRads), 0.0f, Mathf.Sin(coneHalfAngleRads)));
            normals.Add(Vector3.forward);
            triangles.AddRange(new[] { i + 1, i, 0 });

            _coneMesh.vertices = vertices.ToArray();
            _coneMesh.triangles = triangles.ToArray();
            _coneMesh.normals = normals.ToArray();
        }

        private void OnDrawGizmos()
        {
            var forward = transform.forward.To2D();
            var currentAngle = MathF.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
            var upperAngle = (currentAngle + coneHalfAngle) * Mathf.Deg2Rad;
            var lowerAngle = (currentAngle - coneHalfAngle) * Mathf.Deg2Rad;
            Debug.DrawRay(transform.position, new Vector3(Mathf.Cos(upperAngle), 0.0f, Mathf.Sin(upperAngle)) * coneRadius, Color.red);
            Debug.DrawRay(transform.position, new Vector3(Mathf.Cos(lowerAngle), 0.0f, Mathf.Sin(lowerAngle)) * coneRadius, Color.red);

            if (!_coneMesh)
            {
                return;
            }

            Gizmos.color = _seenCharacters.Count > 0 ? new Color(0.0f, 1.0f, 0.0f, 0.5f) : new Color(1.0f, 0.0f, 0.0f, 0.5f);
            Gizmos.DrawMesh(_coneMesh, transform.position, Quaternion.Euler(new Vector3(0.0f, -currentAngle, 0.0f)), Vector3.one * coneRadius);
            StarDebug.DrawCircle(transform.position, Quaternion.FromToRotation(Vector3.forward, Vector3.down), minimumVisibilityRadius,
                new Color(1.0f, 0.0f, 0.0f, 0.4f));
        }
#endif

        private bool IsPointInside(Vector3 point)
        {
            var delta = (point - transform.position).To2D();
            var distSquared = delta.sqrMagnitude;

            if (distSquared > coneRadius * coneRadius)
            {
                return false;
            }

            // This should almost never happen, but it'll catch any divide by zeros
            if (distSquared == 0.0f)
            {
                return true;
            }

            // Catch anything that's inside the visible radius, and don't bother with the cone angle checks
            if (distSquared < minimumVisibilityRadius * minimumVisibilityRadius)
            {
                return true;
            }

            var forward = transform.forward.To2D();
            var deltaVector = delta / Mathf.Sqrt(distSquared);

            var cosBetween = Vector2.Dot(deltaVector, forward);
            var coneHalfAngleCos = Mathf.Cos(coneHalfAngle * Mathf.Deg2Rad);
            return cosBetween > coneHalfAngleCos;
        }

        private bool CanSeePoint(Vector3 point)
        {
            return !Physics.Linecast(transform.position, point, out var result, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
        }

        private bool CanDetect(Vector3 point)
        {
            return IsPointInside(point) && CanSeePoint(point);
        }
    }
}