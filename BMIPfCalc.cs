using System;
namespace gymschedver2
{
    public static class BMIPfCalc
    {
        public static void CalculateProfile(UserAuth user)
        {
            double heightM = user.HeightCm / 100.0;
            user.BmiScore = user.WeightKg / (heightM * heightM);

            string baseIntensity;
            if (user.BmiScore < 18.5)
            {
                user.Category = "Underweight";
                baseIntensity = "Low to Moderate";
            }
            else if (user.BmiScore < 24.9)
            {
                user.Category = "Normal weight";
                baseIntensity = "Moderate to High";
            }
            else if (user.BmiScore < 29.9)
            {
                user.Category = "Overweight";
                baseIntensity = "Low to Moderate";
            }
            else
            {
                user.Category = "Obese";
                baseIntensity = "Low";
            }

            if (user.Age >= 60)
            {
                user.WorkoutIntensity = "Low (Senior Safety)";
            }
            else if (user.Age >= 50)
            {
                if (baseIntensity.Contains("High"))
                {
                    user.WorkoutIntensity = "Moderate";
                }
                else
                {
                    user.WorkoutIntensity = baseIntensity;
                }
            }
            else if (user.Age < 15)
            {
                user.WorkoutIntensity = "Low to Moderate (Junior)";
            }
            else
            {
                if (user.Gender.StartsWith("F", StringComparison.OrdinalIgnoreCase)
                    && user.Category == "Overweight"
                    && user.BmiScore < 27.0)
                {
                    user.WorkoutIntensity = "Moderate";
                }
                else
                {
                    user.WorkoutIntensity = baseIntensity;
                }
            }
        }
    }
}