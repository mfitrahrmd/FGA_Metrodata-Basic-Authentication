namespace Tugas_4;

public class UserRepository
{
    private readonly Dictionary<string, User> _users = new();

    public UserRepository()
    {
    }

    public UserRepository(IEnumerable<User> initialData)
    {
        foreach (var user in initialData)
        {
            _users.Add(user.Username, user);
        }
    }
    
    public bool IsUsernameExist(string username)
    {
        return _users.ContainsKey(username);
    }

    public User InsertOne(User user)
    {
        _users.Add(user.Username, user);

        return user;
    }

    public User FindOneByUsername(string username)
    {
        var foundUser = _users[username];

        return foundUser;
    }

    public List<User> FindManyByFullname(string fullname)
    {
        var foundUsers = (from user in _users where user.Value.FullName.Contains(fullname) select user.Value);

        return foundUsers.ToList();
    }

    public User UpdateById(uint id, string newFirstName, string newLastName, string newPassword)
    {
        var foundUser = (from user in _users where user.Value.Id.Equals(id) select user).FirstOrDefault();

        foundUser.Value.FirstName = newFirstName;
        foundUser.Value.LastName = newLastName;
        foundUser.Value.Password = newPassword;

        return foundUser.Value;
    }

    public User DeleteById(uint id)
    {
        var foundUser = (from user in _users where user.Value.Id.Equals(id) select user).FirstOrDefault();

        _users.Remove(foundUser.Key);

        return foundUser.Value;
    }

    public List<User> List()
    {
        var foundUsers = (from user in _users select user.Value);

        return foundUsers.ToList();
    }
}