# Invoice Automation

An AI-powered full-stack invoice processing application that extracts structured invoice information from PDF documents, allows users to review the extracted data, stores invoice records in a SQLite database, and exports invoice data to Excel.

## Overview

Invoice Automation is designed to demonstrate a real-world business automation workflow for processing invoices.

Instead of manually reading invoice PDFs and entering the information into a system, users can upload an invoice PDF and use AI to extract the invoice information automatically.

The application provides:

* PDF invoice upload
* AI-powered invoice data extraction
* Extracted data review
* Invoice data persistence using SQLite
* Invoice management
* Excel export
* Angular-based dashboard
* ASP.NET Core REST API

## Application Workflow

```text
Invoice PDF
     │
     ▼
Angular Dashboard
     │
     ▼
Extract Invoice
     │
     ▼
OpenAI-powered extraction
     │
     ▼
Extracted Invoice Details
     │
     ├── Review invoice information
     │
     └── Review line items
     │
     ▼
Save
     │
     ▼
SQLite Database
     │
     ▼
Invoices Page
     │
     ▼
Export to Excel
```

# Pages & Functionality

## 1. Dashboard

The Dashboard is used to upload and process invoice PDFs.

### Upload and Extract Invoice

Users can select an invoice PDF using the **Browse** button.

After selecting the PDF, clicking the **Extract Invoice** button sends the invoice to the backend API for AI-powered extraction.

The application extracts structured information from the invoice.

### Extracted Invoice Details

The extracted invoice information is displayed in a table.

The table contains the following invoice-level fields:

| Field          | Description                          |
| -------------- | ------------------------------------ |
| Invoice Number | Unique invoice number                |
| Customer       | Customer name                        |
| Invoice Date   | Invoice date                         |
| Currency       | Invoice currency                     |
| Subtotal       | Invoice subtotal                     |
| Tax            | Tax amount                           |
| Total          | Total invoice amount                 |
| Items          | Number/details of invoice line items |

Each invoice row contains an **expand (+)** option.

Clicking the expand button displays the individual invoice line items.

### Invoice Items

The expanded section displays the following information for each invoice item:

| Field       | Description                    |
| ----------- | ------------------------------ |
| Description | Product or service description |
| Quantity    | Quantity of the item           |
| Unit Price  | Price per unit                 |
| Amount      | Total amount for the item      |

This allows users to review the complete invoice information before saving it.

### Save Invoice

After reviewing the extracted information, users can click the **Save** button.

The extracted invoice information is then sent to the backend API and stored in the **SQLite database**.

The saved data includes:

* Invoice information
* Invoice line items
* Quantity
* Unit price
* Amount
* Subtotal
* Tax
* Total

---

## 2. Invoices

The **Invoices** page displays invoice records that have already been saved in the SQLite database.

### Invoice List

All saved invoice records are retrieved from the backend API and displayed in a table.

The table contains:

| Field          | Description          |
| -------------- | -------------------- |
| Invoice Number | Invoice number       |
| Customer       | Customer name        |
| Invoice Date   | Invoice date         |
| Currency       | Invoice currency     |
| Subtotal       | Invoice subtotal     |
| Tax            | Tax amount           |
| Total          | Total invoice amount |
| Items          | Invoice line items   |

Each invoice row contains an **expand (+)** option.

Clicking the expand button displays the invoice's line items:

| Field       | Description                    |
| ----------- | ------------------------------ |
| Description | Product or service description |
| Quantity    | Quantity                       |
| Unit Price  | Unit price                     |
| Amount      | Line item amount               |

### Export to Excel

The **Export to Excel** button allows users to export the invoice records into an Excel file.

This provides a convenient way to use the processed invoice information for:

* Reporting
* Accounting
* Data analysis
* Further business processing

# Features

## AI-Powered Invoice Extraction

Automatically extracts structured invoice information from PDF documents using an OpenAI-powered extraction service.

## Invoice Data Extraction

The application extracts:

* Invoice Number
* Customer Name
* Invoice Date
* Currency
* Line Items
* Quantity
* Unit Price
* Amount
* Subtotal
* Tax
* Total

## Invoice Review

Users can review the extracted invoice information and individual line items before saving the invoice.

## SQLite Database

Extracted invoice information is persisted in a **SQLite database**.

Entity Framework Core is used for database access and migrations.

## Invoice Management

The backend provides APIs for managing invoice records, including:

* Create invoice
* Retrieve invoices
* Retrieve a specific invoice
* Update invoice
* Delete invoice
* Bulk invoice creation

## Excel Export

Saved invoice information can be exported to an Excel file for further processing and reporting.

## Interactive Invoice Tables

Invoice-level information is displayed in expandable rows, allowing users to view line-item details without making the main table unnecessarily large.

# Screenshots

Add screenshots of the application here.

### Dashboard

<img width="1312" height="506" alt="Screenshot 2026-10-08 193803" src="https://github.com/user-attachments/assets/1e0734d3-6e15-4220-8490-e736082fffcc" />


### Invoice Extraction

<img width="1246" height="575" alt="image" src="https://github.com/user-attachments/assets/4706ac56-1c55-4a08-9dbd-809cbccf0eae" />


### Expanded Invoice Items

<img width="1234" height="862" alt="image" src="https://github.com/user-attachments/assets/3ccfed61-e953-4cb8-a684-abd104537baa" />


### Invoices Page

<img width="1209" height="762" alt="image" src="https://github.com/user-attachments/assets/6a00c56d-0911-4ac1-9edb-d1fa73592e98" />


### Excel Export

<img width="1063" height="857" alt="Screenshot 2026-10-08 192128" src="https://github.com/user-attachments/assets/af836c1e-89f8-466a-b61e-30ccd7298021" />


# Architecture

```text
┌──────────────────────────────┐
│        Angular 22 UI         │
│                              │
│  Dashboard                   │
│  Invoice Extraction          │
│  Invoice Management          │
│  Excel Export                │
└──────────────┬───────────────┘
               │
               │ HTTP / REST API
               ▼
┌──────────────────────────────┐
│     ASP.NET Core 8 API       │
│                              │
│ Controllers                  │
│ Services                     │
│ Repositories                 │
│ Models                       │
└──────────────┬───────────────┘
               │
       ┌───────┴────────┐
       │                │
       ▼                ▼
┌─────────────┐  ┌──────────────────┐
│   OpenAI    │  │ Entity Framework │
│             │  │      Core        │
│ AI Invoice  │  └────────┬─────────┘
│ Extraction  │           │
└─────────────┘           ▼
                    ┌──────────────┐
                    │    SQLite    │
                    │   Database   │
                    └──────────────┘
```

# Tech Stack

## Backend

* ASP.NET Core 8
* C#
* Entity Framework Core
* SQLite
* OpenAI .NET SDK
* Swagger / OpenAPI

## Frontend

* Angular 22
* TypeScript
* Bootstrap 5
* RxJS

## Data & Export

* SQLite
* Entity Framework Core
* Excel export

# Repository Structure

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

# Prerequisites

Before running the application, make sure the following are installed:

* .NET 8 SDK
* Node.js 18+ and npm
* An OpenAI API key

# Configuration

The backend uses .NET User Secrets to securely store the OpenAI API key.

Navigate to the API project:

```bash
cd invoice-automation-api
```

Initialize User Secrets:

```bash
dotnet user-secrets init
```

Configure the OpenAI API key:

```bash
dotnet user-secrets set "OpenAI:ApiKey" "<your-openai-api-key>"
```

Configure the OpenAI model:

```bash
dotnet user-secrets set "OpenAI:Model" "<your-openai-model>"
```

**Important:** Never commit your OpenAI API key to GitHub.

# Running the Backend API

Navigate to the API project:

```bash
cd invoice-automation-api
```

Restore the required packages:

```bash
dotnet restore
```

Run the API:

```bash
dotnet run
```

Swagger will be available at:

```text
http://localhost:5121/swagger
```

# Running the Angular Frontend

Open a second terminal and navigate to the UI project:

```bash
cd invoice-automation-ui
```

Install the required npm packages:

```bash
npm install
```

Start the Angular application:

```bash
npm start
```

The application will be available at:

```text
http://localhost:4200
```

# API Endpoints

The backend exposes the following main endpoints:

| Method | Endpoint                        | Description                                |
| ------ | ------------------------------- | ------------------------------------------ |
| POST   | `/api/Invoices/extract`         | Upload PDF and extract invoice information |
| POST   | `/api/Invoices`                 | Create a single invoice                    |
| GET    | `/api/Invoices`                 | Retrieve all invoices                      |
| GET    | `/api/Invoices/{invoiceNumber}` | Retrieve a specific invoice                |
| PUT    | `/api/Invoices/{invoiceNumber}` | Update an invoice                          |
| DELETE | `/api/Invoices/{invoiceNumber}` | Delete an invoice                          |
| POST   | `/api/Invoices/bulk`            | Create multiple invoices                   |
| POST   | `/api/Invoices/export`          | Export invoice data to Excel               |

# Database

The application uses **SQLite as the database** for storing invoice information.

The database file is:

```text
invoice-automation-api/invoices.db
```

Entity Framework Core is used as the ORM for database operations.

Entity Framework Core migrations are used to create and maintain the database schema.

The database stores structured invoice information including:

* Invoice Number
* Customer
* Invoice Date
* Currency
* Invoice Items
* Quantity
* Unit Price
* Amount
* Subtotal
* Tax
* Total

# Data Model

The application works with two primary data structures.

### InvoiceResponse

```text
InvoiceResponse
├── InvoiceNumber
├── Customer
├── InvoiceDate
├── Currency
├── Subtotal
├── Tax
├── Total
└── Items
```

### InvoiceItem

```text
InvoiceItem
├── Description
├── Quantity
├── UnitPrice
└── Amount
```

# End-to-End Example

A typical invoice processing workflow looks like this:

### Step 1 — Upload

The user opens the Dashboard and selects an invoice PDF using the **Browse** button.

### Step 2 — Extract

The user clicks **Extract Invoice**.

The PDF is sent to the ASP.NET Core API, where the OpenAI-powered extraction service processes the invoice.

### Step 3 — Review

The extracted invoice information is displayed in the Dashboard.

The user can expand the invoice row to review individual line items.

### Step 4 — Save

The user clicks **Save**.

The invoice and its line items are stored in the SQLite database.

### Step 5 — View

The user navigates to the **Invoices** page.

Saved invoice records are retrieved from SQLite and displayed in the invoice table.

### Step 6 — Export

The user clicks **Export to Excel** to generate an Excel file containing the invoice information.

# Configuration Notes

* The frontend API base URL is configured through the Angular environment configuration.
* If the backend API runs on a different port, update the frontend environment configuration accordingly.
* SQLite is used for local development and demonstration purposes.
* The project currently does not include production authentication or authorization.

# Future Improvements

Potential future enhancements include:

* Batch invoice PDF processing
* Multiple invoice format support
* Invoice validation
* Duplicate invoice detection
* Authentication and authorization
* Cloud database integration
* Cloud deployment
* Automated invoice processing workflows
* Improved extraction validation
* Support for additional document types

# Project Purpose

This project was created as a practical demonstration of applying AI to a real-world business automation workflow.

It demonstrates full-stack development skills including:

* AI API integration
* PDF document processing
* Structured data extraction
* REST API development
* Angular application development
* SQLite database integration
* Entity Framework Core
* CRUD operations
* Excel export
* Full-stack application architecture

# License

This project is currently unlicensed unless a specific license file and terms are added.
