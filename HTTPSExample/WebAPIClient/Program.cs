using System.Net.Http.Headers;
using System.Text.Json;

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

string[] apis = ["https://api.github.com/orgs/dotnet/repos", "https://api.github.com/orgs/CSC-3380-Class-Code/repos"];

foreach(string api in apis){
    var repositories = await ProcessRepositoriesAsync(client, api);

    foreach (var repo in repositories)
    {
        Console.WriteLine($"Name: {repo.Name}");
        Console.WriteLine($"Homepage: {repo.Homepage}");
        Console.WriteLine($"GitHub: {repo.GitHubHomeUrl}");
        Console.WriteLine($"Description: {repo.Description}");
        Console.WriteLine($"Watchers: {repo.Watchers:#,0}");
        Console.WriteLine($"{repo.LastPush}");
        Console.WriteLine();
    }
}
static async Task<List<Repository>> ProcessRepositoriesAsync(HttpClient client, string api)
{
    await using Stream stream =
        await client.GetStreamAsync(api);
    var repositories =
        await JsonSerializer.DeserializeAsync<List<Repository>>(stream);
    return repositories ?? new();
}
