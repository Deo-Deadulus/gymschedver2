using System;

namespace gymschedver2
{

    public class UserAuth : User
    {

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalted { get; set; }
        public double HeightCm { get; set; }
        public double WeightKg { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public double BmiScore { get; set; }
        public string Category { get; set; }
        public string WorkoutIntensity { get; set; }
        public List<ScheduledWorkout> Schedule { get; set; } = new List<ScheduledWorkout>();
        public UserAuth() : base("") { }
        public UserAuth(string name, string username) : base(name)
        {

            Username = username;

        }
    }
    public abstract class User
    {
        public string Name { get; set; }
        public User(string name)
        {
            Name = name;
        }
    }
    public class ScheduledWorkout
    {
        public DateTime Date { get; set; }
        public string SplitName { get; set; }
        public bool IsCompleted { get; set; }
        public string WorkoutDayName { get; set; }
    }

}