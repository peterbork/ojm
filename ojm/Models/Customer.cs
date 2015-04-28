using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ojm.Models {
    class Customer {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string CVR { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string ContactPerson { get; set; }
        public Customer(int id, string companyname, string cvr, string address, string email, string phonenumber, string contactperson) {
            this.ID = id;
            this.CompanyName = companyname;
            this.CVR = cvr;
            this.Address = address;
            this.Email = email;
            this.Phonenumber = phonenumber;
            this.ContactPerson = contactperson;
        }
        public Customer(string companyname, string cvr, string address, string email, string phonenumber, string contactperson) {
            this.CompanyName = companyname;
            this.CVR = cvr;
            this.Address = address;
            this.Email = email;
            this.Phonenumber = phonenumber;
            this.ContactPerson = contactperson;
        }
        public Customer() { 
        
        }
    }
}
