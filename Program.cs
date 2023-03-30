using System.Diagnostics;
using Tugas_4;

internal class UserService
{
    private static readonly Dictionary<string, User> Users = new();

    // for storing duplicate username state
    private static readonly Dictionary<string, uint> UsernameSuffix = new();

    public static string GenerateUsername(string firstName, string lastName)
    {
        var generatedUsername = firstName[..2] + lastName[..2];

        if (Users.ContainsKey(generatedUsername))
        {
            UsernameSuffix[generatedUsername]++;
            generatedUsername += UsernameSuffix[generatedUsername].ToString();
        }

        UsernameSuffix.Add(generatedUsername, 0);
        return generatedUsername;
    }

    public static void CreateUser(string firstName, string lastName, string password)
    {
        var newUser = new User(firstName, lastName, password);

        Users.Add(newUser.Username, newUser);
    }

    public static List<User> SearchUser(string name)
    {
        var foundUsers = from user in Users where user.Value.FullName.Contains(name) select user.Value;

        return foundUsers.ToList();
    }

    public static bool UpdateUser(uint id, string newFirstName, string newLastName, string newPassword)
    {
        foreach (var kv in Users)
            if (kv.Value.Id == id)
            {
                kv.Value.FirstName = newFirstName;
                kv.Value.LastName = newLastName;
                kv.Value.Password = newPassword;

                return true;
            }

        throw new Exception("user not found");
    }

    public static bool DeleteUser(uint id)
    {
        foreach (var user in Users)
            if (user.Value.Id == id)
            {
                Users.Remove(user.Key);
                return true;
            }

        throw new Exception("user not found");
    }

    public static bool LoginUser(string username, string password)
    {
        try
        {
            var foundUser = Users[username];

            if (password != foundUser.Password) throw new Exception("incorrect password.");
        }
        catch (KeyNotFoundException)
        {
            throw new Exception($"username {username} does not exist.");
        }

        return true;
    }

    public static void PrintAllUsers()
    {
        foreach (var user in Users) Console.WriteLine(user.Value);
    }
}

internal class CLIProgram
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

                UserService.CreateUser(firstName, lastName, password);
                Console.WriteLine("user successfully created");
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

                Console.WriteLine("user successfully updated");
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

                UserService.DeleteUser(userId);

                Console.WriteLine("user successfully deleted");
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
        UserService.PrintAllUsers();
        
        MenuHandler(new List<(int, string, Action)>
        {
            (1, "Edit User", EditUser),
            (2, "Delete User", DeleteUser),
            (3, "Back", () => { })
        }, () =>
        {
            Console.WriteLine("Action Menu");
        });
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

    private static void LoginUser()
    {
        while (true)
            try
            {
                Console.Write("Username : ");
                var username = Console.ReadLine();
                Console.Write("Password : ");
                var password = Console.ReadLine();

                var isLoginSuccess = UserService.LoginUser(username, password);
                if (isLoginSuccess) Console.WriteLine("login success.");
                Console.ReadLine();
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
                if (selected.Item2.Equals("Exit") || selected.Item2.Equals("Back")) return;

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
        }, () =>
        {
            Console.WriteLine("== BASIC AUTHENTICATION ==");
        });
    }
}

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(DateTime.Now);
        CLIProgram.Start();
    }
}