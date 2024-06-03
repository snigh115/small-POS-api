using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using System.Threading.Tasks;

namespace POS.Model.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }

        [JsonIgnore]
        public ICollection<ProductDto> ProductDto { get; set; }
    }
}