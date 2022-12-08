using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System;

public class FirebaseManager : MonoBehaviour
{
        List<List<Dictionary<string,string>>> teamQueue = null;
        List<Dictionary<string,string>> team = null;
        Dictionary<string, string> teamInfoDict = null;
        [SerializeField] bool isSaveRunning = false;

        private void Update()
        {
            if (!isSaveRunning && teamQueue.Count > 0)
            {
                StartCoroutine(SaveDataCoroutine());
            }   

        }

        public void SaveTeamData(Character[] teamArray)
        {
            team = new List<Dictionary<string, string>>();
            for (int i = 0; i < teamArray.Length; i++)
            {
                if (teamArray[i] == null)
                {
                    continue;
                }
                teamInfoDict = new Dictionary<string, string>();
                
                teamInfoDict.Add("CharacterClass", teamArray[i].characterClass);
                teamInfoDict.Add("Position", i.ToString());
                teamInfoDict.Add("Health", teamArray[i].health.ToString());
                teamInfoDict.Add("Damage", teamArray[i].damage.ToString());
                if (teamArray[i].equippedItem != null)
                {
                    teamInfoDict.Add("Item", teamArray[i].equippedItem.itemClass);
                }
                else 
                {
                    teamInfoDict.Add("Item", "");
                }
                team.Add(teamInfoDict);
            }
            teamQueue.Add(team);
            Debug.Log("Team queue is at: " + teamQueue.Count);
            Debug.Log("Number of team members is at: " + team.Count);
        }




    string teamName = "sheesh";
    LevelController levelController;
    [SerializeField] TeamBuilder teamBuilder;
    [SerializeField] string emailLoginName;
    [SerializeField] string usernameLoginName;
    [SerializeField] int currentInt;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;


    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [Header("UserData")]
    public TMP_InputField usernameField;
    public TMP_InputField xpField;
    public TMP_InputField killsField;
    public TMP_InputField deathsField;
    public GameObject scoreElement;
    public Transform scoreboardContent;

    // public void ClearLoginFeilds()
    // {
    //     emailLoginField.text = "";
    //     passwordLoginField.text = "";
    // }
    // public void ClearRegisterFeilds()
    // {
    //     usernameRegisterField.text = "";
    //     emailRegisterField.text = "";
    //     passwordRegisterField.text = "";
    //     passwordRegisterVerifyField.text = "";
    // }

    public void SignOutButton()
    {
        auth.SignOut();
        // UIManager.instance.LoginScreen();
        // ClearRegisterFeilds();
        // ClearLoginFeilds();
    }

    private void Start()
    {
        TeamBuilder.onTeamBuilderStart += RegisterTeamBuilder;
        teamQueue = new List<List<Dictionary<string,string>>>();
    }

    private void OnDisable()
    {
        TeamBuilder.onTeamBuilderStart -= RegisterTeamBuilder;
    }

    private void RegisterTeamBuilder(TeamBuilder tb)
    {
        teamBuilder = tb;
    }

    void Awake()
    {
        levelController = GetComponent<LevelController>();
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase initialized");
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            // StartCoroutine(LoadUserData());

            yield return new WaitForSeconds(2);

            // usernameField.text = User.DisplayName;
            SceneControl.NextScene("Start Scene");
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if(passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else 
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        // UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                    }
                }
            }
        }
    }

    private IEnumerator SaveDataCoroutine()
    {
        isSaveRunning = true;
        yield return isSaveRunning = true;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        var DBTask = DBreference.Child("Round " + levelController.round.ToString()).GetValueAsync();         
        
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        DataSnapshot snapshot = DBTask.Result;
        int count = Convert.ToInt32(snapshot.ChildrenCount);
        // int randomlySelectedOpponent = UnityEngine.Random.Range(1, count);
        int newIndex = count + 1;
        Debug.Log("New index is " + newIndex);

        for (int i = 0; i < teamQueue[0].Count; i++)
        {
            teamQueue[0][i].TryGetValue("CharacterClass", out string characterClass);
            teamQueue[0][i].TryGetValue("Health", out string health);
            teamQueue[0][i].TryGetValue("Damage", out string damage);
            teamQueue[0][i].TryGetValue("Position", out string teamPosition);
            teamQueue[0][i].TryGetValue("Item", out string item);

            int teamMemberIndex = Int32.Parse(teamPosition);

            // StartCoroutine(UpdateUsernameAuth(emailLoginName, newIndex));
            StartCoroutine(UpdateUsernameDatabase(usernameLoginName, newIndex, teamPosition));
            StartCoroutine(UpdateTeamPosition(teamPosition, newIndex, teamPosition));
            StartCoroutine(UpdateCharacterClass(characterClass, newIndex, teamPosition));
            StartCoroutine(UpdateHealth(health, newIndex, teamPosition));
            StartCoroutine(UpdateDamage(damage, newIndex, teamPosition));
            StartCoroutine(UpdateEquippedItem(item, newIndex, teamPosition));
        }
        Debug.Log("Done saving");
        teamQueue.RemoveAt(0);
        isSaveRunning = false;
    }


    private IEnumerator UpdateUsernameAuth(string _username, int randomInt, string position)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }        
    }

    private IEnumerator UpdateUsernameDatabase(string _username, int index, string position)
    {
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("Round " + levelController.round.ToString()).Child("Team " + index.ToString()).Child("Team Member " + position).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }


    private IEnumerator UpdateTeamPosition(string teamPosition, int index, string position)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("Round " + levelController.round.ToString()).Child("Team " +index.ToString()).Child("Team Member " + position).Child("Position").SetValueAsync(teamPosition);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

        private IEnumerator UpdateCharacterClass(string characterClass, int index, string position)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("Round " + levelController.round.ToString()).Child("Team " + index.ToString()).Child("Team Member " + position).Child("CharacterClass").SetValueAsync(characterClass);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdateHealth(string health, int index, string position)
    {
        //Set the currently logged in user kills
        var DBTask = DBreference.Child("Round " + levelController.round.ToString()).Child("Team " + index.ToString()).Child("Team Member " + position).Child("Health").SetValueAsync(health);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Kills are now updated
        }
    }

    private IEnumerator UpdateDamage(string damage, int index, string position)
    {
        //Set the currently logged in user deaths
        var DBTask = DBreference.Child("Round " + levelController.round.ToString()).Child("Team " + index.ToString()).Child("Team Member " + position).Child("Damage").SetValueAsync(damage);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Deaths are now updated
        }
    }

        private IEnumerator UpdateEquippedItem(string equippedItem, int index, string position)
    {
        //Set the currently logged in user deaths
        var DBTask = DBreference.Child("Round " + levelController.round.ToString()).Child("Team " + index.ToString()).Child("Team Member " + position).Child("EquippedItem").SetValueAsync(equippedItem);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Deaths are now updated
        }
    }

    public void StartLoadUserData()
    {
        StartCoroutine(LoadUserData());
    }

    private IEnumerator LoadUserData()
    {
        //Get the currently logged in user data
        var DBTask = DBreference.Child("Round " + levelController.round.ToString()).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);





        // Debug.Log(DBTask[0]);

        // for (DataSnapshot child in DBTask)
        // Child(User.DisplayName))

        if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
        else if (DBTask.Result.Value == null)
            {
                Debug.Log("It's null bro");
                // Debug.Log(snapshot.Child("0").Child("CharacterClass").Value.ToString());
                // //No data exists yet
                // xpField.text = "0";
                // killsField.text = "0";
                // deathsField.text = "0";
            // string[] teamMemberStrings = teamQueryResults.Split('\n');
            // teamMemberStrings = teamMemberStrings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            // Debug.Log("Team member strings length = " + teamMemberStrings.Length);

            // foreach (var member  in teamMemberStrings)
            // {
            //     Debug.Log(member);
            //     var memberStatStrings = member.Split(',');
            //     teamBuidler.InstantiateEnemyTeamForBattle(Int32.Parse(memberStatStrings[0]), memberStatStrings[1], Int32.Parse(memberStatStrings[2]), Int32.Parse(memberStatStrings[3]), memberStatStrings[4]);
            // }
            }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            int count = Convert.ToInt32(snapshot.ChildrenCount);
            int randomlySelectedOpponent = UnityEngine.Random.Range(1, count + 1);
            Debug.Log("Count is " + count);
            Debug.Log("Random selection is " + randomlySelectedOpponent);
            Debug.Log(snapshot);

            foreach (DataSnapshot kid in snapshot.Child("Team " +randomlySelectedOpponent.ToString()).Children)
            {
                string characterClass = kid.Child("CharacterClass").Value.ToString();
                int position = Int32.Parse(kid.Child("Position").Value.ToString());
                int health = Int32.Parse(kid.Child("Health").Value.ToString());
                int damage = Int32.Parse(kid.Child("Damage").Value.ToString());
                string equippedItem = kid.Child("EquippedItem").Value.ToString();

                teamBuilder.InstantiateEnemyTeamForBattle(position, characterClass, damage, health, equippedItem);
            }
            snapshot = null;
        }
    }
}


