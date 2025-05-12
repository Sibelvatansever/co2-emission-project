# CO2 Emission Project

This project includes 3 APIs:

- **calculator-api** → http://localhost:8080  
- **emissions-api** → http://localhost:5286  
- **measurements-api** → http://localhost:5042

## How to run

```bash
docker compose up --build
```

Each service runs in its own container. Services communicate with each other using their Docker service names (like `measurements-api`, `emissions-api`).

## Notes

- Everything builds and runs cleanly.
- `localhost` replaced with Docker service names for internal API calls.
- Swagger is not added yet (can be done if needed).
