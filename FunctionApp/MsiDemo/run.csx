#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static async Task<string> Run(string input, TraceWriter log)
{
    string token = await GetToken("https://vault.azure.net", "2017-09-01");
    string secret = await GetSecret(input, token, "2016-10-01");
    return secret;
}

// Interacting with MSI
public static HttpClient tokenClient = new HttpClient();
public static async Task<string> GetToken(string resource, string apiversion)  {
    string endpoint = String.Format("{0}/?resource={1}&api-version={2}", 
        Environment.GetEnvironmentVariable("MSI_ENDPOINT"),
        resource,
        apiversion
    );
    tokenClient.DefaultRequestHeaders.Add("Secret", Environment.GetEnvironmentVariable("MSI_SECRET"));
    JObject tokenServiceResponse = JsonConvert.DeserializeObject<JObject>(await tokenClient.GetStringAsync(endpoint));
    return tokenServiceResponse["access_token"].ToString();
}

// Interacting with Key Vault
public static HttpClient keyVaultClient = new HttpClient();
public static async Task<string> GetSecret(string secretName, string token, string apiVersion) {
    string endpoint = String.Format("{0}secrets/{1}?api-version={2}",
        Environment.GetEnvironmentVariable("KEYVAULT_URL"),
        secretName,
        apiVersion
        );
    keyVaultClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    JObject keyVaultResponse = JsonConvert.DeserializeObject<JObject>(await keyVaultClient.GetStringAsync(endpoint));
    return keyVaultResponse["value"].ToString(); 
}
