using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleCallTranslator.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int HeightInCm { get; set; }
    }
}
