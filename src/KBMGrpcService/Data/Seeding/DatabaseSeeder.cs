﻿using KBMGrpcService.Data.Seeding.DTOs;
using KBMGrpcService.Entities;
using KBMGrpcService.Entities.ValueObjects;
using KBMGrpcService.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;

namespace KBMGrpcService.Data.Seeding
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        public async Task SeedAsync(AppDbContext context)
        {
            if (!File.Exists(AppConstants.SeedFile)) return;

            var json = await File.ReadAllTextAsync(AppConstants.SeedFile);
            var data = JsonSerializer.Deserialize<SeedDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (data == null) return;

            foreach (var orgDto in data.Organizations)
            {
                if (await context.Organizations.AnyAsync(o => o.Name == orgDto.Name))
                    continue;
                context.Organizations.Add(new Organization
                {
                    Name = orgDto.Name,
                    Address = orgDto.Address
                });
            }
            await context.SaveChangesAsync();

            foreach (var userDto in data.Users)
            {
                if (await context.Users.AnyAsync(u => u.Username == userDto.Username))
                    continue;
                context.Users.Add(new User
                {
                    Name = userDto.Name,
                    Username = userDto.Username,
                    Email = new Email(userDto.Email)
                });
            }
            await context.SaveChangesAsync();

            foreach (var assoc in data.Memberships)
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == assoc.Username);
                var org = await context.Organizations.FirstOrDefaultAsync(o => o.Name == assoc.OrganizationName);
                if (user == null || org == null) continue;
                if (await context.UserOrganizations.AnyAsync(uo => uo.UserId == user.Id && uo.OrganizationId == org.Id))
                    continue;
                context.UserOrganizations.Add(new UserOrganization
                {
                    UserId = user.Id,
                    OrganizationId = org.Id
                });
            }

            await context.SaveChangesAsync();

            Log.Information("**Configuration Application:** Initial data successfully imported from JSON files");
        }
    }
}
