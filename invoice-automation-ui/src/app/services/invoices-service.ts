import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { InvoiceResponse } from '../models/invoice-response';

@Injectable({
    providedIn: 'root',
})
export class InvoicesService {
    private http = inject(HttpClient);
    
    public getInvoices(): Observable<InvoiceResponse[]> {
        return this.http.get<InvoiceResponse[]>(environment.API_URL + '/Invoices');
    }

    public uploadInvoice(file: File): Observable<InvoiceResponse> {
        const formData = new FormData();
        formData.append('file', file);

        return this.http.post<InvoiceResponse>(environment.API_URL + '/Invoices/extract', formData);
    }
    
    public saveInvoices(invoices: InvoiceResponse[]): Observable<any> {
        return this.http.post(environment.API_URL + '/Invoices/bulk', invoices);
    }

    public exportInvoicesToExcel(): Observable<any> {
        return this.http.post(environment.API_URL + `/Invoices/export`, null, { responseType: 'blob' });
    }
}
