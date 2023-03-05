using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LibraryAssignment
{
    class UserDetails
    {
        String UserName { get; set; }
        String UserPhone { get; set; }
        String UserEmail { get; set; }
        String UserId { get; set; }
        String UserTag { get; set; }
        String UserBooks { get; set; }
        int UserNoBooks { get; set; }

        public static UserDetails currentUser = new UserDetails();

        public void getDetails(UserDetails currentUser)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("UserList.xml");
            XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/catalog/User");
            
            foreach(XmlNode node in xmlNodeList)
            {
                UserName = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/Name"));
                UserPhone = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/Telephone"));
                UserEmail = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/Email"));
                UserId = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/UserID"));
                UserTag = Convert.ToString(xmlDocument.SelectSingleNode("/catalog/User/Tag"));
 
            }
        }

    }
}
