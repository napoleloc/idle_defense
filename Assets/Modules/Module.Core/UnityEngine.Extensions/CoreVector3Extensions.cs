namespace Module.Core
{
    using System.Runtime.CompilerServices;
    using Unity.Collections;
    using UnityEngine;

    using UnityRandom = UnityEngine.Random;

    public static class CoreVector3Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null) =>
            new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null) =>
           new Vector3(vector.x + x ?? vector.x, vector.y + y ?? vector.y, vector.z + z ?? vector.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Sub(this Vector3 vector, float? x = null, float? y = null, float? z = null) =>
           new Vector3(vector.x - x ?? vector.x, vector.y - y ?? vector.y, vector.z - z ?? vector.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Mul(this Vector3 vector, float? x = null, float? y = null, float? z = null) =>
           new Vector3(vector.x * x ?? vector.x, vector.y * y ?? vector.y, vector.z * z ?? vector.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Div(this Vector3 vector, float? x = null, float? y = null, float? z = null) =>
            new Vector3(vector.x / x ?? vector.x, vector.y / y ?? vector.y, vector.z / z ?? vector.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 DirectionTo(this Vector3 source, Vector3 destination) =>
           Vector3.Normalize(destination - source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Closest(this Vector3 target, Vector3[] vectors)
        {
            Vector3 closest = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
            foreach (Vector3 vector in vectors)
            {
                float current = (vector - target).sqrMagnitude;
                if (current < (closest - target).sqrMagnitude)
                    closest = vector;
            }
            return closest;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWithin(this Vector3 vector, Vector3 origin, float radius) =>
            (vector - origin).sqrMagnitude < radius * radius;

        public static void GetPointsAround(this Vector3 center, float radius, ref NativeList<Vector3> points, int count)
        {
            if (points.IsCreated == false)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 randomDirection = UnityRandom.insideUnitSphere * radius;
                Vector3 point = center + randomDirection;

                points.Add(point);
            }
        }

        public static void GetRandomPointsOnEdge(this Vector3 center, float radius, ref NativeList<Vector3> points, int count)
        {
            if (points.IsCreated == false)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                float angle = UnityRandom.Range(0f, Mathf.PI * 2);

                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                Vector3 point = new Vector3(x, 0, z) + center;

                points.Add(point);
            }
        }
    }
}
