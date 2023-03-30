using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using Tugas_4;

class UserService
{
    private static Dictionary<string, User> _users = new Dictionary<string, User>();

    // for storing duplicate username state
    private static Dictionary<string, uint> _usernameSuffix = new Dictionary<string, uint>();

    public static string GenerateUsername(string firstName, string lastName)
    {
        string GeneratedUsername = firstName[..2] + lastName[..2];

        if (_users.ContainsKey(GeneratedUsername))
        {
            _usernameSuffix[GeneratedUsername]++;
            GeneratedUsername += _usernameSuffix[GeneratedUsername].ToString();
        }

        _usernameSuffix.Add(GeneratedUsername, 0);
        return GeneratedUsername;
    }

    public static void CreateUser(string firstName, string lastName, string password)
    {
        User newUser = new User(firstName, lastName, password);

        _users.Add(newUser.Username, newUser);
    }

    public static List<User> SearchUser(string name)
    {
        var foundUsers = from user in _users where user.Value.FullName.Contains(name) select user.Value;

        return foundUsers.ToList();
    }
    
    public static bool UpdateUser(uint id, string newFirstName, string newLastName, string newPassword)
    {
        User foundUser;
        
        foreach (var kv in _users)
        {
            if (kv.Value.Id == id)
            {
                kv.Value.FirstName = newFirstName;
                kv.Value.LastName = newLastName;
                kv.Value.Password = newPassword;

                return true;
            }
        }

        throw new Exception("user not found");
    }

    public static bool DeleteUser(uint id)
    {
        foreach (var user in _users)
        {
            if (user.Value.Id == id)
            {
                _users.Remove(user.Key);
                return true;
            }
        }

        throw new Exception("user not found");
    }

    public static bool LoginUser(string username, string password)
    {
        try
        {
            User foundUser = _users[username];

            if (password != foundUser.Password)
            {
                throw new Exception($"incorrect password.");
            }
        }
        catch (KeyNotFoundException e)
        {
            throw new Exception($"username {username} does not exist.");
        }

        return true;
    }

    public static void PrintAllUsers()
    {
        foreach (var user in _users)
        {
            Console.WriteLine(user.Value);
        }
    }
}

class CLIProgram
{
    public static void CreateUser()
    {
        while (true)
        {
            try
            {
                Console.Write("First Name : ");
                string? firstName = Console.ReadLine();
                Console.Write("Last Name : ");
                string? lastName = Console.ReadLine();
                Console.Write("Password : ");
                string? password = Console.ReadLine();

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
    }

    public static void EditUser()
    {
        while (true)
        {
            try
            {
                Console.Write("User Id to be edited : ");
                string? id = Console.ReadLine();
                Console.Write("New First Name : ");
                string? newFirstName = Console.ReadLine();
                Console.Write("New Last Name : ");
                string? newLastName = Console.ReadLine();
                Console.Write("New Password : ");
                string? newPassword = Console.ReadLine();

                uint userId = Convert.ToUInt32(id);

                UserService.UpdateUser(userId, newFirstName, newLastName, newPassword);

                Console.WriteLine("user successfully updated");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            catch (FormatException e)
            {
                Console.WriteLine("invalid id");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
    
    public static void DeleteUser()
    {
        while (true)
        {
            try
            {
                Console.Write("User Id to be deleted : ");
                string? id = Console.ReadLine();

                uint userId = Convert.ToUInt32(id);

                UserService.DeleteUser(userId);

                Console.WriteLine("user successfully deleted");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            catch (FormatException e)
            {
                Console.WriteLine("invalid id");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
    
    public static void ShowUsers()
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
        catch (Exception e)
        {
            Console.WriteLine("invalid menu.");
        }

    }

    public static void SearchUser()
    {
        while (true)
        {
            try
            {
                Console.Write("Input Name : ");
                string? name = Console.ReadLine();

                List<User> foundUsers = UserService.SearchUser(name);
                
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
    }

    public static void LoginUser()
    {
        while (true)
        {
            try
            {
                Console.Write("Username : ");
                string? username = Console.ReadLine();
                Console.Write("Password : ");
                string? password = Console.ReadLine();

                bool isLoginSuccess = UserService.LoginUser(username, password);
                if (isLoginSuccess) Console.WriteLine("login success.");
                Console.ReadLine();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    
    public static void Start()
    {
        while (true)
        {
            Console.WriteLine($"== BASIC AUTHENTICATION ==");
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