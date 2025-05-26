using DataAccessLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace BusinessLogicLayer
{
    //Marin Grabovac, Lucija Polak
    public class UsersService
    {
        private readonly string encryptionKey = "Zajednicki-projekt-RPP-G2P!";
        public string GetUserRole(string username, string password)
        {
            var repo = new UserRepository();

            string hashedPassword = HashData(password);

            var user = repo.GetUserWithRole(username, hashedPassword);

            if (user == null)
            {
                return "Unknown";
            }

            if (VerifyPassword(password, user.Password) == false)
            {
                return "Unknown";
            }

            return user.UserType.TypeName;
        }
        public void RegisterUser(string username, string password, string email, string phoneNumber, string firstName, string lastName)
        {

            string hashedPassword = HashData(password);
            User user = new User
            {
                Username = username,
                Password = hashedPassword,
                Email = email,
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                IsSubscribed = 0,
                ID_UserType = 2,
                ID_Institution = null
            };

            using (var repo = new UserRepository())
            {
                repo.AddUser(user);
            }
        }
        public string HashData(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var inputHash = HashData(inputPassword);
            return inputHash == storedHash;
        }
        public bool EmailInUse(string email)
        {
            var inUse = false;
            using (var repo = new UserRepository())
            {
                inUse = repo.EmailExists(email);
            }
            if (inUse == false) return false;
            return true;
        }

        public bool UsernameInUse(string username)
        {
            var inUse = false;
            using (var repo = new UserRepository())
            {
                inUse = repo.UsernameExists(username);
            }
            if (inUse == false) return false;
            return true;
        }

        public string ReturnLoggedUser(string username) {
            var user = new User();
            using (var repo = new UserRepository())
            {
                user = repo.GetOneByName(username);
            }
            if (user == null) return null;
            return EncryptData(user.ID_User.ToString());
        }

        public string ReturnEncryptedRole(string role)
        {
            return EncryptData(role);
        }

        public string ReturnDecryptedRole(string role)
        {
            return DecryptData(role);
        }

        private byte[] GenerateKey(string encryptionKey)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey)).Take(16).ToArray();
            }
        }

        private byte[] GenerateIV()
        {
            byte[] iv = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }

        private string EncryptData(string regularText)
        {
            byte[] keyBytes = GenerateKey(encryptionKey);
            byte[] iv = GenerateIV();

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(regularText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string DecryptData(string encryptedText)
        {
            byte[] keyBytes = GenerateKey(encryptionKey);
            byte[] cipherBytes = Convert.FromBase64String(encryptedText);

            byte[] iv = cipherBytes.Take(16).ToArray();
            byte[] actualCipher = cipherBytes.Skip(16).ToArray();

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(actualCipher))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public bool UpdateSubscribedStatus(int id, short status)
        {
            using (var repo = new UserRepository())
            {
                var value = repo.UpdateSubscribedStatus(id, status);
                if (value == false) return false;
                return true;
            }
        }
        public bool GetSubscriptionStatus(int userId)
        {
            using (var repo = new UserRepository())
            {
                var isSubscribed = repo.GetSubscriptionStatus(userId);
                return isSubscribed;
            }
        }
        private void SendEmail(string postTitle, string postContent)
        {
            using (var repo = new UserRepository())
            {
                var users = repo.GetSubsribedUsers();
            }
        }
        public List<User> GetSubsribedUsers()
        {
            using (var repo = new UserRepository())
            {
                var users = repo.GetSubsribedUsers();
                return users;
            }
        }
        public List<User> GetAllUsers()
        {
            using (var ur = new UserRepository())
            {
                return ur.GetAllUsers().ToList();
            }
        }
        public List<User> GetAllEmployees()
        {
            using (var ur = new UserRepository())
            {
                return ur.GetEmployees().ToList();
            }
        }

        public List<User> GetEmployeesForInstitution(int id)
        {
            using (var ur = new UserRepository())
            {
                return ur.GetEmployeesFromInstitution(id).ToList();
            }
        }

        public void RemoveUser(User user)
        {
            using (var ur = new UserRepository())
            {
                ur.RemoveUser(user);
            }
        }

        public List<UserType> GetUserTypes()
        {
            using (var ur = new UserRepository())
            {
                return ur.GetRoles().ToList();
            }
        }
        public void AddUser(User user)
        {
            using (var ur = new UserRepository())
            {
                ur.AddUser(user);
            }
        }

        public void UpdateUser(User user)
        {
            using (var ir = new UserRepository())
            {
                ir.UpdateUser(user);
            }
        }

        public UserType GetUserTypeById(int selectedValue)
        {
            using (var ur = new UserRepository())
            {
                return ur.GetUserTypeById(selectedValue) as UserType;
            }
        }

        public List<User> GetUsersByRole(string role)
        {
            using (var ur = new UserRepository())
            {
                return ur.GetUsersByRole(role).ToList();
            }
        }
    }
}

