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
                var id = Console.ReadLine();

                var userId = Convert.ToUInt32(id);

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

        Console.WriteLine("Menu");
        Console.WriteLine("1. Edit User");
        Console.WriteLine("2. Delete User");
        Console.WriteLine("3. Back");

        try
        {
            var inputMenu = Console.ReadLine();

            var input = Convert.ToInt32(inputMenu);

            switch (input)
            {
                case 1:
                    EditUser();
                    break;
                case 2:
                    DeleteUser();
                    break;
                case 3:
                    return;
                default:
                    Console.WriteLine("invalid menu.");
                    break;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("invalid menu.");
        }
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

    public static void Start()
    {
        while (true)
        {
            Console.WriteLine("== BASIC AUTHENTICATION ==");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Show User");
            Console.WriteLine("3. Search User");
            Console.WriteLine("4. Login User");
            Console.WriteLine("5. Exit");

            try
            {
                var inputMenu = Console.ReadLine();

                var input = Convert.ToInt32(inputMenu);

                switch (input)
                {
                    case 1:
                        Console.Clear();
                        CreateUser();
                        break;
                    case 2:
                        Console.Clear();
                        ShowUsers();
                        break;
                    case 3:
                        Console.Clear();
                        SearchUser();
                        break;
                    case 4:
                        Console.Clear();
                        LoginUser();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("invalid menu.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("invalid menu.");
            }
        }
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