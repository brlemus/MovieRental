using MovieRental.Core.Movie;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieRental.Common;

namespace MovieRental.Core.Account
{
    public class AccountEntity : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public ICollection<MovieLikesEntity> LikedProducts { get; set; }

        public static List<AccountEntity> CreateDumpData()
        {
            return new List<AccountEntity>
            {
                new AccountEntity()
                {
                    UserName = "admin",
                    Password = "admin01".ToSha256(),
                    Role = Role.Admin
                },
                new AccountEntity()
                {
                    UserName = "user",
                    Password = "user01".ToSha256(),
                    Role = Role.User
                }
            };
        }
    }

    public enum Role
    {
        Admin = 0,
        User = 1
    }
}
