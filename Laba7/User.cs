using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Laba7
{
    internal class User
    {
        private int id;
        private static int counter = 0;  

        private string surname;
        private string name;
        private string patronymic;
        private bool isBlocked;
        private string email;
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

        public string Telnum { get => telnum; set => telnum = value; }
        public bool IsBlocked { get => isBlocked; set => isBlocked = value; }
        public int Id { get => id; }
        public string Name { get => name; set => name = value; }
        public string Patronymic { get => patronymic; set => patronymic = value; }
        public string Email { get => email; set => email = value; }
        public string Surname { get => surname; set => surname = value; }


        public override string ToString()
        {
            return $"Пользователь с id {id}: {surname} {name} {patronymic}, статус блокировки:{isBlocked}, телефон: {telnum}, e-mail:{email}.";
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
