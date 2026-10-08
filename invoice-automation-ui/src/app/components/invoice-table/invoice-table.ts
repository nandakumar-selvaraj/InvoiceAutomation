import { Component, input, signal, output } from '@angular/core';
import { InvoiceResponse } from '../../models/invoice-response';
import { CurrencyPipe } from '@angular/common';

@Component({
  imports: [CurrencyPipe],
  selector: 'app-invoice-table',
  styleUrl: './invoice-table.scss',
  templateUrl: './invoice-table.html',
})
export class InvoiceTable {
  invoices = input<InvoiceResponse[] | null>(null);

  showRemoveButton = input<boolean>(false);
  title = input<string>('');
  // Output event to notify parent of deletion
  invoiceRemoved = output<string>();

  private expandedInvoices = signal(new Set<string>());

  toggleInvoice(invoiceNumber?: string): void {
    if (!invoiceNumber) return;

    this.expandedInvoices.update((current) => {
      const next = new Set(current);
      if (next.has(invoiceNumber)) {
        next.delete(invoiceNumber);
      } else {
        next.add(invoiceNumber);
      }
      return next;
    });
  }

  isInvoiceExpanded(invoiceNumber?: string): boolean {
    if (!invoiceNumber) return false;
    return this.expandedInvoices().has(invoiceNumber);
  }

  // Handle remove action and prevent triggering row expansion
  onRemoveInvoice(event: Event, invoiceNumber?: string): void {
    event.stopPropagation(); // Prevents row toggle click event from triggering
    if (invoiceNumber) {
      this.invoiceRemoved.emit(invoiceNumber);
    }
  }
}