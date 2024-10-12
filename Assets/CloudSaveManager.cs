using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public class CloudSaveManager : MonoBehaviour
{
    // Função para salvar dados no Cloud Save
    public async Task SavePlayerData(string key, string value)
    {
        try
        {
            // Cria um dicionário com a chave e o valor
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { key, value }
            };

            // Envia o dado para o Cloud Save
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log("Data saved successfully!");
        }
        catch (CloudSaveException e)
        {
            Debug.LogError($"Failed to save data: {e.Message}");
        }
    }


    public async Task<string> LoadPlayerData(string key)
    {
        try
        {
            // Tenta recuperar os dados da chave especificada
            Dictionary<string, string> savedData =
                await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });

            if (savedData.ContainsKey(key))
            {
                Debug.Log($"Data loaded: {savedData[key]}");
                return savedData[key];
            }
            else
            {
                Debug.Log($"No data found for key: {key}");
                return null;
            }
        }
        catch (CloudSaveException e)
        {
            Debug.LogError($"Failed to load data: {e.Message}");
            return null;
        }
    }
}