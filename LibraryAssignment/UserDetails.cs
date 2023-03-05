using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LibraryAssignment
{
    public class UserDetails
    {
        public String UserName { get; set; }
        public String UserPhone { get; set; }
        public String UserEmail { get; set; }
        public String UserId { get; set; }
        public String UserTag { get; set; }
        public String UserBooks { get; set; }
        public int UserNoBooks { get; set; }

        // public static UserDetails currentUser = new UserDetails();
        //
        // public void getDetails(UserDetails currentUser)
        // {
        //     XmlDocument xmlDocument = new XmlDocument();
        //     xmlDocument.Load("UserList.xml");
        //     XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/User");
        //     
        //     foreach(XmlNode node in xmlNodeList)
        //     {
        //         UserName = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/Name"));
        //         UserPhone = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/Telephone"));
        //         UserEmail = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/Email"));
        //         UserId = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/UserID"));
        //         UserTag = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/Tag"));
        //
        //     }
        // }

    }
}
