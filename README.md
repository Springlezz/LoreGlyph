# LoreGlyph

Created by Springlezz (https://github.com/Springlezz)

## Setup

1. Install PostgreSQL
2. Create database:

CREATE DATABASE mydb;

3. Configure secrets:

dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=mydb;Username=postgres;Password=your_password"
dotnet user-secrets set "Jwt:Key" "your_super_secret_key_123"
dotnet user-secrets set "Jwt:Issuer" "LoreGlyph"
dotnet user-secrets set "Jwt:Audience" "LoreGlyphUsers"

4. Apply migrations:

dotnet ef database update

5. Run project

dotnet run

App runs on:
- https://localhost:7147 (if choose https)
- http://localhost:524
