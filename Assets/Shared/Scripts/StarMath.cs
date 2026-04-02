using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FoxShooter.Scripts
{
public abstract class StarMath
{
    /**
     * <summary>Get a number value, moved towards another value.
     * Prevents overshooting the value.
     * </summary>
     * <param name="input">Value to change</param>
     * <param name="destination">Value to reach</param>
     * <param name="rate">Amount to move input towards destination. Should be positive</param>
     */
    public static float MoveTo(float input, float destination, float rate)
    {
        var delta = destination - input;
        if (delta < rate)
        {
            return destination;
        }

        return input + rate * MathF.Sign(delta);
    }
    
    public static Vector2 MoveTo(Vector2 input, Vector2 destination, float rate)
    {
        var delta = destination - input;
        var deltaSquared = delta.sqrMagnitude;
        if (deltaSquared < rate * rate)
        {
            return destination;
        }
        return input + delta.normalized * rate;
    }

    public static float ClampTowards(float input, float min, float max, float rate)
    {
        if (input > max)
        {
            var delta = input - max;
            if (delta < rate)
            {
                return max;
            }

            return input - rate;
        }
        if (input < min)
        {
            var delta = input - min;
            if (delta > -rate)
            {
                return min;
            }

            return input + rate;
        }

        return input;
    }

    public static T ChooseRandom<T>(List<T> items)
    {
        return items.Count == 0 ? default : items[Random.Range(0, items.Count - 1)];
    }

    /**
     * <summary>Helper function to get a direction vector from an angle</summary>
     * <param name="angle">Angle in degrees</param>
     */
    public static Vector2 VectorFromAngle(float angle)
    {
        var rads = Mathf.Deg2Rad * angle;
        return new Vector2(Mathf.Cos(rads), -Mathf.Sin(rads));
    }

    public static Vector2 RandomDirection(float negativeBounds = -MathF.PI, float positiveBounds = MathF.PI)
    {
        var angle = Random.Range(negativeBounds, positiveBounds);
        return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
    }

    public static Vector2 ClampVectorLength(Vector2 vector, float length)
    {
        var vectorLengthSquared = vector.sqrMagnitude;
        if (vectorLengthSquared > length * length)
        {
            return vector * (length / MathF.Sqrt(vectorLengthSquared));
        }
        return vector;
    }

    public static Vector2 ScaleVectorLength(Vector2 vector, float delta)
    {
        var length = vector.magnitude;
        var normal = vector / length;

        if (-delta > length)
        {
            return Vector2.zero;
        }

        return normal * (length + delta);
    }
    
    public static Vector2 RotateVector(Vector2 vector, float angle)
    {
        return new Vector2(
            vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle),
            vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle)
        );
    }
}
}