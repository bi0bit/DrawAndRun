using System;
using System.Linq;
using Dreamteck.Splines;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public static class CurveUtils
    {
        public static SplinePoint[] Simplify(SplinePoint[] targetCurve, float e)
        {
            SplinePoint[] resultCurve;
            
            //find point with max distance
            float distanceMax = 0f;
            int indexMax = 0;
            for (var i = 1; i < targetCurve.Length; i++)
            {
                var distance = HandleUtility.DistancePointLine(targetCurve[i].position, targetCurve[0].position,
                    targetCurve[targetCurve.Length - 1].position);
                if (distance > distanceMax)
                {
                    indexMax = i;
                    distanceMax = distance;
                }
            }
            
            //if greater than e simplify this
            if (distanceMax > e)
            {
                var recResult1 = Simplify(targetCurve.Take(indexMax).ToArray(), e);
                var recResult2 = Simplify(targetCurve.Skip(indexMax).ToArray(), e);

                resultCurve = recResult1.Take(recResult1.Length - 1).Concat(recResult2).ToArray();
            }
            else
            {
                resultCurve = targetCurve.SubArray(0, targetCurve.Length);
            }

            return resultCurve;
        }

        public static SplinePoint[] ToSimplify(this SplinePoint[] targetCurve, float e)
        {
            return Simplify(targetCurve, e);
        }
        
    }
}