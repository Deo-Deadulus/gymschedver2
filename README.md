GymSchedVer2/
├── Program.cs           # Entry point & Data Loader
├── Login.cs             # User Dashboard Logic
├── SignUp.cs            # Registration Logic
├── Admin.cs             # Admin Dashboard Logic
├── Models/
│   ├── UserAuth.cs      # User Data Model
│   ├── WorkoutSplit.cs  # Workout Structure
│   ├── Exercise.cs      # Exercise Data Model
│   └── AdminAuth.cs     # Admin Config Model
├── Utilities/
│   ├── HIdeP.cs         # Password Hashing & Security
│   ├── CalenSelector.cs # Calendar UI Component
│   └── FitnessProfileCalculator.cs # BMI Logic
└── Data/                # JSON Data Stores
    ├── users.json
    ├── allSPLITS.json
    ├── admin.json
    ├── arms.json
    └── ...
