using System.Text.Json.Nodes;

namespace Tugas_4;

public class CLIProgram
{
    private static void CreateUser()
    {
        while (true)
            try
            {
                Console.Write("First Name : ");
                var firstName = Console.ReadLine();
                Console.Write("Last Name : ");
                var lastName = Console.ReadLine();
                Console.Write("Password : ");
                var password = Console.ReadLine();

                var createdUser = UserService.CreateUser(firstName, lastName, password);
                Console.WriteLine($"[✓] User successfully created with username \"{createdUser.Username}\"");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }

    private static void EditUser()
    {
        while (true)
            try
            {
                Console.Write("User Id to be edited : ");
                var id = Console.ReadLine();
                Console.Write("New First Name : ");
                var newFirstName = Console.ReadLine();
                Console.Write("New Last Name : ");
                var newLastName = Console.ReadLine();
                Console.Write("New Password : ");
                var newPassword = Console.ReadLine();

                var userId = Convert.ToUInt32(id);

                UserService.UpdateUser(userId, newFirstName, newLastName, newPassword);

                Console.WriteLine($"[✓] User successfully updated");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            catch (FormatException)
            {
                Console.WriteLine("invalid id");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }

    private static void DeleteUser()
    {
        while (true)
            try
            {
                Console.Write("User Id to be deleted : ");
                var userId = Convert.ToUInt32(Console.ReadLine());

                var deletedUser = UserService.DeleteUser(userId);

                Console.WriteLine(
                    $"[✓] User with id {deletedUser.Id} & username {deletedUser.Username} has successfully deleted");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            catch (FormatException)
            {
                Console.WriteLine("invalid id");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }

    private static void ShowUsers()
    {
        Console.WriteLine("== SHOW USERS ==");
        var foundUsers = UserService.ListUsers();
        foundUsers.ForEach(Console.WriteLine);

        MenuHandler(new List<(int, string, Action)>
        {
            (1, "Edit User", EditUser),
            (2, "Delete User", DeleteUser),
            (3, "Back", () => { })
        }, () => { Console.WriteLine("Action Menu"); });
    }

    private static void SearchUser()
    {
        while (true)
            try
            {
                Console.Write("Input Name : ");
                var name = Console.ReadLine();

                var foundUsers = UserService.SearchUser(name);

                foundUsers.ForEach(Console.WriteLine);
                Console.ReadLine();
                Console.Clear();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }
    
    // Display random jokes from API call
    private static void DisplayJokes()
    {
        Console.Clear();
        try
        {
            Console.WriteLine("Wait for it..");
            HttpResponseMessage response = new HttpClient().GetAsync("https://official-joke-api.appspot.com/jokes/random").Result;

            var body = response.Content.ReadAsStringAsync().Result;

            var jsonBody = JsonNode.Parse(body);

            Console.WriteLine($"[?] {jsonBody["setup"]}");

            Console.WriteLine("\n↵ for punchline");
            Console.ReadLine();

            Console.WriteLine($"[✓] {jsonBody["punchline"]} XD");

            Console.ReadLine();
            Console.Clear();
        }
        catch (Exception e)
        {
            Console.WriteLine("Sorry cant make a joke right now");
        }
    }

    private static void LoginUser()
    {
        while (true)
            try
            {
                Console.Write("Username : ");
                var username = Console.ReadLine();
                Console.Write("Password : ");
                var password = Console.ReadLine();

                var loggedInUser = UserService.LoginUser(username, password);

                Console.Clear();
                Console.WriteLine("[✓] Login success.");
                Console.WriteLine($"Welcome {loggedInUser.FirstName}, hope you doing well!");
                Thread.Sleep(1000);
                Console.WriteLine(".\n.\n.");
                MenuHandler(new List<(int, string, Action)>
                {
                    (1, "Alright!", DisplayJokes),
                    (2, "Nope", () => { })
                }, () => { Console.WriteLine($"[?] Hey {loggedInUser.FirstName}, do you wanna hear some jokes?"); });
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }

    // listMenu list of menu to be display & serve
    // cb function to be execute before menu
    public static void MenuHandler(List<(int, string, Action)> listMenu, Action cb)
    {
        while (true)
        {
            cb();

            listMenu.ForEach(menu => Console.WriteLine($"{menu.Item1}. {menu.Item2}"));

            try
            {
                Console.Write("\n[.] Select Menu : ");
                var inputMenu = Convert.ToInt32(Console.ReadLine());

                Console.Clear();
                var selected = listMenu.First(tuple => tuple.Item1 == inputMenu);
                if (selected.Item2.Equals("Exit") || selected.Item2.Equals("Back") ||
                    selected.Item2.Equals("Nope")) return;

                selected.Item3();
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("[!] Invalid menu.\n");
            }
        }
    }

    public static void Start()
    {
        Console.Clear();
        MenuHandler(new List<(int, string, Action)>
        {
            (1, "Create User", CreateUser),
            (2, "Show Users", ShowUsers),
            (3, "Search User", SearchUser),
            (4, "Login User", LoginUser),
            (5, "Exit", () => { }),
        }, () => { Console.WriteLine("== BASIC AUTHENTICATION =="); });
    }
}
