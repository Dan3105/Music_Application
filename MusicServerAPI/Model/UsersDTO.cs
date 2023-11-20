using MusicServerAPI.Entity;

namespace MusicServerAPI.Model
{
    public class RoleDTO
    {
        public int Id { get; set; } 
        public string? Name { get; set; }

        public RoleDTO(Role role) {
            Id = role.Id;
            Name = role.RoleName;
        }

        public RoleDTO(int id, string? name)
        {
            Id = id;
            Name = name;
        }
    }

    public class UsersDTO
    {
        public UsersDTO(int id, string? email, DateTime createdDate, List<RoleDTO>? roleDTOs, bool isActive)
        {
            Id = id;
            Email = email;
            CreatedDate = createdDate;
            this.roleDTOs = roleDTOs;
            this.IsActive = isActive;
        }

        public UsersDTO(User user)
        {
            Id = user.Id;
            Email = user.Email;
            CreatedDate = user.Created;
            IsActive = user.Is_activate;
            if(user.Roles  != null)
            {
                roleDTOs = new List<RoleDTO>();
                foreach(Role role in user.Roles)
                {
                    roleDTOs.Add(new RoleDTO(role));
                }
            }
        }

        public int Id { get; set; }
        public string? Email { get; set; }  
        public DateTime CreatedDate { get; set; }   
        public List<RoleDTO>? roleDTOs { get; set; }
        public bool IsActive { get; set; }

    }
}
