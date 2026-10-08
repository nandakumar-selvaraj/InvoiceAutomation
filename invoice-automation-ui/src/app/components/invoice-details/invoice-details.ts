import { Component, inject, OnInit, signal } from '@angular/core';
import { InvoicesService } from '../../services/invoices-service';
import { InvoiceResponse } from '../../models/invoice-response';
import { InvoiceTable } from '../invoice-table/invoice-table';

@Component({
  imports: [InvoiceTable],
  selector: 'app-invoice-details',
  styleUrl: './invoice-details.scss',
  templateUrl: './invoice-details.html',
})
export class InvoiceDetails implements OnInit {
  private service = inject(InvoicesService);

  public invoices = signal<InvoiceResponse[] | null>(null);
  
  ngOnInit(): void {
    this.service.getInvoices().subscribe({
      next: (response) => {
        this.invoices.set(response);
      },
      error: (error) => {
        console.error('Error fetching invoices:', error);
      }
    });
  }

  exportToExcel(): void {
    this.service.exportInvoicesToExcel().subscribe({
      next: (blob: Blob) => {
        // 1. Create a dynamic anchor element link in memory
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        
        // 2. Format the filename matching the download payload intent
        const dateStamp = new Date().toISOString().split('T')[0];
        link.download = `Invoices_Export_${dateStamp}.xlsx`;
        
        // 3. Append to body, simulate a physical click, and clean up memory
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Excel Export Failed:', err);
        alert('An error occurred while generating the Excel spreadsheet.');
      }
    });
  }
}
