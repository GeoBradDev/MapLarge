# MapLarge Test Project – Brad Stricherz

## Overview

This project implements a minimalist file browser using **C# (ASP.NET Core)** for the backend and **vanilla JavaScript** for the frontend. It supports browsing, uploading, downloading, and deleting files and folders from a configurable home directory, with a fully client-rendered, deep-linkable UI.

---

## Requirements Met

-  End-to-end Web API + SPA
-  Deep-linkable JavaScript UI (via URL hash)
-  Browse, upload, download, and delete files and folders
-  JSON-only API responses (no server-rendered HTML)
-  Fully client-side rendering with DOM updates via JavaScript
-  Minimal boilerplate and framework usage

---

##  How to Run

1. **Build the project**:
   ```bash
   dotnet build
````

2. **Run the server**:

   ```bash
   dotnet run --project MapLarge/MapLarge.csproj
   ```

3. **Open your browser** and navigate to:

   ```
   http://localhost:5120/
   ```

---

## ⚙️ Configuration

* The home directory is configurable via an environment variable:

  ```bash
  export HOME_DIR=/path/to/your/folder
  ```

  Defaults to `~/Downloads` if not set.

---

##  Features

*  Browse nested folders with clickable navigation
*  Filter visible files and folders using a search box
* ️ Upload files via drag-and-drop or file picker
* ️ Download individual files with a click
*  Delete files and folders (recursively)

---

## File Structure

* `BrowseController.cs`: Exposes RESTful endpoints for browsing, uploading, downloading, and deleting files
* `index.html`: Minimal HTML scaffold with no server-side rendering
* `script.js`: Handles all UI rendering, filtering, and interaction logic
* `style.css`: Lightweight optional styling for usability

---

## Development Notes

All code was written and tested by me. I used standard developer tools and documentation, as well as LLM-based assistance for productivity (e.g., refining syntax, brainstorming edge cases, and cleaning up layout logic). All architectural and design decisions were my own.

This project reflects how I typically approach real-world tasks:

* Minimalist but complete implementation
* Clean separation of concerns
* Extensible, understandable code
* Built to work and be easily discussed in follow-up

---

## Security Note

This demo app exposes raw file system access and does not perform authentication or path sanitization. In a production environment, strong validation and access controls must be added.


