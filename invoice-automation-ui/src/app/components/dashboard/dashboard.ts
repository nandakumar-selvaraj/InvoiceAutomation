import { Component, inject, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { InvoicesService } from '../../services/invoices-service';
import { InvoiceResponse } from '../../models/invoice-response';
import { InvoiceTable } from '../invoice-table/invoice-table';
@Component({
  imports: [DecimalPipe, InvoiceTable],
  selector: 'app-dashboard',
  styleUrl: './dashboard.scss',
  templateUrl: './dashboard.html',
})
export class Dashboard {
  private service = inject(InvoicesService);

  public invoices = signal<InvoiceResponse[] | null>(null);

  isDragging = false;
  selectedFile: File | null = null;

  onFileSelected(event: Event): void {
    this.selectedFile = null;
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.processFile(input.files);
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      this.processFile(event.dataTransfer.files);
    }
  }

  private processFile(files: FileList): void {
    const file = files[0];
    const isPdfType = file.type === 'application/pdf';
    const isPdfExtension = file.name.toLowerCase().endsWith('.pdf');

    if (isPdfType || isPdfExtension) {
      this.selectedFile = file;
    } else {
      alert('Invalid file format. Please upload PDF files only.');
      this.selectedFile = null;
    }
  }

  clearFile(event: Event): void {
    event.stopPropagation();
    this.selectedFile = null;
  }

  extractInvoice(): void {
    if (this.selectedFile) {
      this.service.uploadInvoice(this.selectedFile).subscribe({
        next: (response) => {
          this.invoices.update((currentInvoices) => {
            if (currentInvoices) {
              return [...currentInvoices, response];
            }
            return [response];
          });
          this.selectedFile = null;
          alert('Invoice extraction successful!');
        },
        error: (error) => {
          console.error('Error during invoice extraction:', error);
          alert('Error occurred while extracting invoice.');
        }
      });
    }
  }

  // Add this method to your Dashboard class
  removeInvoice(invoiceNumber?: string): void {
    this.invoices.update((currentInvoices) => {
      if (!currentInvoices) return null;
      const updated = currentInvoices.filter((inv) => inv.invoiceNumber !== invoiceNumber);
      return updated.length > 0 ? updated : null;
    });
  }

  // API Call to Save extracted data to DB
  saveInvoices(): void {
    const currentInvoices = this.invoices();
    if (!currentInvoices || currentInvoices.length === 0) return;

    this.service.saveInvoices(currentInvoices).subscribe({
      next: () => {
        alert('Invoices saved successfully to the database!');
      },
      error: (error) => {
        console.error('Error saving invoices to DB:', error);
        alert('Failed to save invoices to the database.');
      }
    });
  }
}
