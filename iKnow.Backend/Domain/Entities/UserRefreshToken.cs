using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class UserRefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string RefreshToken { get; set; }

    public DateTime ExpiredAt { get; set; }
}
