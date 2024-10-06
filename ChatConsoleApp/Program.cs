// See https://aka.ms/new-console-template for more information

using ChatConsoleApp.classes;

await Task.Delay(5000);
Console.WriteLine("Fetching users");
string users = await CustomApiClient.CallGetApiJsonResult("/Users");

Console.WriteLine("Users:");
Console.WriteLine(users);

Console.ReadLine();
