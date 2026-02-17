# setup

1. docker compose run --rm inventree-server invoke update
2. docker compose up -d

https://docs.inventree.org/en/1.1.x/start/docker_install/#update-images

http://inventree.localhost/

dotnet ef dbcontext scaffold "Host=localhost;Port=5433;Database=initexample;Username=postgres;Password=initexample" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --context DbContext --force