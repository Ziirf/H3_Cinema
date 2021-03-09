using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models
{
    public class Postcode
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public int Code { get; set; }
        public string City { get; set; }
    }
}
