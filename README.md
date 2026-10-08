# Invoice Automation

An AI-powered full-stack application that automates invoice data extraction from PDF documents and converts unstructured invoice information into structured, manageable data.

## Overview

Invoice Automation allows users to upload invoice PDFs, automatically extract important invoice information using AI, store the results in a database, manage invoice records, and export the data to Excel.

This project demonstrates how AI can be integrated into a real-world business workflow to reduce manual invoice data entry and processing.

### What it does

* Upload invoice PDFs through a web interface
* Extract invoice information using OpenAI
* Convert unstructured invoice content into structured data
* Store invoice records in SQLite
* View and manage invoices through an Angular dashboard
* Create, update, and delete invoice records
* Export invoice data to Excel

## Demo

### Invoice Dashboard

<img width="1903" height="937" alt="image" src="https://github.com/user-attachments/assets/fc5e74a3-5032-4c7f-97d1-1f3d85f6f662" />


### Invoice Extraction

<img width="1910" height="949" alt="image" src="https://github.com/user-attachments/assets/5211b9db-c5ce-4bd1-86f0-f0a6c79356ea" />

<img width="1564" height="925" alt="image" src="https://github.com/user-attachments/assets/0b2d06a6-77c8-4e02-bb0e-220fe50fcc6a" />

### Excel Export

<img width="1509" height="785" alt="image" src="https://github.com/user-attachments/assets/87a4e00b-033f-4214-97ee-3bcdfe27e5fc" />

<img width="1063" height="857" alt="image" src="https://github.com/user-attachments/assets/e1cf98e1-300d-43d5-a2a5-9ca32139a4fc" />

## How It Works

```text
Invoice PDF
     │
     ▼
Angular Web UI
     │
     ▼
ASP.NET Core API
     │
     ▼
OpenAI Invoice Extraction
     │
     ▼
Structured Invoice Data
     │
     ▼
SQLite Database
     │
     ├── View / Manage
     │
     └── Export to Excel
```

## Extracted Invoice Fields

The application extracts structured information including:

* Invoice Number
* Customer Name
* Invoice Date
* Currency
* Line Items

  * Description
  * Quantity
  * Unit Price
  * Amount
* Subtotal
* Tax
* Total

## Features

### AI-Powered Invoice Extraction

Upload a PDF invoice and automatically extract structured invoice information using an OpenAI-powered service.

### Invoice Management

The application supports standard CRUD operations for invoice records:

* Create invoice
* View invoices
* Update invoice
* Delete invoice
* Bulk create invoices

### Excel Export

Export invoice records into an Excel file for further processing, reporting, or accounting workflows.

### Web Dashboard

A modern Angular-based interface provides an easy way to upload, view, and manage invoice information.

## Tech Stack

### Backend

* ASP.NET Core 8
* C#
* Entity Framework Core
* SQLite
* OpenAI .NET SDK
* Swagger / OpenAPI

### Frontend

* Angular 22
* TypeScript
* Bootstrap 5
* RxJS

## Repository Structure

```text
InvoiceAutomation/
│
├── invoice-automation-api/
│   ├── Controllers/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   ├── appsettings.json
│   ├── Program.cs
│   └── InvoiceAutomationApi.csproj
│
├── invoice-automation-ui/
│   ├── src/
│   ├── package.json
│   ├── angular.json
│   └── README.md
│
└── README.md
```

## Prerequisites

Before running the application, make sure you have:

* .NET 8 SDK
* Node.js 18+ and npm
* OpenAI API key

## Configuration

The backend uses .NET User Secrets to securely store the OpenAI API key.

Initialize User Secrets:

```bash
cd invoice-automation-api

dotnet user-secrets init

dotnet user-secrets set "OpenAI:ApiKey" "<your-openai-api-key>"

dotnet user-secrets set "OpenAI:Model" "<your-openai-model>"
```

**Do not commit your OpenAI API key to GitHub.**

## Running the API

Navigate to the API project:

```bash
cd invoice-automation-api

dotnet restore

dotnet run
```

Swagger will be available at:

```text
http://localhost:5121/swagger
```

## Running the Frontend

Open a second terminal:

```bash
cd invoice-automation-ui

npm install

npm start
```

The Angular application will be available at:

```text
http://localhost:4200
```

## API Endpoints

| Method | Endpoint                        | Description                         |
| ------ | ------------------------------- | ----------------------------------- |
| POST   | `/api/Invoices/extract`         | Upload PDF and extract invoice data |
| POST   | `/api/Invoices`                 | Create an invoice                   |
| GET    | `/api/Invoices`                 | Get all invoices                    |
| GET    | `/api/Invoices/{invoiceNumber}` | Get a specific invoice              |
| PUT    | `/api/Invoices/{invoiceNumber}` | Update an invoice                   |
| DELETE | `/api/Invoices/{invoiceNumber}` | Delete an invoice                   |
| POST   | `/api/Invoices/bulk`            | Create multiple invoices            |
| POST   | `/api/Invoices/export`          | Export invoice data to Excel        |

## Database

The application uses SQLite for local data storage.

Database file:

```text
invoice-automation-api/invoices.db
```

Entity Framework Core migrations are used to create and update the database schema.

## Configuration Notes

* The frontend API base URL is configured through the Angular environment configuration.
* If the API runs on a different port, update the frontend environment configuration accordingly.
* The project is currently intended for local development and demonstration purposes.

## Future Improvements

Possible future enhancements include:

* Support for multiple invoice formats
* Batch PDF processing
* Invoice validation
* Duplicate invoice detection
* Cloud database integration
* Authentication and authorization
* Cloud deployment
* Automated invoice processing workflows

## Purpose

This project was built as a practical demonstration of integrating AI into a business automation workflow using modern web technologies.

It demonstrates experience with:

* AI API integration
* Document data extraction
* REST API development
* Angular application development
* Database persistence
* CRUD operations
* Excel generation
* Full-stack application architecture

## License

This project is currently unlicensed.
