using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.Domain.Models
{
    public class Postcode
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string City { get; set; }
    }
}
