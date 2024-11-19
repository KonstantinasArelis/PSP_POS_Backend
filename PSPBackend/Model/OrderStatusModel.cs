using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PSPBackend.Model
{
    public class OrderStatusModel
    {
        [Key]
        public int order_status_id { get; set; }
        public string order_status_name { get; set; }
    }
}
