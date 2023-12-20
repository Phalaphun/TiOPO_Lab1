using System.Net.Mail;
using System.Runtime.Serialization;


namespace Laba7Liba
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "Id")]
        private int id;
        [DataMember(Name = "counter")]
        public static int counter = 0;

        [DataMember(Name = "Surname")]
        private string surname;
        [DataMember(Name = "Name")]
        private string name;
        [DataMember(Name = "Patronymic")]
        private string patronymic;
        [DataMember(Name = "IsBlocked")]
        private bool isBlocked;
        [DataMember(Name = "Email")]
        private string email;
        [DataMember(Name = "Telnum")]
        private string telnum;

        public User(string surname, string name, string patronymic, bool isBlocked, string email, string telnum)
        {
            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.isBlocked = isBlocked;
            this.email = email;
            this.telnum = telnum;

            this.id = counter;
            counter++;
        }

        public User(string surname, string name, string patronymic, string email, string telnum)
        {
            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.email = email;
            this.telnum = telnum;

            this.isBlocked = false;
            this.id = counter;
            counter++;
        }

        public User(User OldUser, bool cloneID)
        {
            this.surname = OldUser.Surname;
            this.name = OldUser.Name;
            this.patronymic = OldUser.Patronymic;
            this.email = OldUser.Email;
            this.telnum = OldUser.Telnum;

            this.isBlocked = OldUser.IsBlocked;
            if(cloneID )
            {
                id = OldUser.Id;
            }
            else
            {
                id = counter;
                counter++;
            }
        }

        public string Telnum { get => telnum; set => telnum = value; }
        public bool IsBlocked { get => isBlocked; set => isBlocked = value; }
        public int Id { get => id; }
        public string Name { get => name; set => name = value; }
        public string Patronymic { get => patronymic; set => patronymic = value; }
        public string Email { get => email; set => email = value; }
        public string Surname { get => surname; set => surname = value; }


        public override string ToString()
        {
            return $"Пользователь с id {id}: {surname} {name} {patronymic}, статус блокировки:{isBlocked}, e-mail:{email}, телефон: {telnum}.";
        }
        public bool IsValidEMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
