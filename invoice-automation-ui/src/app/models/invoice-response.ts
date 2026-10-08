import { InvoiceItem } from "./invoice-item";

export interface InvoiceResponse {
    invoiceNumber?: string;
    customer?: string;
    invoiceDate?: string;
    currency?: string;
    subtotal?: number;
    tax?: number;
    total?: number;
    items?: InvoiceItem[];
}
