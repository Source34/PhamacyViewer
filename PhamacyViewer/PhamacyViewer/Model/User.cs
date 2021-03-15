using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhamacyViewer.Model
{
    public static class User
    {
        static public int Id { get; set; }
        static public string Email { get; set; }
        static public string FIO { get; set; }
        static public string JwtToken { get; set; }
        static public List<Card> Cards { get; set; }
        static public int CardId { get; set; }

        static public void Destruction()
        {
            Id = 0;
            Email = null;
            FIO = null;
            JwtToken = null;
            Cards = null;
            CardId = 0;
        }
    }
}
