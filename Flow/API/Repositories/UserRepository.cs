using API.Models;

namespace API.Repositories;

public class UserRepository : IUserRepository
{
    private List<User> users = new()
    {
        new User() { Id = 1, Name = "John Doe", Email = "random@gmail.com", Password = "password" },
        new User() { Id = 2, Name = "Jane Smith", Email = "jane.smith@email.com", Password = "jsmith123" },
        new User() { Id = 3, Name = "Alice Johnson", Email = "alice.j@email.com", Password = "alicepw" },
        new User() { Id = 4, Name = "Bob Brown", Email = "bob.brown@email.com", Password = "bobbypw" },
        new User() { Id = 5, Name = "Charlie Davis", Email = "charlie.davis@email.com", Password = "charlie123" },
        new User() { Id = 6, Name = "Daniel Evans", Email = "daniel.evans@email.com", Password = "evanspass" },
        new User() { Id = 7, Name = "Eva Miller", Email = "eva.miller@email.com", Password = "evapass456" },
        new User() { Id = 8, Name = "Frank Moore", Email = "frank.moore@email.com", Password = "frankpass" },
        new User() { Id = 9, Name = "Gloria Hall", Email = "gloria.hall@email.com", Password = "gloria789" },
        new User() { Id = 10, Name = "Henry King", Email = "henry.king@email.com", Password = "king987" },
        new User() { Id = 11, Name = "Isabel Lewis", Email = "isabel.lewis@email.com", Password = "isabelpw" },
        new User() { Id = 12, Name = "Jackie Martin", Email = "jackie.martin@email.com", Password = "martinpass" },
        new User() { Id = 13, Name = "Kevin Nelson", Email = "kevin.nelson@email.com", Password = "kevinnelson" },
        new User() { Id = 14, Name = "Linda Owens", Email = "linda.owens@email.com", Password = "linda123" },
        new User() { Id = 15, Name = "Mike Parker", Email = "mike.parker@email.com", Password = "parkerpw" },
        new User() { Id = 16, Name = "Nina Quinn", Email = "nina.quinn@email.com", Password = "ninaquinn" },
        new User() { Id = 17, Name = "Oliver Reed", Email = "oliver.reed@email.com", Password = "reedpass" },
        new User() { Id = 18, Name = "Paula Scott", Email = "paula.scott@email.com", Password = "paulapw" },
        new User() { Id = 19, Name = "Quentin Turner", Email = "quentin.turner@email.com", Password = "turnerqw" },
        new User() { Id = 20, Name = "Rachel Underwood", Email = "rachel.underwood@email.com", Password = "rachelpw" },
        new User() { Id = 21, Name = "Steve Vincent", Email = "steve.vincent@email.com", Password = "stevevin" },
        new User() { Id = 22, Name = "Tina White", Email = "tina.white@email.com", Password = "tinawhite" },
        new User() { Id = 23, Name = "Uma Xavier", Email = "uma.xavier@email.com", Password = "umaxavier" },
        new User() { Id = 24, Name = "Victor Young", Email = "victor.young@email.com", Password = "victoryoung" },
        new User() { Id = 25, Name = "Wendy Zane", Email = "wendy.zane@email.com", Password = "wendyzane" },
        new User() { Id = 26, Name = "Zack Allen", Email = "zack.allen@email.com", Password = "zackallen" }
    };


    public User? Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return null;
        return users.FirstOrDefault(x => x.Name == username && x.Password == password);
    }

    // 1 = Malformed UserDTO data
    // 2 = Username Already exists
    // -1 = Server error
    // 0 = Accepted
    public int Register(UserDTO user)
    {
        if (user == null)
            return 1;
        try
        {
            var addUs = new User()
            {
                Id = MaxId() + 1,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
            bool usrnExist = users.Any(x => x.Name == addUs.Name);
            if (usrnExist)
                return 2;
            users.Add(addUs);
            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

    private int MaxId()
    {
        return users.Max(x => x.Id);
    }
}