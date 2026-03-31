# 🐳 Docker Setup za BHASCoreProject

Kompletan Docker setup za pokretanje ASP.NET Core Razor Pages aplikacije sa SQL Server-om u Docker kontejnerima.

## 📋 Preduslovi

- **Docker Desktop** instaliran i pokrenut
- **Windows 10/11** sa WSL2
- **.NET 10 SDK** (za lokalni development)

## 🚀 Brzo Pokretanje

```powershell
# 1. Clone repository
git clone https://github.com/codexBA/BHASCoreProject
cd BHASCoreProject

# 2. Pokrenite Docker kontejnere
docker-compose up -d --build

# 3. Pratite logove (opciono)
docker-compose logs -f

# 4. Pristupite aplikaciji
# Web: http://localhost:5100
# SQL Server: localhost,14333
```

## 📁 Struktura Fajlova

```
BHAS-PROJEKAT-CLASS/
├── docker-compose.yml              # Glavna konfiguracija
├── .dockerignore                   # Build optimizacija
├── BHASCoreProject/
│   ├── Dockerfile                  # Build instrukcije
│   ├── DockerInstrukcije.html      # 📘 Detaljna dokumentacija
│   └── appsettings.Production.json # Docker konfiguracija
```

## 🔧 Konfiguracija

### Portovi

| Servis | Windows Port | Container Port | Pristup |
|--------|--------------|----------------|---------|
| Web App | **5100** | 8080 | `http://localhost:5100` |
| SQL Server | **14333** | 1433 | `localhost,14333` |

**Zašto custom portovi?**
- **14333** - Izbjegava konflikt sa lokalnim SQL Server-om (1433)
- **5100** - Izbjegava konflikt sa IIS-om i drugim web serverima (8080)

### Connection Strings

```yaml
# U docker-compose.yml environment varijablama:
ConnectionStrings__IdentityConnection: "Server=bhas-sqlserver,1433;Database=BHASIdentity;..."
ConnectionStrings__BusinessConnection: "Server=bhas-sqlserver,1433;Database=BHASBusiness;..."
```

**Ključno:** Koristi se `bhas-sqlserver` (ime kontejnera) kao hostname, **NE** `localhost`!

## 💾 Docker Volumes - Gdje se Podaci Čuvaju?

### Šta su Volumes?

Docker volumes su **virtuelni disk-ovi** koje Docker upravlja. Podaci ostaju sačuvani čak i nakon što obrišete kontejnere!

```yaml
volumes:
  bhas-sqldata:           # Ime volume-a
    name: bhas-sqldata

services:
  bhas-sqlserver:
    volumes:
      - bhas-sqldata:/var/opt/mssql  # SQL podaci se čuvaju ovdje
```

### Fizička Lokacija na Windows-u

```
C:\Users\[VašeKorisničkoIme]\AppData\Local\Docker\wsl\data\

Ili u WSL2:
\\wsl$\docker-desktop-data\data\docker\volumes\bhas-sqldata\_data\
```

### Komande za Upravljanje Volumes

```powershell
# Pregled svih volumes
docker volume ls

# Detalji o volume-u
docker volume inspect bhas-sqldata

# Lokacija na disku
docker volume inspect bhas-sqldata --format '{{ .Mountpoint }}'

# OPREZ: Brisanje volume-a (briše SVE podatke!)
docker volume rm bhas-sqldata
```

## 📊 YAML Struktura - docker-compose.yml

```yaml
services:           # Lista kontejnera
  service-name:     # Ime servisa
    image:          # Docker image
    container_name: # Ime kontejnera
    ports:          # Port mapping (HOST:CONTAINER)
      - "5100:8080" # Windows port 5100 -> Container port 8080
    environment:    # Environment varijable
      KEY: "value"
    volumes:        # Disk storage
      - volume-name:/path/in/container
    networks:       # Mreže za komunikaciju
      - network-name
    depends_on:     # Redoslijed pokretanja
      other-service:
        condition: service_healthy

volumes:            # Definicija named volumes
  volume-name:

networks:           # Definicija networks
  network-name:
    driver: bridge
```

## 🌐 Docker Network - Kako Kontejneri Komuniciraju

```
┌──────────────────────────────────────────┐
│ Windows Host                             │
│  http://localhost:5100 ────────┐         │
│  localhost,14333 ──────────────┼───┐     │
└────────────────────────────────┼───┼─────┘
                                 │   │
      ┌──────────────────────────┼───┼─────┐
      │ Docker Network           ▼   ▼     │
      │  ┌──────────┐      ┌──────────┐    │
      │  │ Web App  │◄─────┤ SQL      │    │
      │  │ :8080    │      │ Server   │    │
      │  └──────────┘      │ :1433    │    │
      │                    └────┬─────┘    │
      └─────────────────────────┼──────────┘
                                │
                                ▼
                      ┌──────────────────┐
                      │ bhas-sqldata     │
                      │ (Volume)         │
                      └──────────────────┘
```

Web aplikacija koristi **ime kontejnera** kao hostname:
- ✅ `Server=bhas-sqlserver,1433` - **Ispravno** (Docker DNS)
- ❌ `Server=localhost,14333` - **Pogrešno** (to je za Windows host)

## 📝 Najčešće Komande

| Akcija | Komanda |
|--------|---------|
| Pokretanje (prvi put) | `docker-compose up -d --build` |
| Pokretanje | `docker-compose up -d` |
| Zaustavljanje | `docker-compose down` |
| Restart | `docker-compose restart` |
| Logovi | `docker-compose logs -f` |
| Status | `docker-compose ps` |
| **OPREZ: Brisanje sa podacima** | `docker-compose down -v` |

## 💾 Backup i Restore

### Backup SQL Podataka

```powershell
# Backup baza
docker exec bhas-sqlserver /opt/mssql-tools18/bin/sqlcmd `
  -S localhost -U sa -P "BhasAdmin123!" -C `
  -Q "BACKUP DATABASE BHASIdentity TO DISK = '/var/opt/mssql/data/BHASIdentity.bak'"

# Kopiraj na Windows
docker cp bhas-sqlserver:/var/opt/mssql/data/BHASIdentity.bak ./backup/
```

### Restore SQL Podataka

```powershell
# Kopiraj u kontejner
docker cp ./backup/BHASIdentity.bak bhas-sqlserver:/var/opt/mssql/data/

# Restore
docker exec bhas-sqlserver /opt/mssql-tools18/bin/sqlcmd `
  -S localhost -U sa -P "BhasAdmin123!" -C `
  -Q "RESTORE DATABASE BHASIdentity FROM DISK = '/var/opt/mssql/data/BHASIdentity.bak' WITH REPLACE"
```

## 🔍 Troubleshooting

### Problem: "Port already in use"

```powershell
# Promijenite portove u docker-compose.yml
ports:
  - "9090:8080"   # Umjesto 5100
  - "15000:1433"  # Umjesto 14333
```

### Problem: "Cannot connect to SQL Server"

```powershell
# Provjerite da li kontejner radi
docker ps

# Provjerite logove
docker logs bhas-sqlserver

# Provjerite network
docker network inspect bhas-network
```

### Problem: "HTTPS certificate error"

✅ **Rješeno** - HTTPS je onemogućen u Docker konfiguraciji za development.

## 📚 Dodatna Dokumentacija

Za **detaljne instrukcije na bosanskom jeziku**, otvorite:

```
BHASCoreProject/DockerInstrukcije.html
```

Sadrži:
- 3 scenarija dockerizacije (lokalna baza, Docker SQL, Docker Compose)
- Detaljno objašnjenje YAML strukture
- Volumes i network-i
- Troubleshooting
- Napredne konfiguracije (HTTPS, backup/restore)

## 🎓 Za Studente

1. **Clone** repo
2. **Otvorite** `DockerInstrukcije.html` u browseru
3. **Pratite** korake za Scenario 3 (Docker Compose)
4. **Pokrenite**: `docker-compose up -d --build`
5. **Pristupite**: `http://localhost:5100`

## 📧 Kontakt

Za pitanja i podršku:
- GitHub Issues: https://github.com/codexBA/BHASCoreProject/issues
- Email: [vaš email]

---

**© 2026 BHAS Core Project | .NET 10 | Docker**
