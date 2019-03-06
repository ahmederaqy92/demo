using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Benday.Presidents.Api.Models
{
    public class Term
    {
        public Term()
        {
            
        }

        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public string Role { get; set; }

        // [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        // [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }
        public int Number { get; set; }
    }
}
