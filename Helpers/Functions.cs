using System;
using System.Collections.Generic;
using System.Linq;
using Triangles.Models;

namespace Triangles.Helpers
{
    public static class Functions
    {
        private static Triangle t;

        public static void Calculate(Triangle triangle)
        {
            t = triangle;

            if (!IsValid())
            {
                t.Classification = Classification.Invalid;
                t.Degree1 = 0;
                t.Degree2 = 0;
                t.Degree3 = 0;
            }
            else
            {
                SetClassification();
                SetDegrees();
            }

            SetClassificationToString();
            SetDegreesToString();
        }

        private static bool IsValid()
        {
            if (t.SideA == 0 || t.SideB == 0 || t.SideC == 0)
                return false;

            // Make sure no two sides are less than or equal to the other side
            if ((t.SideA + t.SideB) <= t.SideC)
                return false;
            if ((t.SideA + t.SideC) <= t.SideB)
                return false;
            if ((t.SideB + t.SideC) <= t.SideA)
                return false;

            return true;
        }

        private static void SetClassification()
        {
            // first, check for all sides equal
            if (t.SideA == t.SideB && t.SideB == t.SideC && t.SideA == t.SideC)
            {
                t.Classification = Classification.Equilateral | Classification.Acute;
            }
            // check for two sides equal
            else if (t.SideA == t.SideB || t.SideB == t.SideC || t.SideA == t.SideC)
            {
                // two sides are equal. now see what other classification it has
                t.Classification = Classification.Isosceles;
                UsePythagoreanTheorem();
            }
            // scalene. now see what other classification it has
            else
            {
                t.Classification = Classification.Scalene;
                UsePythagoreanTheorem();
            }
        }

        private static void UsePythagoreanTheorem()
        {
            var sides = new List<int>() { t.SideA, t.SideB, t.SideC };
            sides.Sort();

            int smallSide1 = sides.First();
            int smallSide2 = sides.ElementAt(1);
            var largestSide = sides.Last();

            if (Squared(smallSide1) + Squared(smallSide2) == Squared(largestSide))
                t.Classification |= Classification.Right;
            else if (Squared(smallSide1) + Squared(smallSide2) > Squared(largestSide))
                t.Classification |= Classification.Acute;
            else
                t.Classification |= Classification.Obtuse;
        }

        private static void SetDegrees()
        {
            /* Now that CosC is no longer computed, I'm not sure if these two lines merit a refactor. Yes, they both share a common 
               set of math instructions, however the refactored method would look similar but with line (94) under an IF and line (95) 
               under under an ELSE, using the passed in variables. 
            */
            var cosA = (Squared(t.SideB) + Squared(t.SideC) - Squared(t.SideA)) / (2 * t.SideB * t.SideC);
            var cosB = (Squared(t.SideA) + Squared(t.SideC) - Squared(t.SideB)) / (2 * t.SideA * t.SideC);

            t.Degree1 = Math.Acos(cosA);
            t.Degree2 = Math.Acos(cosB);

            t.Degree1 = (t.Degree1 * 180) / Math.PI;
            t.Degree2 = (t.Degree2 * 180) / Math.PI;

            t.Degree1 = Math.Round(t.Degree1, 2, MidpointRounding.AwayFromZero);
            t.Degree2 = Math.Round(t.Degree2, 2, MidpointRounding.AwayFromZero);
            t.Degree3 = Math.Round(180.00D - (t.Degree1 + t.Degree2), 2, MidpointRounding.AwayFromZero);
        }

        private static void SetClassificationToString()
        {
            if (AnySideBlank())
                t.ClassificationToString = string.Empty;
            else
            {
                t.ClassificationToString = t.Classification == Classification.Invalid
                    ? "Invalid triangle"
                    : string.Format("Valid {0} triangle", t.Classification.ToString());
            }
        }

        private static void SetDegreesToString()
        {
            t.DegreesToString = AnySideBlank() ? string.Empty : string.Format("{0}   {1}   {2}", t.Degree1, t.Degree2, t.Degree3);
        }

        private static double Squared(double side)
        {
            return Math.Pow(side, 2);
        }

        private static bool AnySideBlank()
        {
            if (t.SideAText == string.Empty)
                return true;
            if (t.SideBText == string.Empty)
                return true;
            if (t.SideCText == string.Empty)
                return true;

            return false;
        }
    }
}
