using System.ComponentModel.DataAnnotations;

namespace BlazorKopikoDemoClient.Client.DataModels
{
    public class OrderModel
    {
        [Required(ErrorMessage = "Ship name is required.")]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "Ship address is required.")]      
        public string ShipAddress { get; set; }
    }
}
