using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PSPBackend.Model
{
    public class OrderStatusModel
    {
        [Key]
        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }
    }
}
