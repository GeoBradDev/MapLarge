# MapLarge Test Project – Brad Stricherz

## Overview

This project implements a minimalist file browser using **C# (ASP.NET Core)** for the backend and **vanilla JavaScript** for the frontend. It supports browsing, uploading, downloading, and deleting files and folders from a configurable home directory, with a fully client-rendered, deep-linkable UI.

---

## ✅ Requirements Met

- End-to-end Web API + SPA
- Deep-linkable JavaScript UI (via URL hash)
- Browse, upload, download, and delete files and folders
- JSON-only API responses (no server-rendered HTML)
- Fully client-side rendering with DOM updates via JavaScript
- Minimal boilerplate and framework usage

---

## 🚀 Live Demo

👉 [https://maplarge.onrender.com/](https://maplarge.onrender.com/)

The application is deployed using a containerized service on [Render.com](https://render.com).

---

## ⚙️ Configuration

* The home directory is configurable via an environment variable:
  ```bash
  export HOME_DIR=/path/to/your/folder
````

If not set, it defaults to `~/Downloads`.

---

## 🛠️ How to Run Locally

1. **Build the project:**

   ```bash
   dotnet build
   ```

2. **Run the server:**

   ```bash
   dotnet run --project MapLarge/MapLarge.csproj
   ```

3. **Open your browser:**

   ```
   http://localhost:5120/
   ```

---

## 📦 Containerization (Docker)

This project is fully containerized using a `Dockerfile` with multi-stage build:

```Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish MapLarge/MapLarge.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MapLarge.dll"]
```

---

## 📄 Render Deployment (`render.yaml`)

This project is deployed using a [Render](https://render.com) blueprint file:

```yaml
services:
  - type: web
    name: maplarge-file-browser
    runtime: docker
    dockerfilePath: Dockerfile
    envVars:
      - key: HOME_DIR
        value: /var/data
    plan: free
```

To deploy:

1. Commit `Dockerfile` and `render.yaml` to the project root.
2. Push the project to GitHub.
3. Visit [Render Dashboard](https://dashboard.render.com/) → **New Blueprint** → Select your repo.
4. Render will build and deploy the containerized app.

---

## ✨ Features

* Browse nested folders with clickable navigation
* Filter visible files and folders using a search box
* Upload files via file picker
* Download individual files
* Delete files and folders (recursively)

---

## 🗂️ File Structure

* `BrowseController.cs`: RESTful API for directory browsing, upload, download, delete
* `index.html`: Plain HTML UI shell
* `script.js`: Pure JS logic for rendering, filtering, navigation
* `style.css`: Minimal CSS for usability
* `Dockerfile`: Multi-stage container build
* `render.yaml`: Render blueprint configuration

---

## 🧑‍💻 Development Notes

All code was written and tested by me. I used standard developer tools and documentation, as well as LLM-based assistance for productivity (e.g., refining syntax, brainstorming edge cases, and cleaning up layout logic). All architectural and design decisions were my own.

This project reflects how I typically approach real-world tasks:

* Minimalist but complete implementation
* Clean separation of concerns
* Extensible, understandable code
* Built to work and be easily discussed in follow-up

---

## ⚠️ Security Note

This demo app exposes raw file system access and does not perform authentication or input sanitization. In a production environment, strong validation, sandboxing, and access controls must be implemented.

