# Gadget Management by Clear Treasury
Gadget Management is an inventory management system for a Gadget Store.

## How to run
- Ensure a MS SQL Server instance with the full text search capability enabled
  - Set the DB connection string in appsettings.json/secrets.json
- Change the JWT signing key in appsettings.json/secrets.json if needed (already set for dev. env.)
- Change the CORS origins in appsettings.json if needed
- If running in dev. environment, DB migrations & data seeding will run automatically
  -	Otherwise, make sure the DB schema/identity data are ready before running (e.g. use Script-Migration)