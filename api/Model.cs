using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api
{
    public class Book
    {
        public string title { get; set; }
        public Author author { get; set; }
        public string summary { get; set; }
        public string ISBN { get; set; }
        public List<Genre> genre { get; set; }
        public string url { get; set; }
        public string ID { get; set; }
    }
    public class Author
    {
        public string first_name { get; set; }
        public string family_name { get; set; }
        public DateTime date_of_birth { get; set; }
        public DateTime date_of_death { get; set; }
        public string name { get; set; }
        public string lifespan { get; set; }
        public string url { get; set; }
    }

    public class Genre
    {
        public string name { get; set; }
    }
}