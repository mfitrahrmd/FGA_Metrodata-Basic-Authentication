namespace Tugas_4;

public class UserService
{
    private static readonly UserRepository UserRepository = new();

    // for storing duplicate username state
    private static readonly Dictionary<string, uint> UsernameSuffix = new();

    public static string GenerateUsername(string firstName, string lastName)
    {
        var generatedUsername = firstName[..2] + lastName[..2];

        if (UserRepository.IsUsernameExist(generatedUsername))
        {
            UsernameSuffix[generatedUsername]++;
            generatedUsername += UsernameSuffix[generatedUsername].ToString();
        }

        UsernameSuffix.Add(generatedUsername, 0);
        return generatedUsername;
    }

    public static User CreateUser(string firstName, string lastName, string password)
    {
        var newUser = new User(firstName, lastName, password);

        var createdUser = UserRepository.InsertOne(newUser);

        return createdUser;
    }

    public static List<User> SearchUser(string name)
    {
        var foundUsers = UserRepository.FindManyByFullname(name);

        if (foundUsers.Count < 1) throw new Exception("[!] User not found");

        return foundUsers;
    }

    public static User UpdateUser(uint id, string newFirstName, string newLastName, string newPassword)
    {
        var updatedUser = UserRepository.UpdateById(id, newFirstName, newLastName, newPassword);

        return updatedUser;
    }

    public static User DeleteUser(uint id)
    {
        var deletedUser = UserRepository.DeleteById(id);

        return deletedUser;
    }

    public static User LoginUser(string username, string password)
    {
        User foundUser;

        try
        {
            foundUser = UserRepository.FindOneByUsername(username);

            if (password != foundUser.Password) throw new Exception("[!] Incorrect password.");
        }
        catch (KeyNotFoundException)
        {
            throw new Exception($"[!] Username {username} does not exist.");
        }

        return foundUser;
    }

    public static List<User> ListUsers()
    {
        var foundUsers = UserRepository.List();

        if (foundUsers.Count < 1) throw new Exception("[!] No user.");

        return foundUsers;
    }
}
