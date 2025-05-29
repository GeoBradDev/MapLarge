let currentItems = [];

// Navigate to a path and update the URL hash
function goTo(path) {
    location.hash = encodeURIComponent(path);
}

// Filter and display the file/folder list
function renderFilteredList(path) {
    const filter = document.getElementById('filterBox').value.trim().toLowerCase();
    const list = document.getElementById('file-list');
    const summary = document.getElementById('summary');

    const filtered = currentItems.filter(item =>
        item.name.toLowerCase().includes(filter)
    );

    // Summary stats
    const fileCount = filtered.filter(i => i.type === 'file').length;
    const folderCount = filtered.filter(i => i.type === 'folder').length;
    const totalSize = filtered.reduce((sum, i) => sum + (i.size || 0), 0);

    summary.textContent = `${folderCount} folders, ${fileCount} files, ${totalSize} bytes`;
    list.innerHTML = '';

    // Add ".." to go up a directory
    if (path) {
        const parentPath = path.split('/').slice(0, -1).join('/');
        const upPath = parentPath || '';
        const upItem = document.createElement('li');
        upItem.textContent = '..';
        upItem.className = 'folder';
        upItem.onclick = () => goTo(upPath);
        list.appendChild(upItem);
    }

    // Render each file/folder
    for (const item of filtered) {
        const entry = document.createElement('li');
        entry.className = item.type;

        const label = document.createElement('span');
        label.textContent = `${item.name}${item.size != null ? ` (${item.size} bytes)` : ''}`;

        // Folder click navigation
        if (item.type === 'folder') {
            entry.onclick = () => goTo(path ? `${path}/${item.name}` : item.name);
        }

        // Delete button
        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = 'Delete';
        deleteBtn.className = 'delete-btn';
        deleteBtn.onclick = async (e) => {
            e.stopPropagation();
            if (!confirm(`Delete "${item.name}"?`)) return;
            const targetPath = path ? `${path}/${item.name}` : item.name;
            const res = await fetch(`/api/browse/delete/${encodeURIComponent(targetPath)}`, {
                method: 'DELETE'
            });

            if (res.ok) {
                fetchAndRender(path);
            } else {
                alert('Delete failed: ' + await res.text());
            }
        };

// Download button (added independently)
        if (item.type === 'file') {
            const downloadBtn = document.createElement('a');
            downloadBtn.textContent = 'Download';
            downloadBtn.href = `/api/browse/download/${encodeURIComponent(path ? `${path}/${item.name}` : item.name)}`;
            downloadBtn.className = 'download-link';
            downloadBtn.download = item.name;
            downloadBtn.onclick = (e) => e.stopPropagation(); // Prevent folder navigation
            entry.appendChild(downloadBtn);
        }

        entry.appendChild(label);
        entry.appendChild(deleteBtn);
        list.appendChild(entry);

    }
}

// Fetch directory listing and trigger rendering
async function fetchAndRender(path = '') {
    const response = await fetch(`/api/browse/${encodeURIComponent(path)}`);
    if (!response.ok) {
        document.getElementById('file-list').innerHTML = `<li style="color:red;">Failed to load folder: ${path}</li>`;
        document.getElementById('summary').textContent = '';
        return;
    }

    currentItems = await response.json();
    document.getElementById('path-title').textContent = `/${path}`;
    renderFilteredList(path);
}

// Initialize everything after DOM loads
window.addEventListener('DOMContentLoaded', () => {
    const path = decodeURIComponent(location.hash.slice(1));
    fetchAndRender(path);

    // Filter files/folders on keyup
    document.getElementById('filterBox').addEventListener('keyup', () => {
        renderFilteredList(decodeURIComponent(location.hash.slice(1)));
    });

    // Handle file upload
    document.getElementById('uploadForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const file = document.getElementById('fileInput').files[0];
        if (!file) return;

        const path = decodeURIComponent(location.hash.slice(1));
        const formData = new FormData();
        formData.append('file', file);

        const res = await fetch(`/api/browse/upload/${path}`, {
            method: 'POST',
            body: formData
        });

        if (res.ok) {
            alert('File uploaded successfully.');
            document.getElementById('fileInput').value = '';
            fetchAndRender(path);
        } else {
            alert('Upload failed: ' + await res.text());
        }
    });
});

// Re-fetch if user changes hash manually
window.addEventListener('hashchange', () => {
    const path = decodeURIComponent(location.hash.slice(1));
    fetchAndRender(path);
});
