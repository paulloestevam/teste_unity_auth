using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Button signinButton;
    public Button signupButton;
    public Button logoutButton;
    public Button loadDataButton;
    public Button saveDataButton;
    public Button sequenceButton;
    public Text feedbackText;

    void Start()
    {
        InitializeUnityServices();

        signinButton.onClick.AddListener(OnSignInButtonClicked);
        signupButton.onClick.AddListener(OnSignUpButtonClicked);
        logoutButton.onClick.AddListener(OnLogoutButtonClicked); // Adicionar o listener de Logout

        saveDataButton.onClick.AddListener(OnSaveDataButtonClicked);
        loadDataButton.onClick.AddListener(OnLoadDataButtonClicked);
        sequenceButton.onClick.AddListener(OnLoadSequenceButtonClicked);
    }

    async void InitializeUnityServices()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("Unity Services Initialized.");
    }

    void OnSignInButtonClicked()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Username and password must not be empty.";
            return;
        }

        SignIn(username, password);
    }

    void OnSignUpButtonClicked()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Username and password must not be empty.";
            return;
        }

        SignUp(username, password);
    }

    void OnLogoutButtonClicked()
    {
        Logout();
    }

    async void SignIn(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            feedbackText.text = "Logged in successfully!";
            Debug.Log("User signed in: " + AuthenticationService.Instance.PlayerId);
        }
        catch (AuthenticationException authException)
        {
            if (authException.Message.Contains("WRONG_USERNAME_PASSWORD"))
            {
                feedbackText.text = "Invalid username or password.";
            }
            else
            {
                feedbackText.text = $"Authentication Error: {authException.Message}";
            }

            Debug.LogError("Sign in failed: " + authException.Message);
        }
        catch (Exception ex)
        {
            feedbackText.text = $"Error: {ex.Message}";
            Debug.LogError("Error during sign-in: " + ex.Message);
        }
    }

    async void SignUp(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            feedbackText.text = "User created and signed in successfully!";
            Debug.Log("User signed up: " + AuthenticationService.Instance.PlayerId);
        }
        catch (AuthenticationException signupException)
        {
            feedbackText.text = $"Sign up failed: {signupException.Message}";
            Debug.LogError("Sign up failed: " + signupException.Message);
        }
        catch (Exception ex)
        {
            feedbackText.text = $"Error: {ex.Message}";
            Debug.LogError("Error during sign-up: " + ex.Message);
        }
    }

    void Logout()
    {
        try
        {
            AuthenticationService.Instance.SignOut();
            feedbackText.text = "Logged out successfully!";
            Debug.Log("User logged out.");
        }
        catch (Exception ex)
        {
            feedbackText.text = $"Error during logout: {ex.Message}";
            Debug.LogError("Error during logout: " + ex.Message);
        }
    }

    async void OnSaveDataButtonClicked()
    {
        CloudSaveManager cloudSaveManager = new CloudSaveManager();
        await cloudSaveManager.SavePlayerData("last_login", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        feedbackText.text = "saved 'last_login' on cloud save";
    }

    async void OnLoadDataButtonClicked()
    {
        CloudSaveManager cloudSaveManager = new CloudSaveManager();
        string playerLevel = await cloudSaveManager.LoadPlayerData("last_login");
        feedbackText.text = "last_login: " + playerLevel;
    }

    public void OnLoadSequenceButtonClicked()
    {
        Debug.Log("Carregando scene Demo do Sequence SDK");
        SceneManager.LoadScene("Demo");
    }
}