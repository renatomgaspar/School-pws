using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School_pws.Data.Entities;
using School_pws.Helpers;

namespace School_pws.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Student");

            var user = await _userHelper.GetUserByEmailAsync("school_manager@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "School",
                    LastName = "Manager",
                    Email = "school_manager@gmail.com",
                    UserName = "school_manager@gmail.com",
                    PhoneNumber = "911111111"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!_context.Subjects.Any())
            {
                AddSubject("MathA", "Mathematics A", user);
                AddSubject("MathB", "Mathematics B", user);
                AddSubject("EngC2", "English C2", user);
                AddSubject("ProgObj", "Object Programming", user);

                await _context.SaveChangesAsync();
            }
        }

        private void AddSubject(string code, string name, User user)
        {
            _context.Subjects.Add(new Subject
            {
                Code = code,
                Name = name,
                Description = "This subject is designed to provide students with a comprehensive understanding of the key concepts and skills within its field. It emphasizes the development of critical thinking, analytical reasoning, and problem-solving abilities, enabling students to approach challenges methodically and creatively. Students are encouraged to engage with both theoretical knowledge and practical applications, fostering a deeper comprehension of the subject matter. The course also aims to enhance independent learning, effective communication, and collaborative work, preparing students for further education or professional environments where these skills are essential.",
                Workload = _random.Next(1000),
                IsActive = true,
                User = user
            });
        }
    }
}
