﻿namespace KBMHttpService.DTOs.User
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
