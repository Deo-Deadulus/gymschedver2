using System;
namespace gymschedver2
{

    public class WorkoutSplit
    {
        public string WorkoutName { get; set; }
        public string WorkoutDescription { get; set; }
        public List<WorkoutDay> Days { get; set; } = new List<WorkoutDay>();
    }

    public class WorkoutDay
    {
        public string DayName { get; set; }
        public List<PrescribedExercise> Exercises { get; set; } = new List<PrescribedExercise>();
    }

    public class PrescribedExercise
    {
        public string Exercise { get; set; }
        public int Sets { get; set; }
        public string Reps { get; set; }
    }

}