using System;
using System.Collections;

namespace API.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime DoB { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Address { get; set; }
        public ICollection Orders { get; set; }
        public OrderDto Basket { get; set; }
    }
}