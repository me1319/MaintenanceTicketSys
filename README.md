 Ticket Management API Collection
 This repository contains the Postman collection for managing and updating ticket statuses.

 To Run This Repo
 1-clone repo
 2- update connectionstring according to your data(appsettings.json)
 3-Startup project name (Web)
 4-update-database

postman

this file exsists beside readme file
Maintenance Ticket System.postman_collection.json

## 📌 Endpoints

# Maintenance Ticket System API Documentation

This Postman collection is designed to manage the lifecycle of maintenance tickets, including creation, assignment, status updates, and attachments.

## ⚙️ Configuration

- **Base URL:** `http://localhost:5181` (or use the `{{baseURL}}` variable in Postman).
- **Format:** All requests and responses use `application/json`.

---

## 📌 API Endpoints

### 1. Tickets Management

#### **Get All Tickets**
Retrieves a list of all maintenance tickets.
* **Method:** `GET`
* **URL:** `{{baseURL}}/api/tickets`

#### **Get Ticket by ID**
Retrieves detailed information for a specific ticket.
* **Method:** `GET`
* **URL:** `{{baseURL}}/api/tickets/{id}`
* **Path Parameter:** `id` (The unique ID of the ticket).

#### **Add New Ticket**
Creates a new maintenance ticket in the system.
* **Method:** `POST`
* **URL:** `{{baseURL}}/api/tickets`
* **Request Body (JSON):**
    ```json
    {
      "title": "First Ticket",
      "description": "printer not working",
      "priority": 1
    }
    ```

---

### 2. Status & Assignments

#### **Assign Ticket Status**
Updates the current status of a specific ticket.
* **Method:** `PUT`
* **URL:** `{{baseURL}}/api/Tickets/{id}/status`
* **Path Parameter:** `id` (Ticket ID).
* **Query Parameter:** `status` (Integer representing the new state).

#### **Assign to Engineer**
Assigns a specific ticket to an engineer.
* **Method:** `PUT`
* **URL:** `{{baseURL}}/api/tickets/{id}/assign`
* **Path Parameter:** `id` (Ticket ID).
* **Query Parameter:** `engineerId` (The ID of the engineer).

---

### 3. Comments & Attachments

#### **Add Comment**
Adds a text comment to a specific ticket.
* **Method:** `POST`
* **URL:** `{{baseURL}}/api/tickets/{id}/comments`
* **Request Body (Raw Text):** `"Add comment to ticket here"`

#### **Add Attachments**
Links a file attachment to a ticket.
* **Method:** `POST`
* **URL:** `{{baseURL}}/api/tickets/{id}/attachments`
* **Request Body (JSON):**
    ```json
    {
      "fileName": "placeholder.txt",
      "fileType": "text/plain",
      "filePath": "/path/to/placeholder.txt"
    }
    ```

---

## 🛠 How to Test
1. Import the **Postman Collection** JSON file.
2. Ensure your local server is running at `localhost:5181`.
3. Use the **Add Ticket** endpoint first to generate a ticket ID for further testing.

 
