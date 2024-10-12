using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

public class Init : MonoBehaviour
{
    // Função assíncrona para inicializar os serviços Unity
    async void Start()
    {
        await InitializeUnityAuthentication();
    }

    private async Task InitializeUnityAuthentication()
    {
        try
        {
            // Inicializar Unity Services
            await UnityServices.InitializeAsync();

            // Checar se o jogador já está logado
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                // Sign-in Anônimo
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Player signed in anonymously as: {AuthenticationService.Instance.PlayerId}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to initialize Authentication: {ex.Message}");
        }
    }
}