# Authentication setup

The API uses 15-minute JWT access tokens and ASP.NET Core's `PasswordHasher<AppUser>`. Keep signing keys and database credentials out of committed settings.

For local development, configure user-secrets:

```sh
dotnet user-secrets init
dotnet user-secrets set "jwtConfig:Key" "<random-secret-at-least-32-bytes>"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<development-SQL-Server-connection-string>"
```

For hosting, use secret environment variables `jwtConfig__Key` and `ConnectionStrings__DefaultConnection`. The issuer/audience in `appsettings.json` must remain the same for token issue and validation. The token lifetime can be set to 1–60 minutes.

## Apply development migration

After inspecting the generated migration and confirming no duplicate normalized emails exist, apply it only to the intended development database:

```sh
dotnet ef database update
```

The migration adds a nullable account owner to existing bookings to preserve old records and adds account profile/email normalization columns. Existing bookings without an owner cannot be shown to a customer.

## Provision the first administrator

Public registration always creates a `Customer`; it does not accept a role. To provision an initial admin, configure these secrets and run the app once with the switch:

```sh
dotnet user-secrets set "AdminProvisioning:Email" "admin@example.com"
dotnet user-secrets set "AdminProvisioning:Password" "<strong-one-time-admin-password>"
dotnet run -- --provision-admin
```

The command adds the account only if its email is not already registered. Remove the admin provisioning secrets after use. If the email is already a customer, this command does not promote it; provision admin changes through a controlled database/admin operation.

## Requests

Register: `POST /api/auth/register`

```json
{ "name": "Example Customer", "email": "customer@example.com", "password": "<at-least-8-characters>" }
```

Login: `POST /api/auth/login`

```json
{ "email": "customer@example.com", "password": "<password>" }
```

Use the returned `accessToken` on protected requests:

```http
Authorization: Bearer <accessToken>
```

Customers can use `/api/account/me` and booking routes. Admin tokens can create/update movies, theatres, and shows. Browse endpoints remain public. Logout is client-side token disposal; without refresh/revocation storage, an issued token remains usable until its short expiry.
