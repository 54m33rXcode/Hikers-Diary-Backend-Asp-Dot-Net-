using Azure.Core;
using Hikers_Diary.DTO;
using Hikers_Diary.Interfaces;
using HikersDiary_Web.Data;
using HikersDiary_Web.Model;
using Microsoft.EntityFrameworkCore;

namespace Hikers_Diary.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepository(MyDbContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        public void AddUser(UserDto user)
        {
            var newUser = new User
            {
                UserName = user.UserName,
                Password = user.Password
            };

            if (user.Profile_Pic_File != null && user.Profile_Pic_File.Length > 0)
            {
                newUser.Profile_Pic = SaveProfilePic(user.Profile_Pic_File);
            }

            _context.User.Add(newUser);
            _context.SaveChanges();
            user.UserId = newUser.UserId;

        }

        public UserDto GetUserById(int userId)
        {
            var user = _context.User.FirstOrDefault(s=>s.UserId == userId);
            return MapUserToDto(user);
        }

        public UserDto GetUserByUsername(string username)
        {
            var user = _context.User.FirstOrDefault(n => n.UserName == username);
            return MapUserToDto(user);
        }

        public IEnumerable<UserDto> SearchUsers(string searchQuery)
        {
            var users = _context.User
            .Where(s => s.UserName.Contains(searchQuery))
            .ToList() 
            .Select(u => new UserDto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Profile_Pic = GetProfilePic(u.Profile_Pic)
            })
            .ToList();
                return users;

        }

        public void UpdateUser(UserDto user)
        {
            var existinguser = _context.User.Find(user.UserId);
            if (existinguser != null)
            {
                existinguser.UserName = user.UserName;
                existinguser.Password = user.Password;

                if (user.Profile_Pic_File != null && user.Profile_Pic_File.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existinguser.Profile_Pic))
                    {
                        var previousProfilePicPath = Path.Combine(Directory.GetCurrentDirectory(), existinguser.Profile_Pic.TrimStart('/'));
                        if (File.Exists(previousProfilePicPath))
                        {
                            File.Delete(previousProfilePicPath);
                        }
                    }

                    existinguser.Profile_Pic = SaveProfilePic(user.Profile_Pic_File);
                }
            }
            _context.User.Update(existinguser);
            _context.SaveChanges();
        }



        public UserDto ValidateUser(string username, string password)
        {
            if (username == null || password == null) { return null; }

            var user = _context.User.FirstOrDefault(u => u.UserName == username && u.Password == password);

            return MapUserToDto(user);
        }

        private UserDto MapUserToDto(User? user)
        {
            if (user == null)
                return null;

            var userDto = new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = user.Password,
                Profile_Pic = GetProfilePic(user.Profile_Pic),
            };
            return userDto;

        }


        private string SaveProfilePic(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string uniqueFileName = "_Profile" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Profilep");
                string destinationPath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(destinationPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return $"/Profilep/{uniqueFileName}";
            }

            return null;
        }

        public string GetProfilePic(string fileName)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null)
            {
                string baseUrl = request.Scheme + "://" + request.Host.Value;
                string imageUrl = baseUrl + fileName;

                return imageUrl;
            }

            return null;
        }
    }
 }
